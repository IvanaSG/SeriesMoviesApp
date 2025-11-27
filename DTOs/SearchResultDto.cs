namespace SeriesMovieTrailers.DTOs
{
    public class SearchResultDto
    {
        public int Total { get; init; }
        public List<MovieListItemDto> Items { get; init; } = new(); 
        public List<SeriesListItemDto> SeriesItems { get; init; } = new();
    }
}
