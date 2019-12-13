using Microsoft.EntityFrameworkCore;
using Moviemo.API.Models;
using System;

namespace Moviemo.API.Data
{
    public class MoviemoContext : DbContext
    {
        public MoviemoContext () { }

        public MoviemoContext (DbContextOptions<MoviemoContext> options) : base(options) { }

        protected override void OnModelCreating (ModelBuilder builder)
        {
            if (builder is null)
                throw new ArgumentNullException(nameof(builder));

            builder.Entity<Genre>().Property(p => p.Name).HasMaxLength(40);
            builder.Entity<Movie>().Property(p => p.Title).HasMaxLength(40);

            builder.Entity<Genre>().ToTable("Genre");
            builder.Entity<Movie>().ToTable("Movie");
        }

        public DbSet<Genre> Genres { get; set; }
        public DbSet<Movie> Movies { get; set; }
    }
}
