using System.ComponentModel.DataAnnotations;

namespace Moviemo.API.Models
{
    public class SaveGenreResource
    {
        [Required, StringLength(maximumLength: 48, MinimumLength = 1)]
        public string Name { get; set; }
    }
}
