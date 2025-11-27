using Microsoft.AspNetCore.Identity;
using Microsoft.IdentityModel.Tokens;
using SeriesMovieTrailers.Models;
using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Text;

namespace SeriesMovieTrailers.Services
{
    public class JwtTokenService : IJwtTokenService
    {
        private readonly IConfiguration _config;
        private readonly UserManager<AppUser> _userManager;
        public JwtTokenService(IConfiguration config, UserManager<AppUser> userManager)
        {
            _config = config; _userManager = userManager;
        }


        public string CreateToken(AppUser user, IList<string> roles)
        {
            var key = Encoding.ASCII.GetBytes(_config["Jwt:Key"]!);
            var tokenHandler = new JwtSecurityTokenHandler();
            var claims = new List<Claim> {
                new Claim(ClaimTypes.NameIdentifier, user.Id),
                new Claim(ClaimTypes.Name, user.UserName ?? string.Empty),
                new Claim(ClaimTypes.Email, user.Email ?? string.Empty)
            };
            foreach (var r in roles) claims.Add(new Claim(ClaimTypes.Role, r));
            var tokenDescriptor = new SecurityTokenDescriptor
            {
                Subject = new ClaimsIdentity(claims),
                Expires = DateTime.UtcNow.AddHours(4),
                SigningCredentials = new SigningCredentials(new SymmetricSecurityKey(key), SecurityAlgorithms.HmacSha256Signature)
            };
            var token = tokenHandler.CreateToken(tokenDescriptor);
            return tokenHandler.WriteToken(token);
        }
    }
}
