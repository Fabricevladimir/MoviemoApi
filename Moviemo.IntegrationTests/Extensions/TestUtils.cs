using Moviemo.API.Data;
using Moviemo.API.Models;
using System.Collections.Generic;

namespace Moviemo.IntegrationTests.Extensions
{
    public static class TestUtils
    {
        public const int InvalidGenreId = 100;

        public static void Init (MoviemoContext context)
        {
            if (context is null)
            {
                throw new System.ArgumentNullException(nameof(context));
            }

            context.Genres.AddRange(GetGenres());
            context.SaveChanges();
        }

        public static void Reinitialize (MoviemoContext context)
        {
            if (context is null)
            {
                throw new System.ArgumentNullException(nameof(context));
            }

            if (context.Genres != null)
                context.Genres.RemoveRange(context.Genres);

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

        public static List<GenreResource> GetGenreResources ()
        {
            return new List<GenreResource>()
            {
                new GenreResource() { Id = 1, Name = "Action"},
                new GenreResource() { Id = 2, Name = "Drama"},
                new GenreResource() { Id = 3, Name = "Comedy"},
                new GenreResource() { Id = 4, Name = "Romance"}
            };
        }
    }
}