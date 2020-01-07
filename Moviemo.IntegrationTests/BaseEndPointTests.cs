using FluentAssertions;
using Microsoft.AspNetCore.Mvc.Testing;
using Microsoft.Data.Sqlite;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.DependencyInjection;
using Moviemo.API;
using Moviemo.API.Constants;
using Moviemo.API.Data;
using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using Xunit;

namespace Moviemo.IntegrationTests
{
    public sealed class BaseEndPointTests : IClassFixture<WebApplicationFactory<Startup>>, IDisposable
    {
        private const string connectionString = "DataSource=:memory:";
        private readonly SqliteConnection connection;
        private readonly WebApplicationFactory<Startup> factory;

        public BaseEndPointTests (WebApplicationFactory<Startup> factory)
        {
            connection = new SqliteConnection(connectionString);
            connection.Open();

            this.factory = factory.WithWebHostBuilder(builder =>
            {
                builder.ConfigureServices(services =>
                {
                    services
                     .AddEntityFrameworkSqlite()
                     .AddDbContext<MoviemoContext>(options =>
                     {
                         options.UseSqlite(connection);
                         options.UseInternalServiceProvider(services.BuildServiceProvider());
                     });
                });
            });
        }

        public static readonly IEnumerable<object[]> ControllerEndpoints = new List<object[]>()
        {
            new object[] {APIRoutes.Genres.GetGenres},
            new object[] {APIRoutes.Movies.GetMovies},
            new object[] {APIRoutes.Genres.GetGenre.Replace("{id}", "1")},
            new object[] {APIRoutes.Movies.GetMovie.Replace("{id}", "1")}
        };

        [Theory]
        [MemberData(nameof(ControllerEndpoints))]
        public async Task ApiRouteReturnsSuccessWithContentType (string url)
        {
            var expectedContentType = "application/json; charset=utf-8";
            var client = factory.CreateClient();

            var response = await client.GetAsync(url);

            response.EnsureSuccessStatusCode();
            response.Content.Headers.ContentType.ToString().Should().Be(expectedContentType);
        }

        public void Dispose ()
        {
            connection.Close();
        }

    }
}
