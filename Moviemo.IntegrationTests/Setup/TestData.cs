using Moviemo.API.Data;
using Moviemo.API.Models;
using System.Collections.Generic;

namespace Moviemo.IntegrationTests.Setup
{
    public class TestData
    {
        private readonly MoviemoContext context;

        public TestData (MoviemoContext context)
        {
            this.context = context;
            context.Database.EnsureDeleted();
            context.Database.EnsureCreated();
        }

        public void SeedDatabase ()
        {
            context.Genres.AddRange(Genres);
            context.Movies.AddRange(Movies);
            context.SaveChanges();
        }

        public static readonly List<Genre> Genres = new List<Genre>()
        {
            new Genre () { Id = 1, Name = "Action"},
            new Genre () { Id = 2, Name = "Drama"},
            new Genre () { Id = 3, Name = "Comedy"},
            new Genre () { Id = 4, Name = "Romance"}
        };

        public static readonly List<Movie> Movies = new List<Movie>()
        {
           new Movie() { Id = 1, GenreId = 1, Title = "Avengers"},
           new Movie() { Id = 2, GenreId = 2, Title = "The Rainmaker"},
           new Movie() { Id = 3, GenreId = 3, Title = "Jumanji"},
           new Movie() { Id = 4, GenreId = 4, Title = "Me Before You"}
        };
    }
}

