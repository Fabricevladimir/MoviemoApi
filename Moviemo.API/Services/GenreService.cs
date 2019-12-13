using Microsoft.EntityFrameworkCore;
using Moviemo.API.Data;
using Moviemo.API.Models;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Moviemo.API.Services
{
    public class GenreService : IGenreService
    {
        private readonly MoviemoContext repository;

        public GenreService (MoviemoContext repository)
        {
            this.repository = repository ?? throw new System.ArgumentNullException(nameof(repository));
        }

        public async Task AddGenreAsync (Genre genre)
        {
            repository.Add(genre);
            await SaveChangesAsync();
        }

        public bool GenreExists (int id)
        {
            return (repository.Genres.Find(id) != null);
        }

        public bool GenreExists (string name)
        {
            return repository.Genres.Any(genre => genre.Name == name);
        }

        public async Task<IEnumerable<Genre>> GetAllGenresAsync ()
        {
            return await repository.Genres
                .Include(genre => genre.Movies)
                .ToListAsync();
        }

        public async Task<Genre> GetGenreAsync (int id)
        {
            return await repository.Genres.FindAsync(id);
        }

        public async Task RemoveGenreAsync (Genre genre)
        {
            repository.Remove(genre);
            await SaveChangesAsync();
        }

        private async Task SaveChangesAsync ()
        {
            await repository.SaveChangesAsync();
        }

        public async Task UpdateAsync ()
        {
            await SaveChangesAsync();
        }
    }
}

