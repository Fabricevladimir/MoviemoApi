using FluentAssertions;
using Microsoft.AspNetCore.Mvc;
using Moq;
using Moviemo.API.Controllers;
using Moviemo.API.Models;
using Moviemo.API.Services;
using Moviemo.IntegrationTests.Setup;
using System.Collections.Generic;
using System.Dynamic;
using System.Linq;
using System.Threading.Tasks;
using Xunit;

namespace Moviemo.UnitTests.Controllers
{
    public class GenresControllerTest : BaseControllerUnitTest
    {
        private GenresController controller;
        private readonly List<Genre> Genres;
        private readonly Mock<IGenreService> mockGenreService;
        private readonly List<GenreResource> GenreResources;
        class InvalidGenre : DynamicObject { }


        public GenresControllerTest ()
        {
            Genres = TestData.Genres;
            GenreResources = mapper.Map<List<GenreResource>>(Genres);
            mockGenreService = new Mock<IGenreService>(MockBehavior.Strict);
        }

        [Fact]
        public async Task GetGenresShouldReturnAllGenres ()
        {
            mockGenreService.Setup(s => s.GetAllGenresAsync()).ReturnsAsync(Genres);
            controller = new GenresController(mapper, mockGenreService.Object);

            var result = await controller.GetGenres();

            result.Result.Should().BeOfType(typeof(OkObjectResult));
            ((result.Result as OkObjectResult).Value as List<GenreResource>)
                .Should().HaveSameCount(Genres)
                .And.BeEquivalentTo(GenreResources);
        }

        [Fact]
        public async Task GetGenreShouldReturnNotFoundWhenGenreWithGivenIdNotInDb ()
        {
            var inValidGenreId = 100;
            mockGenreService.Setup(s => s.GetGenreAsync(inValidGenreId))
               .ReturnsAsync((Genre)null);

            controller = new GenresController(mapper, mockGenreService.Object);

            var result = await controller.GetGenre(inValidGenreId);

            result.Result.Should().BeOfType(typeof(NotFoundObjectResult));
            (result.Result as NotFoundObjectResult).Value.ToString()
                .Should().Contain(typeof(Genre).Name);
        }

        [Fact]
        public async Task GetGenreShouldReturnGenreWithGivenId ()
        {
            var genre = Genres.First();
            var expected = GenreResources.First();
            mockGenreService.Setup(s => s.GetGenreAsync(genre.Id))
                .ReturnsAsync(genre);

            controller = new GenresController(mapper, mockGenreService.Object);

            var result = await controller.GetGenre(genre.Id);

            result.Result.Should().BeOfType(typeof(OkObjectResult));
            (result.Result as OkObjectResult).Value
                .Should().BeEquivalentTo(expected);
        }


        [Fact]
        public async Task PutGenreShouldReturnBadRequestWhenEnpointIdAndResourceIdDoNotMatch ()
        {
            var genre = new GenreResource() { Id = 1 };
            controller = new GenresController(mapper, mockGenreService.Object);

            var result = await controller.PutGenre(genre.Id + 1, genre);

            result.Should().BeOfType(typeof(BadRequestObjectResult));
        }

        [Fact]
        public async Task PutGenreShouldReturnNotFoundWhenGenreWithGivenIdNotFound ()
        {
            var genre = new GenreResource() { Id = 1 };
            mockGenreService.Setup(s => s.GetGenreAsync(genre.Id))
                .ReturnsAsync((Genre)null);

            controller = new GenresController(mapper, mockGenreService.Object);

            var result = await controller.PutGenre(genre.Id, genre);

            result.Should().BeOfType(typeof(NotFoundObjectResult));
            (result as NotFoundObjectResult).Value.ToString()
                 .Should().Contain(typeof(Genre).Name);
        }

        [Fact]
        public async Task PutGenreShouldReturnNoContentWhenSuccessful ()
        {
            var genre = Genres.First();
            var updatedGenre = GenreResources.First();
            mockGenreService.Setup(s => s.GetGenreAsync(genre.Id)).ReturnsAsync(genre);
            mockGenreService.Setup(s => s.UpdateAsync()).Returns(Task.CompletedTask);
            controller = new GenresController(mapper, mockGenreService.Object);

            var result = await controller.PutGenre(genre.Id, updatedGenre);

            result.Should().BeOfType(typeof(NoContentResult));
        }

        [Fact]
        public async Task PostGenreShouldReturnConflictWhenGenreAlreadyExists ()
        {
            var genre = new SaveGenreResource() { Name = "ABC" };
            mockGenreService.Setup(s => s.GenreExists(genre.Name)).Returns(true);

            controller = new GenresController(mapper, mockGenreService.Object);

            var result = await controller.PostGenre(genre);

            result.Result.Should().BeOfType(typeof(ConflictObjectResult));
            (result.Result as ConflictObjectResult).Value.ToString()
                .Should().Contain(typeof(Genre).Name);
        }

        [Fact]
        public async Task PostGenreShouldReturnCreatedGenre ()
        {
            var resource = new SaveGenreResource() { Name = "ABC" };
            var genre = mapper.Map<Genre>(resource);
            var expected = mapper.Map<GenreResource>(genre);

            mockGenreService.Setup(s => s.GenreExists(resource.Name)).Returns(false);
            mockGenreService.Setup(s => s.AddGenreAsync(It.IsAny<Genre>()))
                .Returns(Task.CompletedTask);

            controller = new GenresController(mapper, mockGenreService.Object);

            var result = await controller.PostGenre(resource);

            result.Result.Should().BeOfType(typeof(CreatedAtActionResult));
            (result.Result as CreatedAtActionResult).Value
                .Should().BeEquivalentTo(expected);
        }

        [Fact]
        public async Task PostGenreShouldReturnRouteToCreatedGenre ()
        {
            var resource = new SaveGenreResource() { Name = "ABC" };
            var genre = mapper.Map<Genre>(resource);

            mockGenreService.Setup(s => s.GenreExists(resource.Name)).Returns(false);
            mockGenreService.Setup(s => s.AddGenreAsync(It.IsAny<Genre>()))
                .Returns(Task.CompletedTask);

            controller = new GenresController(mapper, mockGenreService.Object);

            var result = await controller.PostGenre(resource);

            var actionResult = (result.Result as CreatedAtActionResult);
            actionResult.ActionName.Should().Be(nameof(controller.GetGenre));
            actionResult.RouteValues.Keys.Should().Contain("id");
            actionResult.RouteValues.Values.Should().Contain(0);
        }

        [Fact]
        public async Task DeleteMovieShouldReturnNotFoundWhenGenreWithGivenIdNotFound ()
        {
            var genreId = 1;
            mockGenreService.Setup(s => s.GetGenreAsync(genreId)).ReturnsAsync((Genre)null);
            controller = new GenresController(mapper, mockGenreService.Object);

            var result = await controller.DeleteGenre(genreId);

            result.Result.Should().BeOfType(typeof(NotFoundObjectResult));
            (result.Result as NotFoundObjectResult).Value.ToString()
               .Should().Contain(typeof(Genre).Name);
        }

        [Fact]
        public async Task DeleteGenreShouldReturnDeletedGenre ()
        {
            var genre = GenreResources.First();
            mockGenreService.Setup(s => s.RemoveGenreAsync(It.IsAny<Genre>()))
                .Returns(Task.CompletedTask);

            mockGenreService.Setup(s => s.GetGenreAsync(genre.Id))
                .ReturnsAsync(Genres.First());

            controller = new GenresController(mapper, mockGenreService.Object);

            var result = await controller.DeleteGenre(genre.Id);

            result.Result.Should().BeOfType(typeof(OkObjectResult));
            (result.Result as OkObjectResult).Value.Should().BeEquivalentTo(genre);
        }
    }
}
