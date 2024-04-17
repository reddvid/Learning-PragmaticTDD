using System.Net.Http.Json;

namespace Uqs.Weather.Tests.Integration;

public class WeatherForecastTests
{
    private const string BASE_ADDRESS = "https://localhost:7242";
    private const string API_URI = "/WeatherForecast/GetRealWeatherForecast";

    private record WeatherForecast(DateTime Date, int TemperatureC, int TemperatureF, string? Summary);

    [Fact]
    public async Task GetRealWeatherForecast_Execute_GetNext5Days()
    {
        // Arrange
        HttpClient httpClient = new() { BaseAddress = new Uri(BASE_ADDRESS) };
        var today = DateTime.Now.Date;
        var next5Days = new[]
            { today.AddDays(1), today.AddDays(2), today.AddDays(3), today.AddDays(4), today.AddDays(5) };

        // Act
        var httpRes = await httpClient.GetAsync(API_URI);

        // Assert
        var wfs = await httpRes.Content.ReadFromJsonAsync<WeatherForecast[]>();
        for (int i = 0; i < 5; i++)
        {
            Assert.Equal(next5Days[i], wfs[i].Date.Date);
        }
    }
    
    
}