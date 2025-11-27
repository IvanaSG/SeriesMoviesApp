namespace SeriesMovieTrailers.DTOs
{
    public class TrailerDto
    {
        public string Provider { get; init; } = string.Empty;
        public string ProviderVideoId { get; init; } = string.Empty;
        public string EmbedUrl { get; init; } = string.Empty;
        public string? Title { get; init; }
    }
}
