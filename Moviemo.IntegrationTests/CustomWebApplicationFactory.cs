using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Mvc.Testing;
using Microsoft.Data.Sqlite;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.DependencyInjection.Extensions;
using Microsoft.Extensions.Logging;
using Moviemo.API.Data;
using Moviemo.IntegrationTests.Extensions;
using System;

namespace Moviemo.IntegrationTests
{
    public class CustomWebApplicationFactory<TStartup>
        : WebApplicationFactory<TStartup> where TStartup : class
    {
        private IServiceProvider serviceProvider;
        private readonly SqliteConnection connection;
        private readonly string connectionString = ":memory:";

        public CustomWebApplicationFactory ()
        {
            var builder = new SqliteConnectionStringBuilder()
            {
                DataSource = connectionString,
                Mode = SqliteOpenMode.Memory,
                Cache = SqliteCacheMode.Shared
            };

            connection = new SqliteConnection(builder.ConnectionString);
            connection.Open();
            connection.EnableExtensions();
        }

        protected override void ConfigureWebHost (IWebHostBuilder builder)
        {
            if (builder is null)
            {
                throw new ArgumentNullException(nameof(builder));
            }

            builder.ConfigureServices(services =>
            {
                services.RemoveAll(typeof(MoviemoContext));
                services.AddDbContext<MoviemoContext>(options => options.UseSqlite(connection));

                serviceProvider = services.BuildServiceProvider();

                using var scope = serviceProvider.CreateScope();
                var db = scope.ServiceProvider.GetRequiredService<MoviemoContext>();

                var logger = scope.ServiceProvider
                    .GetRequiredService<ILogger<CustomWebApplicationFactory<TStartup>>>();

                db.Database.OpenConnection();
                db.Database.EnsureCreated();

                try
                {
                    TestUtils.Reinitialize(db);
                }
                catch (DbUpdateException ex)
                {
                    logger.LogError(ex, "Error occurred seeding the test database.");
                }
            });
        }

        protected override void Dispose (bool disposing)
        {
            base.Dispose(disposing);
            connection.Close();

            using var scope = serviceProvider.CreateScope();
            var db = scope.ServiceProvider.GetRequiredService<MoviemoContext>();
            db.Database.EnsureDeleted();
        }
    }
}