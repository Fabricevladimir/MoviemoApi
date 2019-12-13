using System.ComponentModel.DataAnnotations;

namespace Moviemo.API.Models
{
    public class Movie
    {
        [Key]
        public int Id { get; set; }
        public int GenreId { get; set; }
        public string Title { get; set; }
        public virtual Genre Genre { get; set; }
    }
}
