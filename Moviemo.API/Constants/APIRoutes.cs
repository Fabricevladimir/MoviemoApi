using System.Diagnostics.CodeAnalysis;

namespace Moviemo.API.Constants
{
    public static class APIRoutes
    {
        public const string Root = "api/";

        [SuppressMessage("Design", "CA1034:Nested types should not be visible",
         Justification = "Association easier seen with nested class as opposed to namespace")]
        public static class Genres
        {
            public const string GetGenre = Root + "genres/{id}";
            public const string PutGenre = Root + "genres/{id}";
            public const string GetGenres = Root + "genres";
            public const string PostGenre = Root + "genres";
            public const string DeleteGenre = Root + "genres/{id}";
        }
    }
}
