using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Threading.Tasks;
using AutoMapper;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Cors;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.JsonPatch;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using MoviesAPI.DTOs;
using MoviesAPI.Entities;
using MoviesAPI.Helpers;
using MoviesAPI.Services;

namespace MoviesAPI.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class PeopleController : ControllerBase
    {
        private readonly ApplicationDbContext context;
        private readonly IMapper mapper;
        private readonly IFileStorageService fileStorageService;

        public PeopleController(ApplicationDbContext context, IMapper mapper, IFileStorageService fileStorageService)
        {
            this.context = context;
            this.mapper = mapper;
            this.fileStorageService = fileStorageService;
        }

        [HttpGet]
        [EnableCors(PolicyName = "AllowAPIRequestIO")]
        public async Task<ActionResult<List<PersonDTO>>> Get([FromQuery] PaginationDTO pagination)
        {
            var queryable = context.People.AsQueryable();
            await HttpContext.InsertPaginationParametersInResponse(queryable, pagination.RecordsPerPage);

            var people = queryable.Paginate(pagination).ToList();
            var peopleDTO = mapper.Map<List<PersonDTO>>(people);

            return peopleDTO;
        }

        [HttpGet("{personId:int}", Name = "getPerson")]
        public ActionResult<PersonDTO> Get(int personId)
        {
            var person = context.People.FirstOrDefault(c => c.Id == personId);

            if (person == null)
            {
                return NotFound();
            }

            var personDTO = mapper.Map<PersonDTO>(person);

            return personDTO;
        }

        [HttpPost]
        [Authorize(AuthenticationSchemes = JwtBearerDefaults.AuthenticationScheme, Roles = "Admin")]
        public async Task<ActionResult> Post([FromForm] PersonCreationDTO personCreation)
        {
            var person = mapper.Map<Person>(personCreation);

            if (personCreation.Picture != null)
            {
                using (var memoryStream = new MemoryStream())
                {
                    personCreation.Picture.CopyTo(memoryStream);
                    var content = memoryStream.ToArray();
                    var extension = Path.GetExtension(personCreation.Picture.FileName);

                    person.Picture = await fileStorageService.SaveFile(content, extension, "people", personCreation.Picture.ContentType);
                }
            }

            context.Add(person);
            context.SaveChanges();

            var personDTO = mapper.Map<PersonDTO>(person);

            return new CreatedAtRouteResult("getPerson", new { personId = personDTO.Id }, personDTO);
        }

        [HttpPut("{personId}")]
        [Authorize(AuthenticationSchemes = JwtBearerDefaults.AuthenticationScheme, Roles = "Admin")]
        public async Task<ActionResult> Put(int personId, [FromForm] PersonCreationDTO personCreation)
        {
            var personFromDb = await context.People.FirstOrDefaultAsync(c => c.Id == personId);

            if (personFromDb == null)
            {
                return NotFound();
            }

            personFromDb = mapper.Map(personCreation, personFromDb);

            if (personCreation.Picture != null)
            {
                using (var memoryStream = new MemoryStream())
                {
                    await personCreation.Picture.CopyToAsync(memoryStream);
                    var content = memoryStream.ToArray();
                    var extension = Path.GetExtension(personCreation.Picture.FileName);

                    personFromDb.Picture = await fileStorageService.EditFile(content, extension, "people", personFromDb.Picture, personCreation.Picture.ContentType);
                }
            }

            await context.SaveChangesAsync();

            return NoContent();
        }

        [HttpPatch("{personId}")]
        [Authorize(AuthenticationSchemes = JwtBearerDefaults.AuthenticationScheme, Roles = "Admin")]
        public async Task<ActionResult> Patch(int personId, [FromBody] JsonPatchDocument<PersonPatchDTO> patchDocument)
        {
            if (patchDocument == null)
            {
                return BadRequest();
            }

            var personFromDb = await context.People.FirstOrDefaultAsync(c => c.Id == personId);

            if (personFromDb == null)
            {
                return NotFound();
            }

            var personDTO = mapper.Map<PersonPatchDTO>(personFromDb);

            patchDocument.ApplyTo(personDTO, ModelState);

            var isValid = TryValidateModel(personDTO);

            if (!isValid)
            {
                return BadRequest();
            }

            mapper.Map(personDTO, personFromDb);

            await context.SaveChangesAsync();

            return NoContent();
        }

        [HttpDelete("{personId}")]
        [Authorize(AuthenticationSchemes = JwtBearerDefaults.AuthenticationScheme, Roles = "Admin")]
        public async Task<ActionResult> Delete(int personId)
        {
            var personFromDb = await context.People.FirstOrDefaultAsync(c => c.Id == personId);

            if (personFromDb == null)
            {
                return NotFound();
            }

            await fileStorageService.DeleteFile(personFromDb.Picture, "people");
            context.Remove(personFromDb);
            context.SaveChanges();

            return NoContent();
        }
    }
}