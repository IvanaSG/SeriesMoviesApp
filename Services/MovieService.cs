using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Caching.Distributed;
using SeriesMovieTrailers.Data;
using SeriesMovieTrailers.DTOs;
using System.Text.Json;

namespace SeriesMovieTrailers.Services
{
    public class MovieService : IMovieService
    {
        private readonly ITmdbService _tmdb;
        private readonly ApplicationDbContext _db;
        private readonly IDistributedCache _cache;
        private const string SearchCachePrefix = "search:";
        private const string MovieCachePrefix = "movie:";


        public MovieService(ITmdbService tmdb, ApplicationDbContext db, IDistributedCache cache)
        {
            _tmdb = tmdb; _db = db; _cache = cache;
        }


        public async Task<(IEnumerable<MovieListItemDto> Items, int Total)> SearchAsync(string q, int page, int pageSize)
        {
            page = Math.Clamp(page, 1, 50);
            pageSize = Math.Clamp(pageSize, 1, 50);
            var cacheKey = $"{SearchCachePrefix}{q.ToLowerInvariant()}:{page}:{pageSize}";
            var cached = await _cache.GetStringAsync(cacheKey);
            if (!string.IsNullOrEmpty(cached))
            {
                var dto = JsonSerializer.Deserialize<SearchResultDto>(cached)!;
                return (dto.Items, dto.Total);
            }


            var res = await _tmdb.SearchMovieAsync(q, page);
            if (res == null) return (Enumerable.Empty<MovieListItemDto>(), 0);


            var items = res.Results.Select(r => new MovieListItemDto
            {
                Id = r.Id,
                Title = r.Title,
                Year = !string.IsNullOrEmpty(r.ReleaseDate) && r.ReleaseDate.Length >= 4 ? int.Parse(r.ReleaseDate.Substring(0, 4)) : null,
                PosterUrl = r.PosterPath != null ? $"https://image.tmdb.org/t/p/w500{r.PosterPath}" : null,
                ShortSummary = r.Overview
            }).ToList();


            var dtoOut = new SearchResultDto { Total = res.TotalResults, Items = items };
            await _cache.SetStringAsync(cacheKey, JsonSerializer.Serialize(dtoOut), new DistributedCacheEntryOptions { AbsoluteExpirationRelativeToNow = TimeSpan.FromSeconds(30) });
            return (items, res.TotalResults);
        }


        public async Task<MovieDetailDto?> GetByExternalIdAsync(int tmdbId)
        {
            var cacheKey = MovieCachePrefix + tmdbId;
            var cached = await _cache.GetStringAsync(cacheKey);
            if (!string.IsNullOrEmpty(cached)) return JsonSerializer.Deserialize<MovieDetailDto>(cached);


            var details = await _tmdb.GetMovieDetailsAsync(tmdbId);
            if (details == null) return null;


            var trailers = new List<TrailerDto>();
            if (details.Videos?.Results != null)
            {
                foreach (var v in details.Videos.Results.Where(x => x.Type.Equals("Trailer", StringComparison.OrdinalIgnoreCase)))
                {
                    if (v.Site.Equals("YouTube", StringComparison.OrdinalIgnoreCase))
                    {
                        trailers.Add(new TrailerDto { Provider = "YouTube", ProviderVideoId = v.Key, EmbedUrl = $"https://www.youtube.com/embed/{v.Key}", Title = v.Name });
                    }
                    else if (v.Site.Equals("Vimeo", StringComparison.OrdinalIgnoreCase))
                    {
                        trailers.Add(new TrailerDto { Provider = "Vimeo", ProviderVideoId = v.Key, EmbedUrl = $"https://player.vimeo.com/video/{v.Key}", Title = v.Name });
                    }
                }
            }


            // Merge local DB trailers (stored by MovieExternalId)
            var local = await _db.Trailers.Where(t => t.MovieExternalId == tmdbId.ToString()).ToListAsync();
            foreach (var lt in local)
            {
                trailers.Add(new TrailerDto { Provider = lt.Provider, ProviderVideoId = lt.ProviderVideoId, EmbedUrl = lt.Provider.Equals("YouTube", StringComparison.OrdinalIgnoreCase) ? $"https://www.youtube.com/embed/{lt.ProviderVideoId}" : $"https://player.vimeo.com/video/{lt.ProviderVideoId}", Title = lt.Title });
            }


            var dto = new MovieDetailDto
            {
                Id = details.Id,
                Title = details.Title,
                Summary = details.Overview,
                PosterUrl = details.PosterPath != null ? $"https://image.tmdb.org/t/p/w500{details.PosterPath}" : null,
                ReleaseDate = details.ReleaseDate,
                Trailers = trailers
            };


            await _cache.SetStringAsync(cacheKey, JsonSerializer.Serialize(dto), new DistributedCacheEntryOptions { AbsoluteExpirationRelativeToNow = TimeSpan.FromMinutes(10) });
            return dto;
        }
    }
}