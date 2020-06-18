using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Threading.Tasks;
using AutoMapper;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using MoviesAPI.DTOs;
using MoviesAPI.Entities;
using MoviesAPI.Services;

namespace MoviesAPI.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class GenresController : ControllerBase
    {
        private readonly ApplicationDbContext context;
        private readonly IMapper mapper;

        public GenresController(ApplicationDbContext context, IMapper mapper)
        {
            this.context = context;
            this.mapper = mapper;
        }

        [HttpGet]
        public ActionResult<List<GenreDTO>> Get()
        {
            var genres = context.Genres.ToList();
            var genresDTO = mapper.Map<List<GenreDTO>>(genres);

            return genresDTO;
        }

        /// <summary>
        /// Get genre by Id
        /// </summary>
        /// <param name="genreId">Genre Id</param>
        /// <returns></returns>
        [HttpGet("{genreId:int}", Name = "getGenre")]
        [ProducesResponseType(404)]
        [ProducesResponseType(typeof(GenreDTO), 200)]
        public ActionResult<GenreDTO> Get(int genreId)
        {
            var genre = context.Genres.FirstOrDefault(c => c.Id == genreId);

            if (genre == null)
            {
                return NotFound();
            }

            var genreDTO = mapper.Map<GenreDTO>(genre);

            return genreDTO;
        }

        [HttpPost]
        [Authorize(AuthenticationSchemes = JwtBearerDefaults.AuthenticationScheme, Roles = "Admin")]
        public ActionResult Post([FromBody] GenreCreationDTO genreCreation)
        {
            var genre = mapper.Map<Genre>(genreCreation);

            context.Add(genre);
            context.SaveChanges();

            var genreDTO = mapper.Map<GenreDTO>(genre);

            return new CreatedAtRouteResult("getGenre", new { genreId = genreDTO.Id }, genreDTO);
        }

        [HttpPut("{genreId:int}")]
        [Authorize(AuthenticationSchemes = JwtBearerDefaults.AuthenticationScheme, Roles = "Admin")]
        public ActionResult Put(int genreId, [FromBody] GenreCreationDTO genreCreation)
        {
            var genre = mapper.Map<Genre>(genreCreation);
            genre.Id = genreId;

            context.Entry(genre).State = EntityState.Modified;
            context.SaveChanges();

            return NoContent();
        }

        [HttpDelete("{genreId:int}")]
        [Authorize(AuthenticationSchemes = JwtBearerDefaults.AuthenticationScheme, Roles = "Admin")]
        public ActionResult Delete(int genreId)
        {
            var exists = context.Genres.Any(c => c.Id == genreId);

            if (!exists)
            {
                return NotFound();
            }

            context.Remove(new Genre() { Id = genreId });
            context.SaveChanges();

            return NoContent();
        }
    }
}