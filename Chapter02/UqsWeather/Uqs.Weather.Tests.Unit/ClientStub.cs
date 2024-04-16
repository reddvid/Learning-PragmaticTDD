using Red.OpenWeather;

namespace Uqs.Weather.Tests.Unit;

public class ClientStub : IClient
{
    private readonly DateTime _now;
    private readonly IEnumerable<double> _sevenDaysTemps;

    public ClientStub(DateTime now, IEnumerable<double> sevenDaysTemps)
    {
        _now = now;
        _sevenDaysTemps = sevenDaysTemps;
    }

    public Units? LastUnitSpy { get; set; }
    
    public Task<OneCallResponse> OneCallAsync(
        decimal latitude,
        decimal longitude,
        IEnumerable<Excludes> excludes,
        Units unit)
    {
        LastUnitSpy = unit;
        const int DAYS = 7;
        OneCallResponse res = new();
        res.Daily = new Daily[DAYS];
        for (int i = 0; i < DAYS; i++)
        {
            res.Daily[i] = new();
            res.Daily[i].Dt = _now.AddDays(i);
            res.Daily[i].Temp = new();
            res.Daily[i].Temp.Day = _sevenDaysTemps.ElementAt(i);
        }

        return Task.FromResult(res);
    }
}