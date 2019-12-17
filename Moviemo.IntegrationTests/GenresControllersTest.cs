using AutoMapper;
using FluentAssertions;
using Moviemo.API;
using Moviemo.API.Constants;
using Moviemo.API.Mappings;
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

        private readonly List<Genre> Genres;
        private readonly List<GenreResource> GenreResources;

        public GenresControllerTests ()
        {
            var mapper = new MapperConfiguration(opt =>
                opt.AddProfile(new MappingProfiles()))
                .CreateMapper();

            Genres = TestUtils.GetGenres();
            GenreResources = mapper.Map<List<GenreResource>>(Genres);

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
            genres.Should().HaveCount(Genres.Count + 1);
            genres.Should().ContainEquivalentOf(addedGenre);
        }

        [Fact]
        public async Task GetAllShouldReturnAllGenres ()
        {
            (var response, var genres) = await GetAllGenresAsync();

            response.StatusCode.Should().Be(HttpStatusCode.OK);
            genres.Should().BeEquivalentTo(GenreResources);
        }

        [Fact]
        public async Task GetOneShouldReturnGenreWithGivenId ()
        {
            var genre = Genres.First();
            var expected = GenreResources.First();
            var response = await client
                .GetAsync(APIRoutes.Genres.GetGenre.Replace("{id}", genre.Id.ToString()));

            response.StatusCode.Should().Be(HttpStatusCode.OK);

            var returnedGenre = await response.Content.ReadAsJsonAsync<GenreResource>();
            returnedGenre.Should().BeEquivalentTo(expected);
        }

        [Fact]
        public async Task UpdateShouldUpdateGenre ()
        {
            var genre = GenreResources.First();
            var updatedGenre = new GenreResource() { Id = genre.Id, Name = "abc" };

            var response = await client.PutAsJsonAsync(APIRoutes.Genres.PutGenre
                .Replace("{id}", genre.Id.ToString()), updatedGenre);

            response.StatusCode.Should().Be(HttpStatusCode.NoContent);

            (var genresResponse, var genres) = await GetAllGenresAsync();
            genresResponse.StatusCode.Should().Be(HttpStatusCode.OK);
            genres.Should().HaveSameCount(Genres);
            genres.Should().ContainEquivalentOf(updatedGenre);
        }

        [Fact]
        public async Task DeleteShouldRemoveGenre ()
        {
            var genre = GenreResources.First();
            var response = await client.DeleteAsync(APIRoutes.Genres.DeleteGenre
                .Replace("{id}", genre.Id.ToString()));

            response.StatusCode.Should().Be(HttpStatusCode.OK);

            (var genresResponse, var genres) = await GetAllGenresAsync();
            genresResponse.StatusCode.Should().Be(HttpStatusCode.OK);
            genres.Should().HaveCount(Genres.Count - 1);
            genres.Should().NotContain(genre);
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
