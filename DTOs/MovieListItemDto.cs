namespace SeriesMovieTrailers.DTOs
{
    public class MovieListItemDto
    {
        public int Id { get; init; }
        public string Title { get; init; } = string.Empty;
        public int? Year { get; init; }
        public string? PosterUrl { get; init; }
        public string? ShortSummary { get; init; }
    }
}
