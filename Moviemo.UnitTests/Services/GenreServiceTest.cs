using FluentAssertions;
using Microsoft.EntityFrameworkCore;
using Moviemo.API.Data;
using Moviemo.API.Models;
using Moviemo.API.Services;
using Moviemo.IntegrationTests.Extensions;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Xunit;

namespace Moviemo.UnitTests.Services
{
    public sealed class GenreServiceTest : BaseServiceUnitTest, IDisposable
    {
        private const int VALID_GENRE_ID = 1;
        private const int INVALID_GENRE_ID = 100;
        private const string VALID_GENRE_NAME = "Action";
        private const string INVALID_GENRE_NAME = "Invalid";

        private readonly List<Genre> Genres;
        private IGenreService genreService;

        public GenreServiceTest ()
        {
            Genres = TestUtils.GetGenres();
        }

        [Theory]
        [InlineData(VALID_GENRE_ID, true)]
        [InlineData(INVALID_GENRE_ID, false)]
        public void GenreExistsShouldFindGenreById (int id, bool expected)
        {
            using var repository = new MoviemoContext(options);
            genreService = new GenreService(repository);

            genreService.GenreExists(id).Should().Be(expected);
        }

        [Theory]
        [InlineData(VALID_GENRE_NAME, true)]
        [InlineData(INVALID_GENRE_NAME, false)]
        public void GenreExistsShouldFindGenreByName (string name, bool expected)
        {
            using var repository = new MoviemoContext(options);
            genreService = new GenreService(repository);

            genreService.GenreExists(name).Should().Be(expected);
        }

        [Theory]
        [MemberData(nameof(GetGenreTestData))]
        public async Task GetGenreAsyncShouldReturnGenreWithGivenId (int id, Genre expected)
        {
            using var repository = new MoviemoContext(options);
            genreService = new GenreService(repository);

            var result = await genreService.GetGenreAsync(id);

            result.Should().BeEquivalentTo(expected);
        }

        [Fact]
        public async Task RemoveGenreAsyncShouldRemoveGenre ()
        {
            var genre = Genres.First();
            using var repository = new MoviemoContext(options);
            genreService = new GenreService(repository);

            await genreService.RemoveGenreAsync(genre);

            // Assert on different context
            using var newRepositoryInstance = new MoviemoContext(options);

            var genres = await newRepositoryInstance.Genres.ToListAsync();
            genres.Should().HaveCount(Genres.Count - 1);
            genres.Should().NotContain(genre);
        }

        [Fact]
        public async Task AddGenreAsyncShouldAddOneGenre ()
        {
            var newGenre = new Genre() { Name = "ABC" };
            using var repository = new MoviemoContext(options);
            genreService = new GenreService(repository);

            await genreService.AddGenreAsync(newGenre);

            // Assert on different context
            using var newRepositoryInstance = new MoviemoContext(options);

            var genres = await newRepositoryInstance.Genres.ToListAsync();
            genres.Should().HaveCount(Genres.Count + 1);
            genres.Should().Contain(genre => genre.Name == newGenre.Name);
        }


        [Fact]
        public void RemoveGenreAsyncShouldThrowWhenRemovingInvalidGenre ()
        {
            var genre = new Genre() { Id = INVALID_GENRE_ID };
            using var repository = new MoviemoContext(options);
            genreService = new GenreService(repository);

            Func<Task> sutMethod = async () =>
                await genreService.RemoveGenreAsync(genre);

            sutMethod.Should().Throw<DbUpdateConcurrencyException>();
        }

        [Fact]
        public async Task GetAllGenresAsyncShouldReturnAllGenres ()
        {
            using var repository = new MoviemoContext(options);
            genreService = new GenreService(repository);

            var genres = await genreService.GetAllGenresAsync();

            genres.Should().HaveCount(Genres.Count);
        }

        public void Dispose ()
        {
            connection.Close();
            connection.Dispose();
        }

        private static readonly Genre Genre = new Genre() { Id = VALID_GENRE_ID, Name = VALID_GENRE_NAME };
        public static IEnumerable<object[]> GetGenreTestData =>
            new List<object[]>
            {
                new object[] { VALID_GENRE_ID, Genre },
                new object[] { INVALID_GENRE_ID, null }
            };
    }
}