namespace WeatherForecastAPI.Repositories.Interfaces
{
    public interface IWeatherRepository
    {
        Task<string> FetchWeatherForCityAsync(string city);
    }
}
