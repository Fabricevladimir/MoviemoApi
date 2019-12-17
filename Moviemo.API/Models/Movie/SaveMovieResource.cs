using System.ComponentModel.DataAnnotations;

namespace Moviemo.API.Models
{
    public class SaveMovieResource
    {
        [Required]
        public int GenreId { get; set; }

        [Required, StringLength(maximumLength: 48, MinimumLength = 1)]
        public string Title { get; set; }
    }
}
