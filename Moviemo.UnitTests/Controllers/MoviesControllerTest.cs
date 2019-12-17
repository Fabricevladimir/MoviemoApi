using FluentAssertions;
using Microsoft.AspNetCore.Mvc;
using Moq;
using Moviemo.API.Controllers;
using Moviemo.API.Models;
using Moviemo.API.Services;
using Moviemo.IntegrationTests.Extensions;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Xunit;

namespace Moviemo.UnitTests.Controllers
{
    public class MoviesControllerTest : BaseControllerUnitTest
    {
        private MoviesController controller;
        private readonly List<Movie> Movies;
        private readonly Mock<IGenreService> mockGenreService;
        private readonly Mock<IMovieService> mockMovieService;
        private readonly List<MovieResource> MovieResources;

        public MoviesControllerTest ()
        {
            Movies = TestUtils.GetMovies();
            MovieResources = mapper.Map<List<MovieResource>>(Movies);
            mockGenreService = new Mock<IGenreService>(MockBehavior.Strict);
            mockMovieService = new Mock<IMovieService>(MockBehavior.Strict);
        }

        [Fact]
        public async Task GetMoviesShouldReturnAllMovies ()
        {
            mockMovieService.Setup(s => s.GetAllMoviesAsync()).ReturnsAsync(Movies);
            controller = new MoviesController(mapper, mockMovieService.Object, mockGenreService.Object);

            var result = await controller.GetMovies();

            result.Result.Should().BeOfType(typeof(OkObjectResult));
            ((result.Result as OkObjectResult).Value as List<MovieResource>)
                .Should().HaveSameCount(Movies)
                .And.BeEquivalentTo(MovieResources);
        }

        [Fact]
        public async Task GetMovieShouldReturnNotFoundWhenMovieWithGivenIdNotFound ()
        {
            var invalidMovieId = 100;
            mockMovieService.Setup(s => s.GetMovieAsync(invalidMovieId))
                .ReturnsAsync((Movie)null);

            controller = new MoviesController(mapper, mockMovieService.Object, mockGenreService.Object);

            var result = await controller.GetMovie(invalidMovieId);

            result.Result.Should().BeOfType(typeof(NotFoundObjectResult));
            (result.Result as NotFoundObjectResult).Value.ToString()
                .Should().Contain(typeof(Movie).Name);
        }

        [Fact]
        public async Task GetMovieShouldReturnMovieWithGivenId ()
        {
            var movie = Movies.First();
            var expected = MovieResources.First();
            mockMovieService.Setup(s => s.GetMovieAsync(movie.Id))
                .ReturnsAsync(movie);

            controller = new MoviesController(mapper, mockMovieService.Object, mockGenreService.Object);

            var result = await controller.GetMovie(movie.Id);

            result.Result.Should().BeOfType(typeof(OkObjectResult));
            (result.Result as OkObjectResult).Value
                .Should().BeEquivalentTo(expected);
        }

        [Fact]
        public async Task PutMovieShouldReturnBadRequestWhenEndpointIdAndResourceIdDoNotMatch ()
        {
            var movie = new MovieResource() { Id = 1 };
            controller = new MoviesController(mapper, mockMovieService.Object, mockGenreService.Object);

            var result = await controller.PutMovie(movie.Id + 1, movie);

            result.Should().BeOfType(typeof(BadRequestObjectResult));
        }

        [Fact]
        public async Task PutMovieShouldReturnNotFoundWhenMovieWithGivenIdNotFound ()
        {
            var movie = new MovieResource() { Id = 1 };
            mockMovieService.Setup(s => s.GetMovieAsync(movie.Id))
                .ReturnsAsync((Movie)null);

            controller = new MoviesController(mapper, mockMovieService.Object, mockGenreService.Object);

            var result = await controller.PutMovie(movie.Id, movie);

            result.Should().BeOfType(typeof(NotFoundObjectResult));
            (result as NotFoundObjectResult).Value.ToString()
                .Should().Contain(typeof(Movie).Name);
        }

        [Fact]
        public async Task PutMovieShouldReturnNotFoundWhenGenreWithGivenIdNotFound ()
        {
            var movie = Movies.First();
            var updatedMovie = MovieResources.First();
            mockMovieService.Setup(s => s.GetMovieAsync(movie.Id)).ReturnsAsync(movie);
            mockGenreService.Setup(s => s.GenreExists(updatedMovie.GenreId)).Returns(false);
            controller = new MoviesController(mapper, mockMovieService.Object, mockGenreService.Object);

            var result = await controller.PutMovie(updatedMovie.Id, updatedMovie);

            result.Should().BeOfType(typeof(NotFoundObjectResult));
            (result as NotFoundObjectResult).Value.ToString()
                .Should().Contain(typeof(Genre).Name);
        }

        [Fact]
        public async Task PutMovieShouldReturnNoContentWhenSuccessful ()
        {
            var movie = Movies.First();
            var updatedMovie = MovieResources.First();
            mockMovieService.Setup(s => s.GetMovieAsync(movie.Id)).ReturnsAsync(movie);
            mockMovieService.Setup(s => s.UpdateAsync()).Returns(Task.CompletedTask);
            mockGenreService.Setup(s => s.GenreExists(updatedMovie.GenreId)).Returns(true);
            controller = new MoviesController(mapper, mockMovieService.Object, mockGenreService.Object);

            var result = await controller.PutMovie(updatedMovie.Id, updatedMovie);

            result.Should().BeOfType(typeof(NoContentResult));
        }

        [Fact]
        public async Task PostMovieShouldReturnNotFoundWhenGenreWithGivenIdNotFound ()
        {
            var movie = new SaveMovieResource() { GenreId = 1 };
            mockGenreService.Setup(s => s.GenreExists(movie.GenreId)).Returns(false);
            controller = new MoviesController(mapper, mockMovieService.Object, mockGenreService.Object);

            var result = await controller.PostMovie(movie);

            result.Result.Should().BeOfType(typeof(NotFoundObjectResult));
            (result.Result as NotFoundObjectResult).Value.ToString()
                .Should().Contain(typeof(Genre).Name);
        }

        [Fact]
        public async Task PostMovieShouldReturnCreatedMovie ()
        {
            var resource = new SaveMovieResource() { GenreId = 1, Title = "ABC" };
            var movie = mapper.Map<Movie>(resource);
            var expected = mapper.Map<MovieResource>(movie);

            mockGenreService.Setup(s => s.GenreExists(resource.GenreId)).Returns(true);
            mockMovieService.Setup(s => s.AddMovieAsync(It.IsAny<Movie>()))
                .Returns(Task.CompletedTask);

            controller = new MoviesController(mapper, mockMovieService.Object, mockGenreService.Object);

            var result = await controller.PostMovie(resource);

            result.Result.Should().BeOfType(typeof(CreatedAtActionResult));
            (result.Result as CreatedAtActionResult).Value
                .Should().BeEquivalentTo(expected);
        }

        [Fact]
        public async Task PostMovieShouldReturnRouteToCreatedMovie ()
        {
            var resource = new SaveMovieResource() { GenreId = 1, Title = "ABC" };
            var movie = mapper.Map<Movie>(resource);

            mockGenreService.Setup(s => s.GenreExists(resource.GenreId)).Returns(true);
            mockMovieService.Setup(s => s.AddMovieAsync(It.IsAny<Movie>())).Returns(Task.CompletedTask);
            controller = new MoviesController(mapper, mockMovieService.Object, mockGenreService.Object);

            var result = await controller.PostMovie(resource);

            var actionResult = (result.Result as CreatedAtActionResult);
            actionResult.ActionName.Should().Be(nameof(controller.GetMovie));
            actionResult.RouteValues.Keys.Should().Contain("id");
            actionResult.RouteValues.Values.Should().Contain(0);
        }

        [Fact]
        public async Task DeleteMovieShouldReturnNotFoundWhenMovieWithGivenIdNotFound ()
        {
            var movieId = 1;
            mockMovieService.Setup(s => s.GetMovieAsync(movieId)).ReturnsAsync((Movie)null);
            controller = new MoviesController(mapper, mockMovieService.Object, mockGenreService.Object);

            var result = await controller.DeleteMovie(movieId);

            result.Result.Should().BeOfType(typeof(NotFoundObjectResult));
            (result.Result as NotFoundObjectResult).Value.ToString()
               .Should().Contain(typeof(Movie).Name);
        }

        [Fact]
        public async Task DeleteMovieShouldReturnDeletedMovie ()
        {
            var movie = MovieResources.First();
            mockMovieService.Setup(s => s.RemoveMovieAsync(It.IsAny<Movie>()))
                .Returns(Task.CompletedTask);

            mockMovieService.Setup(s => s.GetMovieAsync(movie.Id))
                .ReturnsAsync(Movies.First());

            controller = new MoviesController(mapper, mockMovieService.Object, mockGenreService.Object);

            var result = await controller.DeleteMovie(movie.Id);

            result.Result.Should().BeOfType(typeof(OkObjectResult));
            (result.Result as OkObjectResult).Value.Should().BeEquivalentTo(movie);
        }
    }
}
