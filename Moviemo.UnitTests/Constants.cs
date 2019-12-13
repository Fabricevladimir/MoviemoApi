using Moviemo.API.Models;
using Moviemo.IntegrationTests.Extensions;

namespace Moviemo.UnitTests
{
    public static class Constants
    {
        public const int VALID_GENRE_ID = 1;
        public const string VALID_GENRE_NAME = "Action";

        public const int INVALID_GENRE_ID = 100;
        public const string INVALID_GENRE_NAME = "INVALID";
        public static int INITIAL_COUNT = TestUtils.GetGenres().Count;

        public static GenreResource VALID_GENRERESOURCE = TestUtils.GetGenreResources()[0];
        public static Genre VALID_GENRE = TestUtils.GetGenres()[0];
        public static Genre INVALID_GENRE = new Genre()
        {
            Id = INVALID_GENRE_ID,
            Name = INVALID_GENRE_NAME
        };

    }
}
