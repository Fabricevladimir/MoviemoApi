using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using AutoMapper;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Moviemo.API.Data;
using Moviemo.API.Extensions;
using Moviemo.API.Models;
using Moviemo.API.Services;

namespace Moviemo.API.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class MoviesController : ControllerBase
    {
        private readonly IMapper mapper;
        private readonly IGenreService genreService;
        private readonly IMovieService movieService;

        public MoviesController (IMapper mapper, IMovieService movieService, IGenreService genreService)
        {
            this.mapper = mapper ?? throw new ArgumentNullException(nameof(mapper));
            this.genreService = genreService ?? throw new ArgumentNullException(nameof(genreService));
            this.movieService = movieService ?? throw new ArgumentNullException(nameof(movieService));
        }

        /// <summary>
        /// Get all the movies
        /// </summary>
        /// <returns>Ennumerable collection of movies</returns>
        [HttpGet]
        [ProducesResponseType(typeof(IEnumerable<MovieResource>), StatusCodes.Status200OK)]
        public async Task<ActionResult<IEnumerable<Movie>>> GetMovies ()
        {
            var movies = await movieService.GetAllMoviesAsync();
            return Ok(mapper.Map<IEnumerable<MovieResource>>(movies));
        }

        /// <summary>
        /// Get a movie by its id
        /// </summary>
        /// <param name="id"></param>
        /// <returns>A movie with the given Id</returns>
        [HttpGet("{id}")]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        [ProducesResponseType(typeof(MovieResource), StatusCodes.Status200OK)]
        public async Task<ActionResult<Movie>> GetMovie (int id)
        {
            var movie = await movieService.GetMovieAsync(id);

            if (movie == null)
            {
                var errorMessage = Utils.NotFoundMessage(typeof(Movie),
                    nameof(movie.Id), id);

                return NotFound(errorMessage);
            }

            return Ok(mapper.Map<MovieResource>(movie));
        }

        /// <summary>
        /// Update a movie
        /// </summary>
        /// <param name="id"></param>
        /// <param name="resource"></param>
        [HttpPut("{id}")]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        [ProducesResponseType(StatusCodes.Status204NoContent)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        public async Task<IActionResult> PutMovie (int id, MovieResource resource)
        {
            if (id != resource.Id)
            {
                return BadRequest(Utils.ResourceEndpointIdError);
            }

            var movie = await movieService.GetMovieAsync(id);
            if (movie == null)
            {
                var message = Utils.NotFoundMessage(typeof(Movie),
                    nameof(movie.Id), id);

                return NotFound(message);
            }

            if (!genreService.GenreExists(resource.GenreId))
            {
                var message = Utils.NotFoundMessage(typeof(Genre),
                    nameof(resource.GenreId), resource.GenreId);

                return NotFound(message);
            }

            mapper.Map(resource, movie);
            await movieService.UpdateAsync();

            return NoContent();
        }

        /// <summary>
        /// Create a genre
        /// </summary>
        /// <param name="resource"></param>
        /// <returns>Newly created movie</returns>
        [HttpPost]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        [ProducesResponseType(typeof(MovieResource), StatusCodes.Status201Created)]
        public async Task<ActionResult<MovieResource>> PostMovie ([FromBody]SaveMovieResource resource)
        {
            if (!genreService.GenreExists(resource.GenreId))
            {
                var message = Utils.NotFoundMessage(typeof(Genre),
                    nameof(resource.GenreId), resource.GenreId);

                return NotFound(message);
            }

            var movie = mapper.Map<Movie>(resource);
            await movieService.AddMovieAsync(movie);
            var result = mapper.Map<MovieResource>(movie);

            return CreatedAtAction(nameof(GetMovie), new { id = result.Id }, result);
        }

        /// <summary>
        /// Remove a movie
        /// </summary>
        /// <param name="id"></param>
        /// <returns>Deleted movie</returns>
        [HttpDelete("{id}")]
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        public async Task<ActionResult<MovieResource>> DeleteMovie (int id)
        {
            var movie = await movieService.GetMovieAsync(id);
            if (movie == null)
            {
                var message = Utils.NotFoundMessage(typeof(Movie), nameof(movie.Id), id);
                return NotFound(message);
            }

            await movieService.RemoveMovieAsync(movie);
            return Ok(mapper.Map<MovieResource>(movie));
        }
    }
}
