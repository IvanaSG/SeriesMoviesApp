namespace SeriesMovieTrailers.DTOs
{
    public class MovieDetailDto
    {
        public int Id { get; init; }
        public string Title { get; init; } = string.Empty;
        public string? Summary { get; init; }
        public string? PosterUrl { get; init; }
        public string? ReleaseDate { get; init; }
        public IEnumerable<TrailerDto> Trailers { get; init; } = Enumerable.Empty<TrailerDto>();
    }
}
