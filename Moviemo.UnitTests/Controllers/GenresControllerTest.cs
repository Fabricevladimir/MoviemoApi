using FluentAssertions;
using Microsoft.AspNetCore.Mvc;
using Moq;
using Moviemo.API.Controllers;
using Moviemo.API.Models;
using System.Collections;
using System.Collections.Generic;
using System.Threading.Tasks;
using Xunit;

namespace Moviemo.UnitTests.Controllers
{
    public sealed class GenresControllerTest : BaseControllerUnitTest
    {
        private GenresController genresController;

        [Fact]
        public async Task GetOne_ShouldReturnNotFoundWhenGenreWithGivenIdNotInDb ()
        {
            mockGenreService.Setup(s => s.GetGenreAsync(Constants.INVALID_GENRE_ID))
               .ReturnsAsync((Genre)null);

            genresController = new GenresController(mapper, mockGenreService.Object);

            var result = await genresController.GetOne(Constants.INVALID_GENRE_ID);

            result.Result.Should().BeOfType(typeof(NotFoundObjectResult));
            (result.Result as NotFoundObjectResult).Value.Should().NotBeNull();
        }

        [Fact]
        public async Task GetOne_ShouldReturnGenreWithGivenId ()
        {
            mockGenreService.Setup(s => s.GetGenreAsync(Constants.VALID_GENRE_ID))
                .ReturnsAsync(Constants.VALID_GENRE);

            genresController = new GenresController(mapper, mockGenreService.Object);

            var result = await genresController.GetOne(Constants.VALID_GENRE_ID);

            result.Result.Should().BeOfType(typeof(OkObjectResult));
            (result.Result as OkObjectResult).Value
                .Should().BeEquivalentTo(Constants.VALID_GENRERESOURCE);
        }

        [Fact]
        public async Task Create_ShouldReturnConflictWhenGenreAlreadyExists ()
        {
            var genreToAdd = new SaveGenreResource() { Name = "ABC" };
            mockGenreService.Setup(s => s.GenreExists(genreToAdd.Name))
                .Returns(true);

            genresController = new GenresController(mapper, mockGenreService.Object);

            var result = await genresController.Create(genreToAdd);

            result.Result.Should().BeOfType(typeof(ConflictObjectResult));
            (result.Result as ConflictObjectResult).Value.Should().NotBeNull();
        }

        [Fact]
        public async Task Create_ShouldReturnCreatedGenre ()
        {
            var genreToAdd = new SaveGenreResource() { Name = "ABC" };
            var returnedGenre = new GenreResource() { Id = 0, Name = genreToAdd.Name };

            mockGenreService.Setup(s => s.GenreExists(genreToAdd.Name))
                .Returns(false);

            genresController = new GenresController(mapper, mockGenreService.Object);

            var result = await genresController.Create(genreToAdd);

            result.Result.Should().BeOfType(typeof(CreatedAtActionResult));
            (result.Result as CreatedAtActionResult).Value
                .Should().BeEquivalentTo(returnedGenre);
        }

        [Fact]
        public async Task GetAll_ShouldReturnAllGenres ()
        {
            var genres = new Genre[] { new Genre(), new Genre(), new Genre() };
            mockGenreService.Setup(s => s.GetAllGenresAsync())
                .ReturnsAsync(genres);

            genresController = new GenresController(mapper, mockGenreService.Object);

            var result = await genresController.GetAll();

            result.Result.Should().BeOfType(typeof(OkObjectResult));
            ((result.Result as OkObjectResult).Value as List<GenreResource>)
                .Should().HaveSameCount(genres);
        }

        [Fact]
        public async Task Update_ShouldReturnNotFoundWhenGenreWithGivenIdDoesNotExist ()
        {
            var genreToUpdate = new GenreResource() { Id = 1 };
            mockGenreService.Setup(s => s.GetGenreAsync(genreToUpdate.Id))
                .ReturnsAsync((Genre)null);

            genresController = new GenresController(mapper, mockGenreService.Object);

            var result = await genresController.Update(genreToUpdate.Id, genreToUpdate);

            result.Should().BeOfType(typeof(NotFoundObjectResult));
        }

        [Fact]
        public async Task Update_ShouldReturnBadRequestWhenEnpointIdAndResourceIdDoNotMatch ()
        {
            var genreToUpdate = new GenreResource() { Id = 1 };
            genresController = new GenresController(mapper, mockGenreService.Object);

            var result = await genresController.Update(0, genreToUpdate);

            result.Should().BeOfType(typeof(BadRequestObjectResult));
        }

        [Fact]
        public async Task Update_ShouldReturnNoContent ()
        {
            var genreToUpdate = new GenreResource() { Id = 1 };
            mockGenreService.Setup(s => s.GetGenreAsync(genreToUpdate.Id))
                .ReturnsAsync(new Genre());

            genresController = new GenresController(mapper, mockGenreService.Object);

            var result = await genresController.Update(genreToUpdate.Id, genreToUpdate);

            result.Should().BeOfType(typeof(NoContentResult));
        }

        [Fact]
        public async Task Delete_ShouldReturnNotFoundWhenGenreWithGivenIdDoesNotExist ()
        {
            var genreId = 1;
            mockGenreService.Setup(s => s.GetGenreAsync(genreId))
                .ReturnsAsync((Genre)null);

            genresController = new GenresController(mapper, mockGenreService.Object);

            var result = await genresController.Delete(genreId);

            result.Should().BeOfType(typeof(NotFoundObjectResult));
        }

        [Fact]
        public async Task Delete_ShouldReturnNoContent ()
        {
            var genreId = 1;
            mockGenreService.Setup(s => s.GetGenreAsync(genreId))
                .ReturnsAsync(new Genre());

            genresController = new GenresController(mapper, mockGenreService.Object);

            var result = await genresController.Delete(genreId);

            result.Should().BeOfType(typeof(NoContentResult));
        }
    }
}
