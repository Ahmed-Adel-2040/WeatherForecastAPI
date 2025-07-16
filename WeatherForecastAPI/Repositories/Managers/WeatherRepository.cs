using WeatherForecastAPI.Repositories.Interfaces;

namespace WeatherForecastAPI.Repositories.Managers
{
    public class WeatherRepository : IWeatherRepository
    {
        public Task<string> FetchWeatherForCityAsync(string city)
        {
            var temp = new Random().Next(15, 35);
            return Task.FromResult($"Weather in {city}: {temp}°C");
        }
    }
}
