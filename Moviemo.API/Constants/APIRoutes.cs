using System.Diagnostics.CodeAnalysis;

namespace Moviemo.API.Constants
{
    [SuppressMessage("Design", "CA1034:Nested types should not be visible",
        Justification = "Association easier seen with nested class as opposed to namespace")]
    public static class APIRoutes
    {
        public const string Root = "api/";

        public static class Genres
        {
            public const string GetGenre = Root + "genres/{id}";
            public const string PutGenre = Root + "genres/{id}";
            public const string GetGenres = Root + "genres";
            public const string PostGenre = Root + "genres";
            public const string DeleteGenre = Root + "genres/{id}";
        }

        public static class Movies
        {
            public const string GetMovie = Root + "movies/{id}";
            public const string PutMovie = Root + "movies/{id}";
            public const string GetMovies = Root + "movies";
            public const string PostMovie = Root + "movies";
            public const string DeleteMovie = Root + "movies/{id}";
        }
    }
}
