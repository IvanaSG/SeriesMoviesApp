using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using SeriesMovieTrailers.DTOs;
using SeriesMovieTrailers.Models;
using SeriesMovieTrailers.Services;

namespace SeriesMovieTrailers.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class MoviesController : ControllerBase
    {
        private readonly IMovieService _movieService;


        public MoviesController(IMovieService movieService)
        {
            _movieService = movieService;
        }


        [HttpGet("search")]
        public async Task<IActionResult> Search([FromQuery] string q, [FromQuery] int page = 1, [FromQuery] int pageSize = 20)
        {
            if (string.IsNullOrWhiteSpace(q)) return BadRequest("Query required");
            var (items, total) = await _movieService.SearchAsync(q, page, pageSize);
            var dto = new SearchResultDto { Total = total, Items = items.ToList() };
            return Ok(dto);
        }


        [HttpGet("{tmdbId:int}")]
        public async Task<IActionResult> Get(int tmdbId)
        {
            var movie = await _movieService.GetByExternalIdAsync(tmdbId);
            if (movie == null) return NotFound();
            return Ok(movie);
        }
    }
}