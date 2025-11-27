using Microsoft.AspNetCore.Identity;

namespace SeriesMovieTrailers.Models
{
    public class AppUser : IdentityUser
    {
        public string? Locale { get; set; }
        public DateTime CreatedAt { get; set; } = DateTime.UtcNow;
    }
}
