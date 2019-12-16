using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;

namespace Moviemo.API.Models
{
    public class MovieResource
    {
        [Key]
        public int Id { get; set; }
        public int GenreId { get; set; }
        public string Title { get; set; }
        public string Genre { get; set; }
    }
}
