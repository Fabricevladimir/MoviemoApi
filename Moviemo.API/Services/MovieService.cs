using Microsoft.EntityFrameworkCore;
using Moviemo.API.Data;
using Moviemo.API.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Moviemo.API.Services
{
    public class MovieService : IMovieService
    {
        private readonly MoviemoContext repository;

        public MovieService (MoviemoContext repository)
        {
            this.repository = repository ?? throw new ArgumentNullException(nameof(repository));
        }

        public async Task AddMovieAsync (Movie movie)
        {
            repository.Add(movie);
            await SaveChangesAsync();
        }

        public async Task<IEnumerable<Movie>> GetAllMoviesAsync ()
        {
            return await repository.Movies
                .Include(movie => movie.Genre)
                .ToListAsync();
        }

        public async Task<Movie> GetMovieAsync (int id)
        {
            return await repository.Movies.FindAsync(id);
        }

        public bool MovieExists (int id)
        {
            return repository.Movies.Any(movie => movie.Id == id);
        }

        public bool MovieExists (string title)
        {
            return repository.Movies.Any(movie => movie.Title == title);
        }

        public async Task RemoveMovieAsync (Movie movie)
        {
            repository.Remove(movie);
            await SaveChangesAsync();
        }

        public async Task UpdateAsync ()
        {
            await SaveChangesAsync();
        }

        private async Task SaveChangesAsync ()
        {
            await repository.SaveChangesAsync();
        }
    }
}
