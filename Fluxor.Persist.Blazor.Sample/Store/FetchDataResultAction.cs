using BlazorSSR.Data;

namespace BlazorSSR.Store;

public class FetchDataResultAction
{
    public IEnumerable<WeatherForecast> Forecasts { get; }

    public FetchDataResultAction(IEnumerable<WeatherForecast> forecasts)
    {
        Forecasts = forecasts;
    }
}