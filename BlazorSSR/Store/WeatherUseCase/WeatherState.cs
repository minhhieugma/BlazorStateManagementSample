using BlazorSSR.Data;
using Fluxor;

namespace BlazorSSR.Store.WeatherUseCase;

[FeatureState(Name = nameof(WeatherState))]
public sealed class WeatherState
{
    public bool IsLoading { get; }
    public IEnumerable<WeatherForecast> Forecasts { get; }

    private WeatherState() { } // Required for creating initial state

    public WeatherState(bool isLoading, IEnumerable<WeatherForecast> forecasts)
    {
        IsLoading = isLoading;
        Forecasts = forecasts ?? Array.Empty<WeatherForecast>();
    }
}