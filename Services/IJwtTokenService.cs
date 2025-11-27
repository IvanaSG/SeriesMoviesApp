using SeriesMovieTrailers.Models;

namespace SeriesMovieTrailers.Services
{
    public interface IJwtTokenService
    {
        string CreateToken(AppUser user, IList<string> roles);
    }
}
