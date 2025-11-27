namespace SeriesMovieTrailers.Models
{
    public class SearchLog
    {
        public int Id { get; set; }
        public string Query { get; set; } = string.Empty;
        public string? FiltersJson { get; set; }
        public int ResultCount { get; set; }
        public int DurationMs { get; set; }
        public DateTime CreatedAt { get; set; } = DateTime.UtcNow;
    }
}
