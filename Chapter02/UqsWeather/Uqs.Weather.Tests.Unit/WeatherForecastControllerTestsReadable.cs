using Microsoft.Extensions.Logging;
using NSubstitute;
using Red.OpenWeather;
using Uqs.Weather.Controllers;
using Uqs.Weather.Tests.Unit.Builders;
using Uqs.Weather.Wrappers;

namespace Uqs.Weather.Tests.Unit;

public class WeatherForecastControllerTestsReadable
{
    private const double NEXT_T = 3.3;
    private const double DAY5_T = 7.7;
    private readonly DateTime _today = new DateTime(2024, 4, 17);
    private readonly double[] _realWeatherTemps = new[] { 2, NEXT_T, 4, 5.5, 6, DAY5_T, 8 };

    private readonly ILogger<WeatherForecastController> _loggerMock =
        Substitute.For<ILogger<WeatherForecastController>>();

    private readonly INowWrapper _nowWrapperMock = Substitute.For<INowWrapper>();
    private readonly IRandomWrapper _randomWrapperMock = Substitute.For<IRandomWrapper>();
    private readonly IClient _clientMock = Substitute.For<IClient>();
    private readonly WeatherForecastController _sut;

    public WeatherForecastControllerTestsReadable()
    {
        _sut = new WeatherForecastController(_loggerMock, _clientMock, _nowWrapperMock, _randomWrapperMock);
    }

    [Fact]
    public async Task GetReal_NotInterestedInTodayWeather_WFStartsFromNextDay()
    {
        // Arrange
        OneCallResponse res = new OneCallResponseBuilder()
            .SetTemps(new[] { 0, 3.3, 0, 0, 0, 0, 0 })
            .Build();
        
        _clientMock.OneCallAsync(
                Arg.Any<decimal>(),
                Arg.Any<decimal>(),
                Arg.Any<IEnumerable<Excludes>>(),
                Arg.Any<Units>())
            .Returns(res);

        // Act
        var wfs = await _sut.GetReal();

        // Assert
        Assert.Equal(3, wfs.First().TemperatureC);
    }
}