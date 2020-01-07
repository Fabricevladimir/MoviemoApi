using FluentAssertions;
using Microsoft.EntityFrameworkCore;
using Moviemo.API.Data;
using Moviemo.API.Models;
using Moviemo.API.Services;
using Moviemo.IntegrationTests.Setup;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Xunit;

namespace Moviemo.UnitTests.Services
{
    public sealed class MovieServiceTest : BaseServiceUnitTest, IDisposable
    {
        private const int VALID_MOVIE_ID = 1;
        private const int VALID_GENRE_ID = 1;
        private const int INVALID_MOVIE_ID = 100;
        private const string VALID_MOVIE_TITLE = "Avengers";
        private const string INVALID_MOVIE_TITLE = "Invalid";

        private readonly List<Movie> Movies;
        private IMovieService movieService;

        public MovieServiceTest ()
        {
            Movies = TestData.Movies;
        }

        [Theory]
        [InlineData(VALID_MOVIE_ID, true)]
        [InlineData(INVALID_MOVIE_ID, false)]
        public void MovieExistsShouldFindMovieById (int id, bool expected)
        {
            using var repository = new MoviemoContext(options);
            movieService = new MovieService(repository);

            movieService.MovieExists(id).Should().Be(expected);
        }

        [Theory]
        [InlineData(VALID_MOVIE_TITLE, true)]
        [InlineData(INVALID_MOVIE_TITLE, false)]
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
            var movie = Movies.First();
            using var repository = new MoviemoContext(options);
            movieService = new MovieService(repository);

            await movieService.RemoveMovieAsync(movie);

            using var newRepoInstance = new MoviemoContext(options);
            var movies = await newRepoInstance.Movies.ToListAsync();
            movies.Should().HaveCount(Movies.Count - 1);
            movies.Should().NotContain(movie);
        }

        [Fact]
        public async Task AddMovieAsyncShouldAddOneMovie ()
        {
            var newMovie = new Movie { GenreId = 1, Title = "CDEF" };
            using var repository = new MoviemoContext(options);
            movieService = new MovieService(repository);

            await movieService.AddMovieAsync(newMovie);

            using var newRepoInstance = new MoviemoContext(options);
            var movies = await newRepoInstance.Movies.ToListAsync();
            movies.Should().HaveCount(Movies.Count + 1);
            movies.Should().Contain(movie => movie.Title == newMovie.Title);
        }

        [Fact]
        public void RemoveMovieAsyncShouldThrowWhenRemovingInValidMovie ()
        {
            var movie = new Movie() { Id = INVALID_MOVIE_ID };
            using var repository = new MoviemoContext(options);
            movieService = new MovieService(repository);

            Func<Task> sutMethod = async () =>
                await movieService.RemoveMovieAsync(movie);

            sutMethod.Should().Throw<DbUpdateConcurrencyException>();
        }

        [Fact]
        public async Task GetAllMoviesAsyncShouldReturnAllMovies ()
        {
            using var repository = new MoviemoContext(options);
            movieService = new MovieService(repository);

            var movies = await movieService.GetAllMoviesAsync();

            movies.Should().HaveCount(Movies.Count);
        }

        public void Dispose ()
        {
            connection.Close();
            connection.Dispose();
        }

        private static readonly Movie Movie = new Movie()
        {
            Id = VALID_MOVIE_ID,
            Title = VALID_MOVIE_TITLE,
            GenreId = VALID_GENRE_ID
        };

        public static IEnumerable<object[]> GetMovieTestData =>
            new List<object[]>
            {
                new object[] { VALID_MOVIE_ID, Movie },
                new object[] { INVALID_MOVIE_ID, null }
            };
    }
}
