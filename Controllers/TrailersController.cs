using Microsoft.AspNetCore.Mvc;
using SeriesMovieTrailers.Data;
using SeriesMovieTrailers.DTOs;
using SeriesMovieTrailers.Models;
using System.Data.Entity;

namespace SeriesMovieTrailers.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class TrailersController : ControllerBase
    {
        private readonly ApplicationDbContext _db;
        public TrailersController(ApplicationDbContext db) { _db = db; }


        [HttpGet("movie/{tmdbId:int}")]
        public async Task<IActionResult> GetForMovie(int tmdbId)
        {
            var trailers = await _db.Trailers.Where(t => t.MovieExternalId == tmdbId.ToString()).ToListAsync();
            var dto = trailers.Select(t => new TrailerDto { Provider = t.Provider, ProviderVideoId = t.ProviderVideoId, EmbedUrl = t.Provider.Equals("YouTube", StringComparison.OrdinalIgnoreCase) ? $"https://www.youtube.com/embed/{t.ProviderVideoId}" : $"https://player.vimeo.com/video/{t.ProviderVideoId}", Title = t.Title });
            return Ok(dto);
        }
    }
}
