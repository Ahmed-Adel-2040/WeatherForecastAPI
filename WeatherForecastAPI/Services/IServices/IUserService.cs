namespace WeatherForecastAPI.Services.IServices
{
    public interface IUserService
    {
        bool Register(string username, string password);
        string Login(string username, string password);
    }
}
