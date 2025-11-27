using System.ComponentModel.DataAnnotations;

namespace SeriesMovieTrailers.Models
{
    public class Series
    {
        public int Id { get; set; }
        public string ExternalId { get; set; } = string.Empty; // from external repository

        [Required]
        public string Title { get; set; } = string.Empty;
        public int? Year { get; set; }
        public string? ShortSummary { get; set; }
        public string? Language { get; set; }
        public DateTime? ReleaseDate { get; set; }
        public int noEpisodes { get;set; }
        public int noSeassons { get; set; }
        public ICollection<Episode>? Episodes { get; set; }
    }
}
