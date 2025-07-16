using Microsoft.Extensions.Caching.Memory;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using WeatherForecastAPI.Repositories.Interfaces;
using WeatherForecastAPI.Repositories.Managers;
using WeatherForecastAPI.Services.ConcreateServices;
using WeatherForecastAPI.Services.IServices;

namespace WeatherApi.Test
{
    public class WeatherServiceTests
    {
        private readonly IMemoryCache _cache;
        private readonly IWeatherRepository _repo;
        private readonly IWeatherService _service;

        public WeatherServiceTests()
        {
            _cache = new MemoryCache(new MemoryCacheOptions());
            _repo = new WeatherRepository();
            _service = new WeatherService(_cache, _repo);
        }

        [Fact]
        public async Task GetWeatherAsync_ReturnsCachedValue()
        {
            var city = "Berlin";

            // First call to cache it
            var first = await _service.GetWeatherAsync(city);
            var second = await _service.GetWeatherAsync(city);

            Assert.Equal(first, second); // Cached response must match
        }

        [Fact]
        public async Task GetWeatherAsync_ReturnsNonEmpty()
        {
            var result = await _service.GetWeatherAsync("Madrid");
            Assert.False(string.IsNullOrWhiteSpace(result));
        }
    }
}
