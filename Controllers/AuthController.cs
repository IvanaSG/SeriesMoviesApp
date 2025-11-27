using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using SeriesMovieTrailers.Models;
using SeriesMovieTrailers.Services;

namespace SeriesMovieTrailers.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class AuthController : ControllerBase
    {
        private readonly UserManager<AppUser> _userManager;
        private readonly SignInManager<AppUser> _signInManager;
        private readonly IJwtTokenService _jwt;


        public AuthController(UserManager<AppUser> userManager, SignInManager<AppUser> signInManager, IJwtTokenService jwt)
        {
            _userManager = userManager; _signInManager = signInManager; _jwt = jwt;
        }


        [HttpPost("register")]
        public async Task<IActionResult> Register([FromBody] RegisterDto dto)
        {
            var user = new AppUser { UserName = dto.UserName, Email = dto.Email };
            var result = await _userManager.CreateAsync(user, dto.Password);
            if (!result.Succeeded) return BadRequest(result.Errors);
            await _userManager.AddToRoleAsync(user, "User");
            return Ok(new { user.Id });
        }


        [HttpPost("login")]
        public async Task<IActionResult> Login([FromBody] LoginDto dto)
        {
            var user = await _userManager.FindByNameAsync(dto.UserName);
            if (user == null) return Unauthorized();
            var result = await _signInManager.CheckPasswordSignInAsync(user, dto.Password, lockoutOnFailure: true);
            if (!result.Succeeded) return Unauthorized();
            var roles = await _userManager.GetRolesAsync(user);
            var token = _jwt.CreateToken(user, roles);
            return Ok(new { access_token = token });
        }
    }


    public record RegisterDto(string UserName, string Email, string Password);
    public record LoginDto(string UserName, string Password);
}
