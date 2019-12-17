using AutoMapper;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Moviemo.API.Extensions;
using Moviemo.API.Models;
using Moviemo.API.Services;
using System;
using System.Collections.Generic;
using System.Diagnostics.CodeAnalysis;
using System.Threading.Tasks;

namespace Moviemo.API.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    [SuppressMessage("Design", "CA1062:Validate arguments of public methods",
     Justification = "Route auto validates property as part of the modelstate")]
    public class GenresController : ControllerBase
    {
        private readonly IMapper mapper;
        private readonly IGenreService genreService;

        public GenresController (IMapper mapper, IGenreService genreService)
        {
            this.mapper = mapper ?? throw new ArgumentNullException(nameof(mapper));
            this.genreService = genreService ?? throw new ArgumentNullException(nameof(genreService));
        }

        /// <summary>
        /// Get all the genres
        /// </summary>
        /// <returns>Ennumerable collection of genres</returns>
        [HttpGet]
        [ProducesResponseType(typeof(IEnumerable<GenreResource>), StatusCodes.Status200OK)]
        public async Task<ActionResult<IEnumerable<GenreResource>>> GetGenres ()
        {
            var genres = await genreService.GetAllGenresAsync();

            return Ok(mapper.Map<IEnumerable<GenreResource>>(genres));
        }

        /// <summary>
        /// Get a genre by its id
        /// </summary>
        /// <param name="id"></param>
        /// <returns>A genre with the given Id</returns>
        [HttpGet("{id}")]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        [ProducesResponseType(typeof(GenreResource), StatusCodes.Status200OK)]
        public async Task<ActionResult<GenreResource>> GetGenre (int id)
        {
            var genre = await genreService.GetGenreAsync(id);

            if (genre == null)
            {
                var errorMessage = Utils.NotFoundMessage(typeof(Genre), nameof(genre.Id), id);
                return NotFound(errorMessage);
            }

            return Ok(mapper.Map<GenreResource>(genre));
        }

        /// <summary>
        /// Update a genre
        /// </summary>
        /// <param name="id"></param>
        /// <param name="resource"></param>
        [HttpPut("{id}")]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        [ProducesResponseType(StatusCodes.Status204NoContent)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        public async Task<ActionResult> PutGenre (int id, [FromBody]GenreResource resource)
        {
            if (id != resource.Id)
            {
                return BadRequest(Utils.ResourceEndpointIdError);
            }

            var genre = await genreService.GetGenreAsync(id);
            if (genre == null)
            {
                var message = Utils.NotFoundMessage(typeof(Genre), nameof(id), id);
                return NotFound(message);
            }

            mapper.Map(resource, genre);
            await genreService.UpdateAsync();

            return NoContent();
        }

        /// <summary>
        /// Create a genre
        /// </summary>
        /// <param name="resource"></param>
        /// <returns>Newly created genre</returns>
        [HttpPost]
        [ProducesResponseType(StatusCodes.Status409Conflict)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        [ProducesResponseType(typeof(GenreResource), StatusCodes.Status201Created)]
        public async Task<ActionResult<GenreResource>> PostGenre ([FromBody]SaveGenreResource resource)
        {
            if (genreService.GenreExists(resource.Name))
            {
                var message = Utils.ConflictMessage(
                    typeof(Genre), nameof(resource.Name), resource.Name);

                return Conflict(message);
            }

            var genre = mapper.Map<Genre>(resource);
            await genreService.AddGenreAsync(genre);

            var result = mapper.Map<GenreResource>(genre);

            return CreatedAtAction(nameof(GetGenre), new { id = result.Id }, result);
        }




        /// <summary>
        /// Remove a genre
        /// </summary>
        /// <param name="id"></param>
        /// <returns>Deleted genre</returns>
        [HttpDelete("{id}")]
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        public async Task<ActionResult<GenreResource>> DeleteGenre (int id)
        {
            var genre = await genreService.GetGenreAsync(id);
            if (genre == null)
            {
                var message = Utils.NotFoundMessage(typeof(Genre), nameof(id), id);
                return NotFound(message);
            }

            await genreService.RemoveGenreAsync(genre);
            return Ok(mapper.Map<GenreResource>(genre));
        }
    }
}