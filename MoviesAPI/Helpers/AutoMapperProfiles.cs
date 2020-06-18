using AutoMapper;
using Microsoft.AspNetCore.Identity;
using MoviesAPI.DTOs;
using MoviesAPI.Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace MoviesAPI.Helpers
{
    public class AutoMapperProfiles : Profile
    {
        public AutoMapperProfiles()
        {
            CreateMap<Genre, GenreDTO>().ReverseMap();

            CreateMap<GenreCreationDTO, Genre>();

            CreateMap<Person, PersonDTO>().ReverseMap();

            CreateMap<PersonCreationDTO, Person>()
            .ForMember(c => c.Picture, options => options.Ignore());

            CreateMap<Person, PersonPatchDTO>().ReverseMap();

            CreateMap<Movie, MovieDTO>().ReverseMap();

            CreateMap<MovieCreationDTO, Movie>()
            .ForMember(c => c.Poster, options => options.Ignore())
            .ForMember(c => c.MoviesGeneres, options => options.MapFrom(MapMoviesGenres))
            .ForMember(c => c.MoviesActors, options => options.MapFrom(MapMoviesActors));

            CreateMap<Movie, MovieDetailsDTO>()
            .ForMember(c => c.Genres, options => options.MapFrom(MapMoviesGenres))
            .ForMember(c => c.Actors, options => options.MapFrom(MapMoviesActors));

            CreateMap<Movie, MoviePatchDTO>().ReverseMap();

            CreateMap<IdentityUser, UserDTO>()
                .ForMember(c => c.Email, option => option.MapFrom(x => x.Email))
                .ForMember(c => c.UserId, option => option.MapFrom(x => x.Id));
        }

        private List<GenreDTO> MapMoviesGenres(Movie movie, MovieDetailsDTO movieDetailsDTO)
        {
            var result = new List<GenreDTO>();

            foreach (var movieGenre in movie.MoviesGeneres)
            {
                result.Add(new GenreDTO() { Id = movieGenre.GenreId, Name = movieGenre.Genre.Name });
            }

            return result;
        }

        private List<ActorDTO> MapMoviesActors(Movie movie, MovieDetailsDTO movieDetailsDTO)
        {
            var result = new List<ActorDTO>();

            foreach (var actor in movie.MoviesActors)
            {
                result.Add(new ActorDTO() { PersonId = actor.PersonId, Character = actor.Character, PersonName = actor.Person.Name });
            }

            return result;
        }

        private List<MoviesGeneres> MapMoviesGenres(MovieCreationDTO movieCreationDTO, Movie movie)
        {
            var result = new List<MoviesGeneres>();

            foreach (var id in movieCreationDTO.GeneresIds)
            {
                result.Add(new MoviesGeneres() { GenreId = id });
            }

            return result;
        }

        private List<MoviesActors> MapMoviesActors(MovieCreationDTO movieCreationDTO, Movie movie)
        {
            var result = new List<MoviesActors>();

            foreach (var actor in movieCreationDTO.Actors)
            {
                result.Add(new MoviesActors() { PersonId = actor.PersonId, Character = actor.Character });
            }

            return result;
        }
    }
}