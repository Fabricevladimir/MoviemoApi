using System.ComponentModel.DataAnnotations;

namespace Moviemo.API.Models
{
    public class GenreResource
    {
        [Required]
        public int Id { get; set; }

        [Required, StringLength(maximumLength: 48, MinimumLength = 1)]
        public string Name { get; set; }
    }
}
