using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore;
using SeriesMovieTrailers.Models;

namespace SeriesMovieTrailers.Data
{
    public class ApplicationDbContext : IdentityDbContext<AppUser>
    {
        public ApplicationDbContext(DbContextOptions<ApplicationDbContext> options) : base(options) { }


        public DbSet<Movie> Movies { get; set; }
        public DbSet<Episode> Episode { get; set; }
        public DbSet<Series> Series { get; set; }
        public DbSet<Trailer> Trailers { get; set; }
        public DbSet<SearchLog> SearchLogs { get; set; }


        protected override void OnModelCreating(ModelBuilder builder)
        {
            base.OnModelCreating(builder);


            builder.Entity<Movie>(entity => {
                entity.HasKey(m => m.Id);
                entity.HasIndex(m => m.ExternalId);
                entity.Property(m => m.Title).IsRequired().HasMaxLength(500);
                entity.Property(m => m.ShortSummary).HasMaxLength(2000);
            });

            builder.Entity<Episode>(entity => {
                entity.HasKey(e => e.Id);
                entity.HasIndex(e => e.ExternalId);
                entity.Property(e => e.Title).IsRequired().HasMaxLength(500);
                entity.Property(e => e.ShortSummary).HasMaxLength(2000);
                entity.HasIndex(e => e.SeriesId);
            });

            builder.Entity<Series>(entity => {
                entity.HasKey(s => s.Id);
                entity.HasIndex(s => s.ExternalId);
                entity.Property(s => s.Title).IsRequired().HasMaxLength(500);
                entity.Property(s => s.ShortSummary).HasMaxLength(2000);
            });

            builder.Entity<Trailer>(entity => {
                entity.HasKey(t => t.Id);
                entity.HasIndex(t => t.MovieId);
                entity.HasIndex(t => t.SeriesId);
                entity.HasIndex(t => t.EpisodeId);
            });

            builder.Entity<SearchLog>(entity => {
                entity.HasKey(s => s.Id);
            });
        }
    }
}
