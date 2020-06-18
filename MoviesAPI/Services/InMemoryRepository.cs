using MoviesAPI.Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace MoviesAPI.Services
{
    public class InMemoryRepository : IRepository
    {
        private List<Genre> _genres;

        public InMemoryRepository()
        {
            _genres = new List<Genre>()
            {
                new Genre() { Id = 1, Name = "Comedy" },
                new Genre() { Id = 2, Name = "Action" }
            };
        }

        public List<Genre> GetAllGenres()
        {
            return _genres;
        }

        public Genre GetGenreById(int genreId)
        {
            return _genres.FirstOrDefault(c => c.Id == genreId);
        }

        public void AddGenre(Genre genre)
        {
            genre.Id = _genres.Max(c => c.Id) + 1;
            _genres.Add(genre);
        }
    }
}