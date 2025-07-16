using Microsoft.IdentityModel.Tokens;
using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Text;
using WeatherForecastAPI.Services.IServices;

namespace WeatherForecastAPI.Services.ConcreateServices
{
    public class UserService : IUserService
    {
        private static readonly Dictionary<string, string> users = new();
        private readonly IConfiguration _config;

        public UserService(IConfiguration config) => _config = config;

        public bool Register(string username, string password)
        {
            if (users.ContainsKey(username)) return false;
            users[username] = password;
            return true;
        }

        public string Login(string username, string password)
        {
            if (users.TryGetValue(username, out var storedPassword) && storedPassword == password)
            {
                return GenerateJwtToken(username);
            }
            return null;
        }

        private string GenerateJwtToken(string username)
        {
            var claims = new[] { new Claim(ClaimTypes.Name, username) };
            var key = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(_config["Jwt:Key"]));
            var creds = new SigningCredentials(key, SecurityAlgorithms.HmacSha256);
            var token = new JwtSecurityToken(
                issuer: _config["Jwt:Issuer"],
                audience: _config["Jwt:Audience"],
                claims: claims,
                expires: DateTime.Now.AddMinutes(60),
                signingCredentials: creds);

            return new JwtSecurityTokenHandler().WriteToken(token);
        }
    }
}
