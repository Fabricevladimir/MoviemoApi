using FluentAssertions;
using Microsoft.EntityFrameworkCore;
using Moviemo.API.Data;
using Moviemo.API.Models;
using Moviemo.API.Services;
using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using Xunit;

namespace Moviemo.UnitTests.Services
{
    public sealed class GenreServiceTest : BaseServiceUnitTest, IDisposable
    {
        private IGenreService genreService;

        public void Dispose ()
        {
            connection.Close();
            connection.Dispose();
        }

        [Theory]
        [InlineData(Constants.VALID_GENRE_ID, true)]
        [InlineData(Constants.INVALID_GENRE_ID, false)]
        public void GenreExists_ShouldFindGenreById (int id, bool expected)
        {
            using var repository = new MoviemoContext(options);
            genreService = new GenreService(repository);

            genreService.GenreExists(id).Should().Be(expected);
        }

        [Theory]
        [InlineData(Constants.VALID_GENRE_NAME, true)]
        [InlineData(Constants.INVALID_GENRE_NAME, false)]
        public void GenreExists_ShouldFindGenreByName (string name, bool expected)
        {
            using var repository = new MoviemoContext(options);
            genreService = new GenreService(repository);

            genreService.GenreExists(name).Should().Be(expected);
        }

        [Theory]
        [MemberData(nameof(GetGenreTestData))]
        public async Task GetGenreAsync_ShouldReturnGenreWithGivenId (int id, Genre expected)
        {
            using var repository = new MoviemoContext(options);
            genreService = new GenreService(repository);

            var result = await genreService.GetGenreAsync(id);

            result.Should().BeEquivalentTo(expected);
        }

        [Fact]
        public async Task RemoveGenreAsync_ShouldRemoveValidGenre ()
        {
            using var repository = new MoviemoContext(options);
            genreService = new GenreService(repository);

            await genreService.RemoveGenreAsync(Constants.VALID_GENRE);

            // Assert on different context
            using var newRepositoryInstance = new MoviemoContext(options);

            var genres = await newRepositoryInstance.Genres.ToListAsync();
            genres.Should().HaveCount(Constants.INITIAL_COUNT - 1);
            genres.Should().NotContain(Constants.VALID_GENRE);
        }

        [Fact]
        public async Task AddGenreAsync_ShouldAddGenre ()
        {
            var genreToAdd = new Genre() { Name = "ABC" };
            using var repository = new MoviemoContext(options);
            genreService = new GenreService(repository);

            await genreService.AddGenreAsync(genreToAdd);

            // Assert on different context
            using var newRepositoryInstance = new MoviemoContext(options);

            var genres = await newRepositoryInstance.Genres.ToListAsync();
            genres.Should().HaveCount(Constants.INITIAL_COUNT + 1);
            genres.Should().Contain(genre => genre.Name == genreToAdd.Name);
        }


        [Fact]
        public void RemoveGenreAsync_ShouldThrowWhenRemovingInvalidGenre ()
        {
            using var repository = new MoviemoContext(options);
            genreService = new GenreService(repository);

            Func<Task> sutMethod = async () =>
                await genreService.RemoveGenreAsync(Constants.INVALID_GENRE);

            sutMethod.Should().Throw<DbUpdateConcurrencyException>();
        }

        [Fact]
        public async Task GetAllGenresAsync_ShouldReturnAllGenres ()
        {
            using var repository = new MoviemoContext(options);
            genreService = new GenreService(repository);

            var result = await genreService.GetAllGenresAsync();

            result.Should().HaveCount(Constants.INITIAL_COUNT);
        }

        public static IEnumerable<object[]> GetGenreTestData =>
            new List<object[]>
            {
                new object[] { Constants.VALID_GENRE_ID, Constants.VALID_GENRE },
                new object[] { Constants.INVALID_GENRE_ID, null }
            };
    }
}