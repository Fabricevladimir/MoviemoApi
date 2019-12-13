using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;

namespace Moviemo.API.Models
{
    public class Genre
    {
        [Key]
        public int Id { get; set; }
        public string Name { get; set; }
        public virtual ICollection<Movie> Movies { get; }
    }
}
