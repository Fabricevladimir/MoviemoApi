using Moviemo.API.Models;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace Moviemo.API.Services
{
    public interface IMovieService
    {
        bool MovieExists (int id);
        bool MovieExists (string title);
        Task UpdateAsync ();
        Task<Movie> GetMovieAsync (int id);
        Task AddMovieAsync (Movie movie);
        Task RemoveMovieAsync (Movie movie);
        Task<IEnumerable<Movie>> GetAllMoviesAsync ();
    }
}
