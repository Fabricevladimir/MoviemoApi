using FluentAssertions;
using Moviemo.API;
using Moviemo.API.Constants;
using Moviemo.API.Models;
using Moviemo.IntegrationTests.Extensions;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Threading.Tasks;
using Xunit;

namespace Moviemo.IntegrationTests
{
    public sealed class GenresControllerTests : IDisposable
    {
        private readonly HttpClient client;
        private readonly CustomWebApplicationFactory<Startup> factory;

        public GenresControllerTests ()
        {
            factory = new CustomWebApplicationFactory<Startup>();
            client = factory.CreateClient();
        }

        [Fact]
        public async Task CreateShouldAddOne ()
        {
            var genre = new SaveGenreResource() { Name = "abc" };

            var response = await client.PostAsJsonAsync(APIRoutes.Genres.PostGenre, genre);
            response.StatusCode.Should().Be(HttpStatusCode.Created);

            var addedGenre = await response.Content.ReadAsJsonAsync<GenreResource>();

            (var genresResponse, var genres) = await GetAllGenresAsync();
            genresResponse.StatusCode.Should().Be(HttpStatusCode.OK);
            genres.Should().HaveCount(TestUtils.GetGenreResources().Count + 1);
            genres.Should().ContainEquivalentOf(addedGenre);
        }

        [Fact]
        public async Task GetAllShouldReturnAllGenres ()
        {
            (var response, var genres) = await GetAllGenresAsync();
            response.StatusCode.Should().Be(HttpStatusCode.OK);
            genres.Should().BeEquivalentTo(TestUtils.GetGenreResources());
        }

        [Fact]
        public async Task GetOneShouldReturnGenreWithGivenId ()
        {
            var genre = TestUtils.GetGenres().First();

            var response = await client
                .GetAsync(APIRoutes.Genres.GetGenre.Replace("{id}", genre.Id.ToString()));

            response.StatusCode.Should().Be(HttpStatusCode.OK);

            var returnedGenre = await response.Content.ReadAsJsonAsync<GenreResource>();
            returnedGenre.Id.Should().Be(genre.Id);
            returnedGenre.Name.Should().Be(genre.Name);
        }

        [Fact]
        public async Task UpdateShouldUpdateGenre ()
        {
            var genre = TestUtils.GetGenreResources().First();
            var updatedGenre = new GenreResource() { Id = genre.Id, Name = "abc" };

            var response = await client.PutAsJsonAsync(APIRoutes.Genres.PutGenre
                .Replace("{id}", genre.Id.ToString()), updatedGenre);

            response.StatusCode.Should().Be(HttpStatusCode.NoContent);

            (var genresResponse, var genres) = await GetAllGenresAsync();
            genresResponse.StatusCode.Should().Be(HttpStatusCode.OK);
            genres.Should().HaveSameCount(TestUtils.GetGenreResources());
            genres.Should().ContainEquivalentOf(updatedGenre);
        }

        [Fact]
        public async Task DeleteShouldRemoveGenre ()
        {
            var genreToDelete = TestUtils.GetGenreResources().First();

            var response = await client.DeleteAsync(APIRoutes.Genres.DeleteGenre
                .Replace("{id}", genreToDelete.Id.ToString()));

            response.StatusCode.Should().Be(HttpStatusCode.NoContent);

            (var genresResponse, var genres) = await GetAllGenresAsync();
            genresResponse.StatusCode.Should().Be(HttpStatusCode.OK);
            genres.Should().HaveCount(TestUtils.GetGenreResources().Count - 1);
            genres.Should().NotContain(genreToDelete);
        }

        // This sooo reminds me of JavaScript!
        private async Task<(HttpResponseMessage, IEnumerable<GenreResource>)> GetAllGenresAsync ()
        {
            var genresResponse = await client.GetAsync(APIRoutes.Genres.GetGenres);
            var genres = await genresResponse.Content.ReadAsJsonAsync<List<GenreResource>>();

            return (genresResponse, genres);
        }

        public void Dispose ()
        {
            client.Dispose();
            factory.Dispose();
        }
    }
}
