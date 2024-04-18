using Red.OpenWeather;

namespace Uqs.Weather.Tests.Unit.Builders;

public class OneCallResponseBuilder
{
    private int _days = 7;
    private DateTime _today = new(2024, 4, 18);
    private double[] _temps = { 2, 3.3, 4, 5.5, 6, 7.7, 8 };

    public OneCallResponseBuilder SetDays(int days)
    {
        _days = days;
        return this;
    }

    public OneCallResponseBuilder SetToday(DateTime today)
    {
        _today = today;
        return this;
    }

    public OneCallResponseBuilder SetTemps(double[] temps)
    {
        _temps = temps;
        return this;
    }

    public OneCallResponse Build()
    {
        var res = new OneCallResponse();
        res.Daily = new Daily[_days];
        for (int i = 0; i < _days; i++)
        {
            res.Daily[i] = new();
            res.Daily[i].Dt = _today.AddDays(i);
            res.Daily[i].Temp = new();
            res.Daily[i].Temp.Day = _temps.ElementAt(i);
        }

        return res;
    }
}