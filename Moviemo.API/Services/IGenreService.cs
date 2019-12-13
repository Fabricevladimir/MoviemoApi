using Moviemo.API.Models;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace Moviemo.API.Services
{
    public interface IGenreService
    {
        bool GenreExists (int id);
        bool GenreExists (string name);
        Task UpdateAsync ();
        Task<Genre> GetGenreAsync (int id);
        Task AddGenreAsync (Genre genre);
        Task RemoveGenreAsync (Genre genre);
        Task<IEnumerable<Genre>> GetAllGenresAsync ();
    }
}
