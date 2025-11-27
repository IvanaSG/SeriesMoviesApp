using System.ComponentModel.DataAnnotations;

namespace SeriesMovieTrailers.Models
{
    public class Episode
    {
        public int Id { get; set; }
        public string ExternalId { get; set; } = string.Empty; // from external repository
        public int? SeriesId { get; set; }
        public virtual Series? Series { get; set; }
        public int Seasson { get; set; }

        [Required]
        public string Title { get; set; } = string.Empty;
        public int? Year { get; set; }
        public string? ShortSummary { get; set; }
        public int? RuntimeMinutes { get; set; }
    }
}
