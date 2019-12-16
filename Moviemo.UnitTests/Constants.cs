using Moviemo.API.Models;
using Moviemo.IntegrationTests.Extensions;

namespace Moviemo.UnitTests
{
    public static class Constants
    {
        public const int VALID_GENRE_ID = 1;
        public const int VALID_MOVIE_ID = 1;
        public const int INVALID_GENRE_ID = 100;
        public const int INVALID_MOVIE_ID = 100;
        public const string VALID_GENRE_NAME = "Action";
        public const string VALID_MOVIE_TITLE = "Jumanji";
        public const string INVALID_GENRE_NAME = "INVALID";
        public const string INVALID_MOVIE_TITLE = "INVALID";
        public static int INITIAL_GENRE_COUNT = TestUtils.GetGenres().Count;
        public static int INITIAL_MOVIE_COUNT = TestUtils.GetMovies().Count;
        public static GenreResource VALID_GENRERESOURCE = TestUtils.GetGenreResources()[0];
        public static Genre VALID_GENRE = TestUtils.GetGenres()[0];
        public static Movie VALID_MOVIE = TestUtils.GetMovies()[0];

        public static Genre INVALID_GENRE = new Genre()
        {
            Id = INVALID_GENRE_ID,
            Name = INVALID_GENRE_NAME
        };

        public static Movie INVALID_MOVIE = new Movie()
        {
            Id = INVALID_MOVIE_ID,
            GenreId = INVALID_GENRE_ID,
            Title = INVALID_MOVIE_TITLE
        };

    }
}
