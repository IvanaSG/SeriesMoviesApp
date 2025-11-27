using SeriesMovieTrailers.DTOs;
using SeriesMovieTrailers.Models;

namespace SeriesMovieTrailers.Services
{
    public interface IMovieService
    {
        Task<(IEnumerable<MovieListItemDto> Items, int Total)> SearchAsync(string q, int page, int pageSize);
        Task<MovieDetailDto?> GetByExternalIdAsync(int tmdbId);
    }
}
