using Microsoft.Data.Sqlite;
using Microsoft.EntityFrameworkCore;
using Moviemo.API.Data;
using Moviemo.IntegrationTests.Setup;

namespace Moviemo.UnitTests
{
    public class BaseServiceUnitTest
    {
        protected readonly SqliteConnection connection;
        protected readonly DbContextOptions<MoviemoContext> options;

        public BaseServiceUnitTest ()
        {
            connection = new SqliteConnection("DataSource=:memory:");
            connection.Open();

            options = new DbContextOptionsBuilder<MoviemoContext>()
                .UseSqlite(connection)
                .Options;

            using var context = new MoviemoContext(options);
            var db = new TestData(context);
            db.SeedDatabase();
        }
    }
}
