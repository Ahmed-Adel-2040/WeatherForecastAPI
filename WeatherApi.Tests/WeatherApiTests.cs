using System.Net;
using System.Net.Http.Headers;
using System.Text;
using System.Text.Json;
using Microsoft.AspNetCore.Mvc.Testing;
using Microsoft.Extensions.Caching.Memory;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.VisualStudio.TestPlatform.TestHost;
using WeatherForecastAPI.Repositories.Interfaces;
using WeatherForecastAPI.Repositories.Managers;
using WeatherForecastAPI.Services.ConcreateServices;
using WeatherForecastAPI.Services.IServices;
using Xunit;

namespace WeatherApi.Test;
public class WeatherApiTests : IClassFixture<WebApplicationFactory<Program>>
{
    private readonly WebApplicationFactory<Program> _factory;
    private readonly HttpClient _client;

    public WeatherApiTests(WebApplicationFactory<Program> factory)
    {
        _factory = factory;
        _client = factory.CreateClient();
    }

    [Fact]
    public async Task Register_NewUser_ReturnsSuccess()
    {
        var content = new StringContent(JsonSerializer.Serialize(new { Username = "testuser", Password = "testpass" }), Encoding.UTF8, "application/json");
        var response = await _client.PostAsync("/api/auth/register", content);
        Assert.Equal(HttpStatusCode.OK, response.StatusCode);
    }

    [Fact]
    public async Task Login_ValidUser_ReturnsToken()
    {
        var user = new { Username = "loginuser", Password = "loginpass" };

        // Register first
        var registerContent = new StringContent(JsonSerializer.Serialize(user), Encoding.UTF8, "application/json");
        await _client.PostAsync("/api/auth/register", registerContent);

        // Login
        var loginContent = new StringContent(JsonSerializer.Serialize(user), Encoding.UTF8, "application/json");
        var response = await _client.PostAsync("/api/auth/login", loginContent);
        var json = await response.Content.ReadAsStringAsync();

        Assert.Equal(HttpStatusCode.OK, response.StatusCode);
        Assert.Contains("token", json.ToLower());
    }

    [Fact]
    public async Task GetWeather_WithToken_ReturnsWeather()
    {
        var user = new { Username = "weatheruser", Password = "weatherpass" };

        // Register
        var registerContent = new StringContent(JsonSerializer.Serialize(user), Encoding.UTF8, "application/json");
        await _client.PostAsync("/api/auth/register", registerContent);

        // Login
        var loginContent = new StringContent(JsonSerializer.Serialize(user), Encoding.UTF8, "application/json");
        var loginResponse = await _client.PostAsync("/api/auth/login", loginContent);
        var loginJson = await loginResponse.Content.ReadAsStringAsync();
        var token = JsonSerializer.Deserialize<JsonElement>(loginJson).GetProperty("token").GetString();

        _client.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("Bearer", token);
        var weatherResponse = await _client.GetAsync("/api/weather?city=Cairo");

        Assert.Equal(HttpStatusCode.OK, weatherResponse.StatusCode);
        var result = await weatherResponse.Content.ReadAsStringAsync();
        Assert.Contains("Cairo", result);
    }

    [Fact]
    public async Task GetWeather_WithoutToken_ReturnsUnauthorized()
    {
        var response = await _client.GetAsync("/api/weather?city=London");
        Assert.Equal(HttpStatusCode.Unauthorized, response.StatusCode);
    }
}


