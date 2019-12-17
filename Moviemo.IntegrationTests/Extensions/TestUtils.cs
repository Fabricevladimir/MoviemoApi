using Moviemo.API.Data;
using Moviemo.API.Models;
using System.Collections.Generic;

namespace Moviemo.IntegrationTests.Extensions
{
    public static class TestUtils
    {
        public static void Init (MoviemoContext context)
        {
            if (context is null)
            {
                throw new System.ArgumentNullException(nameof(context));
            }

            context.Genres.AddRange(GetGenres());
            context.Movies.AddRange(GetMovies());
            context.SaveChanges();
        }

        public static void Reinitialize (MoviemoContext context)
        {
            if (context is null)
            {
                throw new System.ArgumentNullException(nameof(context));
            }

            if (context.Genres != null && context.Movies != null)
            {
                context.Genres.RemoveRange(context.Genres);
                context.Movies.RemoveRange(context.Movies);
            }

            Init(context);
        }

        public static List<Genre> GetGenres ()
        {
            return new List<Genre>()
            {
                new Genre() { Id = 1, Name = "Action"},
                new Genre() { Id = 2, Name = "Drama"},
                new Genre() { Id = 3, Name = "Comedy"},
                new Genre() { Id = 4, Name = "Romance"}
            };
        }

        public static List<Movie> GetMovies ()
        {
            return new List<Movie>()
            {
                new Movie() { Id = 1, GenreId = 1, Title = "Avengers"},
                new Movie() { Id = 2, GenreId = 2, Title = "The Rainmaker"},
                new Movie() { Id = 3, GenreId = 3, Title = "Jumanji"},
                new Movie() { Id = 4, GenreId = 4, Title = "Me Before You"}
            };
        }
    }
}