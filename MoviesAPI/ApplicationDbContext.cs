﻿using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore;
using MoviesAPI.Entities;
using System;
using System.Collections.Generic;
using System.Diagnostics.CodeAnalysis;
using System.Linq;
using System.Threading.Tasks;

namespace MoviesAPI
{
    public class ApplicationDbContext : IdentityDbContext
    {
        public ApplicationDbContext([NotNullAttribute] DbContextOptions options) : base(options)
        {
        }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            modelBuilder.Entity<MoviesGeneres>().HasKey(c => new { c.MovieId, c.GenreId });
            modelBuilder.Entity<MoviesActors>().HasKey(c => new { c.MovieId, c.PersonId });

            SeedData(modelBuilder);

            base.OnModelCreating(modelBuilder);
        }

        private void SeedData(ModelBuilder modelBuilder)
        {
            var adventure = new Genre() { Id = 4, Name = "Adventure" };
            var animation = new Genre() { Id = 5, Name = "Animation" };
            var drama = new Genre() { Id = 6, Name = "Drama" };
            var romance = new Genre() { Id = 7, Name = "Romance" };

            modelBuilder.Entity<Genre>()
                .HasData(new List<Genre>
                {
                    adventure, animation, drama, romance
                });

            var jimCarrey = new Person() { Id = 8, Name = "Jim Carrey", DateOfBirth = new DateTime(1962, 01, 17) };
            var robertDowney = new Person() { Id = 9, Name = "Robert Downey Jr.", DateOfBirth = new DateTime(1965, 4, 4) };
            var chrisEvans = new Person() { Id = 10, Name = "Chris Evans", DateOfBirth = new DateTime(1981, 06, 13) };

            modelBuilder.Entity<Person>()
                .HasData(new List<Person>
                {
                    jimCarrey, robertDowney, chrisEvans
                });

            var endgame = new Movie()
            {
                Id = 10,
                Title = "Avengers: Endgame",
                InTheathers = true,
                ReleaseDate = new DateTime(2019, 04, 26)
            };

            var iw = new Movie()
            {
                Id = 11,
                Title = "Avengers: Infinity Wars",
                InTheathers = false,
                ReleaseDate = new DateTime(2019, 04, 26)
            };

            var sonic = new Movie()
            {
                Id = 12,
                Title = "Sonic the Hedgehog",
                InTheathers = false,
                ReleaseDate = new DateTime(2020, 02, 28)
            };
            var emma = new Movie()
            {
                Id = 13,
                Title = "Emma",
                InTheathers = false,
                ReleaseDate = new DateTime(2020, 02, 21)
            };
            var greed = new Movie()
            {
                Id = 14,
                Title = "Greed",
                InTheathers = false,
                ReleaseDate = new DateTime(2020, 02, 21)
            };

            modelBuilder.Entity<Movie>()
                .HasData(new List<Movie>
                {
                    endgame, iw, sonic, emma, greed
                });

            modelBuilder.Entity<MoviesGeneres>().HasData(
                new List<MoviesGeneres>()
                {
                    new MoviesGeneres(){MovieId = endgame.Id, GenreId = drama.Id},
                    new MoviesGeneres(){MovieId = endgame.Id, GenreId = adventure.Id},
                    new MoviesGeneres(){MovieId = iw.Id, GenreId = drama.Id},
                    new MoviesGeneres(){MovieId = iw.Id, GenreId = adventure.Id},
                    new MoviesGeneres(){MovieId = sonic.Id, GenreId = adventure.Id},
                    new MoviesGeneres(){MovieId = emma.Id, GenreId = drama.Id},
                    new MoviesGeneres(){MovieId = emma.Id, GenreId = romance.Id},
                    new MoviesGeneres(){MovieId = greed.Id, GenreId = drama.Id},
                    new MoviesGeneres(){MovieId = greed.Id, GenreId = romance.Id},
                });

            modelBuilder.Entity<MoviesActors>().HasData(
                new List<MoviesActors>()
                {
                    new MoviesActors(){MovieId = endgame.Id, PersonId = robertDowney.Id, Character = "Tony Stark", Order = 1},
                    new MoviesActors(){MovieId = endgame.Id, PersonId = chrisEvans.Id, Character = "Steve Rogers", Order = 2},
                    new MoviesActors(){MovieId = iw.Id, PersonId = robertDowney.Id, Character = "Tony Stark", Order = 1},
                    new MoviesActors(){MovieId = iw.Id, PersonId = chrisEvans.Id, Character = "Steve Rogers", Order = 2},
                    new MoviesActors(){MovieId = sonic.Id, PersonId = jimCarrey.Id, Character = "Dr. Ivo Robotnik", Order = 1}
                });
        }

        public DbSet<Genre> Genres { get; set; }
        public DbSet<Person> People { get; set; }
        public DbSet<Movie> Movies { get; set; }
        public DbSet<MoviesGeneres> MoviesGeneres { get; set; }
        public DbSet<MoviesActors> MoviesActors { get; set; }
    }
}