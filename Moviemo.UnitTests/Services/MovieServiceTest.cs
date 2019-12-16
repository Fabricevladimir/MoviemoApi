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
    public sealed class MovieServiceTest : BaseServiceUnitTest, IDisposable
    {
        private IMovieService movieService;

        public void Dispose ()
        {
            connection.Close();
            connection.Dispose();
        }

        [Theory]
        [InlineData(Constants.VALID_MOVIE_ID, true)]
        [InlineData(Constants.INVALID_MOVIE_ID, false)]
        public void MovieExistsShouldFindMovieById (int id, bool expected)
        {
            using var repository = new MoviemoContext(options);
            movieService = new MovieService(repository);

            movieService.MovieExists(id).Should().Be(expected);
        }

        [Theory]
        [InlineData(Constants.VALID_MOVIE_TITLE, true)]
        [InlineData(Constants.INVALID_MOVIE_TITLE, false)]
        public void MovieExistsShouldFindMovieByName (string title, bool expected)
        {
            using var repository = new MoviemoContext(options);
            movieService = new MovieService(repository);

            movieService.MovieExists(title).Should().Be(expected);
        }

        [Theory]
        [MemberData(nameof(GetMovieTestData))]
        public async Task GetMovieAsyncShouldReturnMovieWithGivenId (int id, Movie expected)
        {
            using var repository = new MoviemoContext(options);
            movieService = new MovieService(repository);

            var result = await movieService.GetMovieAsync(id);

            result.Should().BeEquivalentTo(expected);
        }

        [Fact]
        public async Task RemoveMovieAsyncShouldRemoveMovie ()
        {
            using var repository = new MoviemoContext(options);
            movieService = new MovieService(repository);

            await movieService.RemoveMovieAsync(Constants.VALID_MOVIE);

            using var newRepoInstance = new MoviemoContext(options);
            var movies = await newRepoInstance.Movies.ToListAsync();
            movies.Should().HaveCount(Constants.INITIAL_MOVIE_COUNT - 1);
            movies.Should().NotContain(Constants.VALID_MOVIE);
        }

        [Fact]
        public void RemoveMovieAsyncShouldThrowWhenRemovingInValidMovie ()
        {
            using var repository = new MoviemoContext(options);
            movieService = new MovieService(repository);

            Func<Task> sutMethod = async () =>
                await movieService.RemoveMovieAsync(Constants.INVALID_MOVIE);

            sutMethod.Should().Throw<DbUpdateConcurrencyException>();
        }

        [Fact]
        public async Task AddMovieAsyncShouldAddValidMovie ()
        {
            var newMovie = new Movie { GenreId = 1, Title = "CDEF" };
            using var repository = new MoviemoContext(options);
            movieService = new MovieService(repository);

            await movieService.AddMovieAsync(newMovie);

            using var newRepoInstance = new MoviemoContext(options);
            var movies = await newRepoInstance.Movies.ToListAsync();
            movies.Should().HaveCount(Constants.INITIAL_MOVIE_COUNT + 1);
            movies.Should().Contain(movie => movie.Title == newMovie.Title);
        }

        [Fact]
        public async Task GetAllMoviesAsyncShouldReturnAllMovies ()
        {
            using var repository = new MoviemoContext(options);
            movieService = new MovieService(repository);

            var movies = await movieService.GetAllMoviesAsync();

            movies.Should().HaveCount(Constants.INITIAL_MOVIE_COUNT);
        }

        public static IEnumerable<object[]> GetMovieTestData =>
            new List<object[]>
            {
                new object[] {Constants.VALID_MOVIE_ID, Constants.VALID_MOVIE},
                new object[] {Constants.INVALID_MOVIE_ID, null}
            };
    }
}
