using SeriesMovieTrailers.DTOs;

namespace SeriesMovieTrailers.Services
{
    public interface ISeriesService
    {
        Task<(IEnumerable<SeriesListItemDto> Items, int Total)> SearchAsync(string q, int page, int pageSize);
        Task<SeriesDetailDto?> GetByExternalIdAsync(int tmdbId);
    }
}
