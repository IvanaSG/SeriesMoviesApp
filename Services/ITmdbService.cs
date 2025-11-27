namespace SeriesMovieTrailers.Services
{
    public interface ITmdbService
    {
        Task<TmdbSearchResponse?> SearchMovieAsync(string query, int page);
        Task<TmdbMovieDetails?> GetMovieDetailsAsync(int tmdbId);
        Task<TmdbSearchResponse?> SearchSeriesAsync(string query, int page);
        Task<TmdbSeriesDetails?> GetSeriesDetailsAsync(int tmdbId);

    }
}
