namespace WeatherForecastAPI.Services.IServices
{
    public interface IWeatherService
    {
        Task<string> GetWeatherAsync(string city);
    }
}
