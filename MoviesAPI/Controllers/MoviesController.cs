using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Threading.Tasks;
using AutoMapper;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.JsonPatch;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using MoviesAPI.DTOs;
using MoviesAPI.Entities;
using MoviesAPI.Helpers;
using MoviesAPI.Services;
using System.Linq.Dynamic.Core;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Authentication.JwtBearer;

namespace MoviesAPI.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class MoviesController : ControllerBase
    {
        private readonly ApplicationDbContext context;
        private readonly IMapper mapper;
        private readonly IFileStorageService fileStorageService;

        public MoviesController(ApplicationDbContext context, IMapper mapper, IFileStorageService fileStorageService)
        {
            this.context = context;
            this.mapper = mapper;
            this.fileStorageService = fileStorageService;
        }

        [HttpGet]
        public async Task<ActionResult<IndexMoviePageDTO>> Get()
        {
            var top = 6;
            var today = DateTime.Today;

            var upcomingReleases = await context.Movies
                .Where(c => c.ReleaseDate > today)
                .OrderBy(c => c.ReleaseDate)
                .Take(top)
                .ToListAsync();

            var inTheaters = await context.Movies
                .Where(c => c.InTheathers)
                .Take(top)
                .ToListAsync();

            var result = new IndexMoviePageDTO();
            result.InTheaters = mapper.Map<List<MovieDTO>>(inTheaters);
            result.UpcomingReleases = mapper.Map<List<MovieDTO>>(upcomingReleases);

            return result;
        }

        [HttpGet("filter")]
        public async Task<ActionResult<List<MovieDTO>>> Filter([FromQuery] FilterMovieDTO filterMovieDTO)
        {
            var movieQueryable = context.Movies.AsQueryable();

            if (!string.IsNullOrWhiteSpace(filterMovieDTO.Title))
            {
                movieQueryable = movieQueryable.Where(c => c.Title.Contains(filterMovieDTO.Title));
            }

            if (filterMovieDTO.InTheaters)
            {
                movieQueryable = movieQueryable.Where(c => c.InTheathers);
            }

            if (filterMovieDTO.UpcomingReleases)
            {
                var today = DateTime.Today;
                movieQueryable = movieQueryable.Where(c => c.ReleaseDate > today);
            }

            if (filterMovieDTO.GenreId != 0)
            {
                movieQueryable = movieQueryable
                    .Where(c => c.MoviesGeneres.Select(d => d.GenreId).Contains(filterMovieDTO.GenreId));
            }

            if (!string.IsNullOrWhiteSpace(filterMovieDTO.OrderingField))
            {
                try
                {
                    movieQueryable = movieQueryable
                    .OrderBy($"{filterMovieDTO.OrderingField} {(filterMovieDTO.Ascending ? "ascending" : "descending")}");
                }
                catch
                {
                    //Log Error
                }
            }

            await HttpContext.InsertPaginationParametersInResponse(movieQueryable, filterMovieDTO.RecordPerPage);

            var movies = await movieQueryable.Paginate(filterMovieDTO.Pagination).ToListAsync();

            return mapper.Map<List<MovieDTO>>(movies);
        }

        [HttpGet("{id}", Name = "getMovie")]
        public async Task<ActionResult<MovieDetailsDTO>> Get(int id)
        {
            var movie = await context.Movies
                .Include(c => c.MoviesActors).ThenInclude(c => c.Person)
                .Include(c => c.MoviesGeneres).ThenInclude(c => c.Genre)
                .FirstOrDefaultAsync(c => c.Id == id);

            if (movie == null)
            {
                return NotFound();
            }

            return mapper.Map<MovieDetailsDTO>(movie);
        }

        [HttpPost]
        [Authorize(AuthenticationSchemes = JwtBearerDefaults.AuthenticationScheme, Roles = "Admin")]
        public async Task<ActionResult> Post([FromForm] MovieCreationDTO movieCreation)
        {
            var movie = mapper.Map<Movie>(movieCreation);

            if (movieCreation.Poster != null)
            {
                using (var memoryStream = new MemoryStream())
                {
                    movieCreation.Poster.CopyTo(memoryStream);
                    var content = memoryStream.ToArray();
                    var extension = Path.GetExtension(movieCreation.Poster.FileName);

                    movie.Poster = await fileStorageService.SaveFile(content, extension, "movies", movieCreation.Poster.ContentType);
                }
            }

            AnnotateActorOrder(movie);

            context.Add(movie);
            context.SaveChanges();

            var movieDTO = mapper.Map<MovieDTO>(movie);

            return new CreatedAtRouteResult("getMovie", new { id = movie.Id }, movieDTO);
        }

        private static void AnnotateActorOrder(Movie movie)
        {
            if (movie.MoviesActors != null)
            {
                for (var i = 0; i < movie.MoviesActors.Count; i++)
                {
                    movie.MoviesActors[i].Order = i;
                }
            }
        }

        [HttpPut("{id}")]
        [Authorize(AuthenticationSchemes = JwtBearerDefaults.AuthenticationScheme, Roles = "Admin")]
        public async Task<ActionResult> Put(int id, [FromForm] MovieCreationDTO movieCreation)
        {
            var movieFromDb = await context.Movies.FirstOrDefaultAsync(c => c.Id == id);

            if (movieFromDb == null)
            {
                return NotFound();
            }

            movieFromDb = mapper.Map(movieCreation, movieFromDb);

            if (movieCreation.Poster != null)
            {
                using (var memoryStream = new MemoryStream())
                {
                    await movieCreation.Poster.CopyToAsync(memoryStream);
                    var content = memoryStream.ToArray();
                    var extension = Path.GetExtension(movieCreation.Poster.FileName);

                    movieFromDb.Poster = await fileStorageService.EditFile(content, extension, "movies", movieFromDb.Poster, movieCreation.Poster.ContentType);
                }
            }

            List<MoviesGeneres> moviesGeneres = context.MoviesGeneres.Where(c => c.MovieId == movieFromDb.Id).ToList();
            List<MoviesActors> moviesActors = context.MoviesActors.Where(c => c.MovieId == movieFromDb.Id).ToList();

            context.RemoveRange(moviesActors);
            context.RemoveRange(moviesGeneres);

            AnnotateActorOrder(movieFromDb);

            await context.SaveChangesAsync();

            return NoContent();
        }

        [HttpDelete("{id}")]
        [Authorize(AuthenticationSchemes = JwtBearerDefaults.AuthenticationScheme, Roles = "Admin")]
        public async Task<ActionResult> Delete(int id)
        {
            var movieFromDb = await context.Movies.FirstOrDefaultAsync(c => c.Id == id);

            if (movieFromDb == null)
            {
                return NotFound();
            }

            await fileStorageService.DeleteFile(movieFromDb.Poster, "movies");
            context.Remove(movieFromDb);
            context.SaveChanges();

            return NoContent();
        }

        [HttpPatch("{id}")]
        [ProducesResponseType(400)]
        [ProducesResponseType(404)]
        [Authorize(AuthenticationSchemes = JwtBearerDefaults.AuthenticationScheme, Roles = "Admin")]
        public async Task<ActionResult> Patch(int id, [FromBody] JsonPatchDocument<MoviePatchDTO> patchDocument)
        {
            if (patchDocument == null)
            {
                return BadRequest();
            }

            var movieFromDb = await context.Movies.FirstOrDefaultAsync(c => c.Id == id);

            if (movieFromDb == null)
            {
                return NotFound();
            }

            var movieDTO = mapper.Map<MoviePatchDTO>(movieFromDb);

            patchDocument.ApplyTo(movieDTO, ModelState);

            var isValid = TryValidateModel(movieDTO);

            if (!isValid)
            {
                return BadRequest();
            }

            mapper.Map(movieDTO, movieFromDb);

            await context.SaveChangesAsync();

            return NoContent();
        }
    }
}