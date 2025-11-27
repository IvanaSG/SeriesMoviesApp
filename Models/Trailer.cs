namespace SeriesMovieTrailers.Models
{
    public class Trailer
    {
        public int Id { get; set; }
        public int? MovieId { get; set; }
        public virtual Movie? Movie { get; set; }
        public int? EpisodeId { get; set; }
        public virtual Episode? Episode { get; set; }
        public int? SeriesId { get; set; }
        public virtual Series? Series { get; set; }
        public string MovieExternalId { get; set; } = string.Empty; // TMDb id
        public string Provider { get; set; } = "YouTube"; // YouTube/Vimeo
        public string ProviderVideoId { get; set; } = string.Empty;
        public string? Title { get; set; }
        public string? ThumbnailUrl { get; set; }
        public int? DurationSeconds { get; set; }
        public bool IsOfficial { get; set; }
    }
}
