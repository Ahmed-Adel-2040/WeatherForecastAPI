using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using WeatherForecastAPI.Services.IServices;

namespace WeatherForecastAPI.Controllers
{
    [Route("api/weather")]
    [ApiController]
    [Authorize]
    public class WeatherController : ControllerBase
    {
        private readonly IWeatherService _weatherService;
        public WeatherController(IWeatherService weatherService) => _weatherService = weatherService;

        [HttpGet]
        public async Task<IActionResult> Get([FromQuery] string city)
        {
            if (string.IsNullOrEmpty(city)) return BadRequest("City is required.");
            var result = await _weatherService.GetWeatherAsync(city);
            return Ok(result);
        }
    }
}
