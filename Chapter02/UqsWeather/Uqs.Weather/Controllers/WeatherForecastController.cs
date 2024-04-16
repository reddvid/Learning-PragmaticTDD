using System.Net;
using Microsoft.AspNetCore.Mvc;
using Newtonsoft.Json;
using Red.OpenWeather;
using Uqs.Weather.Wrappers;

namespace Uqs.Weather.Controllers;

[ApiController]
[Route("[controller]")]
public class WeatherForecastController : ControllerBase
{
    private const int FORECAST_DAYS = 5;
    private readonly ILogger<WeatherForecastController> _logger;
    private readonly IClient _client;
    private readonly INowWrapper _nowWrapper;
    private readonly IRandomWrapper _randomWrapper;
    
    private static readonly string[] Summaries =
        ["Freezing", "Bracing", "Chilly", "Cool", "Mild", "Warm", "Balmy", "Hot", "Sweltering", "Scorching"];

    private string MapFeelToTemp(int temperatureC)
    {
        if (temperatureC <= 0) return Summaries.First();
        int summariesIndex = temperatureC / 5 + 1;
        if (summariesIndex >= Summaries.Length) return Summaries.Last();
        return Summaries[summariesIndex];
    }

    public WeatherForecastController(
        ILogger<WeatherForecastController> logger, 
        IClient client,
        INowWrapper nowWrapper,
        IRandomWrapper randomWrapper)
    {
        _client = client;
        _logger = logger;
        _nowWrapper = nowWrapper;
        _randomWrapper = randomWrapper;
    }

    [HttpGet("ConvertCToF")]
    public double ConvertCToF(double c)
    {
        double f = c * (9d / 5d) + 32;
        _logger.LogInformation("conversion requested");
        return f;
    }

    [HttpGet("GetRealWeatherForecast")]
    public async Task<IEnumerable<WeatherForecast>> GetReal()
    {
        const decimal PH_LAT = 14.63m;
        const decimal PH_LON = 121.03m;

        OneCallResponse response = await _client.OneCallAsync(PH_LAT, PH_LON, new[]
        {
            Excludes.Alerts,
            Excludes.Current,
            Excludes.Minutely,
            Excludes.Hourly
        }, Units.Metric);
        
        WeatherForecast[] weatherForecasts = new WeatherForecast[FORECAST_DAYS];
        for (int i = 0; i < weatherForecasts.Length; i++)
        {
            var weatherForecast = weatherForecasts[i] = new WeatherForecast();
            weatherForecast.Date = response.Daily[i + 1].Dt;
            double forecastedTemp = response.Daily[i + 1].Temp.Day;
            weatherForecast.TemperatureC = (int)Math.Round(forecastedTemp);
            weatherForecast.Summary = MapFeelToTemp(weatherForecast.TemperatureC);
        }
        return weatherForecasts;
    }

    [HttpGet("GetRandomWeatherForecast")]
    public IEnumerable<WeatherForecast> GetRandom()
    {
        WeatherForecast[] weatherForecasts = new WeatherForecast[FORECAST_DAYS];

        for (int i = 0; i < weatherForecasts.Length; i++)
        {
            var weatherForecast = weatherForecasts[i] = new WeatherForecast();
            weatherForecast.Date = _nowWrapper.Now.AddDays(i + 1);
            weatherForecast.TemperatureC = _randomWrapper.Next(-20, 55);
            weatherForecast.Summary = MapFeelToTemp(weatherForecast.TemperatureC);
        }

        return weatherForecasts;
    }
}