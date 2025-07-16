using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using WeatherForecastAPI.DTOs;
using WeatherForecastAPI.Services.IServices;

namespace WeatherForecastAPI.Controllers
{
    [Route("api/auth")]
    [ApiController]
    public class AuthController : ControllerBase
    {
        private readonly IUserService _userService;
        public AuthController(IUserService userService) => _userService = userService;

        [HttpPost("register")]
        public IActionResult Register([FromBody] LoginDto dto)
        {
            return _userService.Register(dto.Username, dto.Password)
                ? Ok("User registered.")
                : BadRequest("User already exists.");
        }

        [HttpPost("login")]
        public IActionResult Login([FromBody] LoginDto dto)
        {
            var token = _userService.Login(dto.Username, dto.Password);
            return token == null ? Unauthorized() : Ok(new { Token = token });
        }
    }
}
