using DummyNetflixApi.entities;
using DummyNetflixApi.Entities;
using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace DummyNetflixApi.Database
{
    public class DummyNetflixDbContext : IdentityDbContext
    {
        public DummyNetflixDbContext(DbContextOptions<DummyNetflixDbContext> options) : base(options)
        {
        }

        protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder) => optionsBuilder.LogTo(Console.WriteLine);

        public DbSet<Movie> Movies { get; set; }
        public DbSet<Genre> Genries { get; set; }
        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            base.OnModelCreating(modelBuilder);
            var movie = modelBuilder.Entity<Movie>();
            movie.HasKey(x => x.Id);
            movie.Property(x => x.Id).IsRequired().HasMaxLength(36);
            movie.Property(x => x.Description).HasMaxLength(256);
            movie.Property(x => x.Title).IsRequired().HasMaxLength(64);
            movie.Property(x => x.ImageBase64).HasMaxLength(3698688);// 3MB
            movie.Property(x => x.ImageHeaderBase64).HasMaxLength(32);
            movie.HasOne(p => p.Genre).WithMany(b => b.Movies);

            var genre = modelBuilder.Entity<Genre>();
            genre.HasKey(x => x.Id);
            genre.Property(x => x.Name).IsRequired().HasMaxLength(128);
        }
    }
}
