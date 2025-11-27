namespace SeriesMovieTrailers.DTOs
{
    public class SeriesDetailDto
    {
        public int Id { get; init; }
        public string Title { get; init; } = string.Empty;
        public string? Summary { get; init; }
        public string? PosterUrl { get; init; }
        public string? ReleaseDate { get; init; }
        public IEnumerable<TrailerDto> Trailers { get; init; } = Enumerable.Empty<TrailerDto>();
        public int noSeassons { get; set; }
        public int noEpisodes { get; set; }
    }
}
