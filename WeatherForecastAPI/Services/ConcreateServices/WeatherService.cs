using Microsoft.Extensions.Caching.Memory;
using WeatherForecastAPI.Repositories.Interfaces;
using WeatherForecastAPI.Services.IServices;

namespace WeatherForecastAPI.Services.ConcreateServices
{
    public class WeatherService : IWeatherService
    {
        private readonly IMemoryCache _cache;
        private readonly IWeatherRepository _weatherRepo;

        public WeatherService(IMemoryCache cache, IWeatherRepository weatherRepo)
        {
            _cache = cache;
            _weatherRepo = weatherRepo;
        }

        public async Task<string> GetWeatherAsync(string city)
        {
            if (_cache.TryGetValue(city, out string cachedWeather))
                return cachedWeather;

            var weather = await _weatherRepo.FetchWeatherForCityAsync(city);
            _cache.Set(city, weather, TimeSpan.FromMinutes(5));
            return weather;
        }
    }
}
