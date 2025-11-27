using System.ComponentModel.DataAnnotations;

namespace SeriesMovieTrailers.Models
{
    public class Movie
    {

        public int Id { get; set; }
        public string ExternalId { get; set; } = string.Empty; // from external repository

        [Required]
        public string Title { get; set; } = string.Empty;
        public int? Year { get; set; }
        public string? ShortSummary { get; set; }
        public int? RuntimeMinutes { get; set; }
        public string? Language { get; set; }
        public DateTime? ReleaseDate { get; set; }
        public DateTime CreatedAt { get; set; } = DateTime.UtcNow;
    }
}