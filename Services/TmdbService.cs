using System.Text.Json;

namespace SeriesMovieTrailers.Services
{
    public class TmdbService : ITmdbService
    {
        private readonly HttpClient _http;
        private readonly string _apiKey;


        public TmdbService(IHttpClientFactory httpFactory, IConfiguration config)
        {
            _http = httpFactory.CreateClient("tmdb");
            _apiKey = config["TMDB:ApiKey"] ?? throw new Exception("TMDB:ApiKey missing");
        }


        public async Task<TmdbSearchResponse?> SearchMovieAsync(string query, int page)
        {
            var url = $"search/movie?api_key={_apiKey}&query={Uri.EscapeDataString(query)}&page={page}";
            var res = await _http.GetAsync(url);
            if (!res.IsSuccessStatusCode) return null;
            var s = await res.Content.ReadAsStringAsync();
            return JsonSerializer.Deserialize<TmdbSearchResponse>(s, new JsonSerializerOptions { PropertyNameCaseInsensitive = true });
        }

        public async Task<TmdbSearchResponse?> SearchSeriesAsync(string query, int page)
        {
            var url = $"search/series?api_key={_apiKey}&query={Uri.EscapeDataString(query)}&page={page}";
            var res = await _http.GetAsync(url);
            if (!res.IsSuccessStatusCode) return null;
            var s = await res.Content.ReadAsStringAsync();
            return JsonSerializer.Deserialize<TmdbSearchResponse>(s, new JsonSerializerOptions { PropertyNameCaseInsensitive = true });
        }


        public async Task<TmdbMovieDetails?> GetMovieDetailsAsync(int tmdbId)
        {
            var url = $"movie/{tmdbId}?api_key={_apiKey}&append_to_response=videos,images";
            var res = await _http.GetAsync(url);
            if (!res.IsSuccessStatusCode) return null;
            var s = await res.Content.ReadAsStringAsync();
            return JsonSerializer.Deserialize<TmdbMovieDetails>(s, new JsonSerializerOptions { PropertyNameCaseInsensitive = true });
        }
        public async Task<TmdbSeriesDetails?> GetSeriesDetailsAsync(int tmdbId)
        {
            var url = $"series/{tmdbId}?api_key={_apiKey}&append_to_response=videos,images";
            var res = await _http.GetAsync(url);
            if (!res.IsSuccessStatusCode) return null;
            var s = await res.Content.ReadAsStringAsync();
            return JsonSerializer.Deserialize<TmdbSeriesDetails>(s, new JsonSerializerOptions { PropertyNameCaseInsensitive = true });
        }
    }
    public record TmdbSearchResponse(int page, [property: System.Text.Json.Serialization.JsonPropertyName("results")] List<TmdbMovieResult> Results, [property: System.Text.Json.Serialization.JsonPropertyName("total_results")] int TotalResults);
    public record TmdbMovieResult(int Id, string Title, string Overview, string? PosterPath, [property: System.Text.Json.Serialization.JsonPropertyName("release_date")] string? ReleaseDate, double Popularity);


    public class TmdbMovieDetails
    {
        public int Id { get; set; }
        public string Title { get; set; } = String.Empty;
        public string Overview { get; set; } = String.Empty;
        public string? PosterPath { get; set; }
        public string? ReleaseDate { get; set; }
        public TmdbVideos? Videos { get; set; }
    }

    public class TmdbSeriesDetails
    {
        public int Id { get; set; }
        public string Title { get; set; } = String.Empty;
        public string Overview { get; set; } = String.Empty;
        public string? PosterPath { get; set; }
        public string? ReleaseDate { get; set; }
        public TmdbVideos? Videos { get; set; }
        public int noSeassons { get; set; }
        public int noEpisodes { get; set; }
    }
    public class TmdbVideos { public List<TmdbVideo>? Results { get; set; } }
    public class TmdbVideo { public string Key { get; set; } = string.Empty; public string Site { get; set; } = string.Empty; public string Type { get; set; } = string.Empty; public string Name { get; set; } = string.Empty; }
}
