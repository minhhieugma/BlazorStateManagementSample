using BlazorSSRFluxor.Data;

namespace BlazorSSRFluxor.Store;

public class FetchDataResultAction
{
    public IEnumerable<WeatherForecast> Forecasts { get; }

    public FetchDataResultAction(IEnumerable<WeatherForecast> forecasts)
    {
        Forecasts = forecasts;
    }
}