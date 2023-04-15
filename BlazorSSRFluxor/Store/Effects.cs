using BlazorSSRFluxor.Data;
using Fluxor;

namespace BlazorSSRFluxor.Store;

public class Effects
{
    private readonly WeatherForecastService _weatherForecastService;

    public Effects(WeatherForecastService weatherForecastService)
    {
        _weatherForecastService = weatherForecastService;
    }

    [EffectMethod]
    public async Task HandleFetchDataAction(FetchDataAction action, IDispatcher dispatcher)
    {
        var forecasts = await _weatherForecastService.GetForecastAsync(DateOnly.FromDateTime(DateTime.Now));
        dispatcher.Dispatch(new FetchDataResultAction(forecasts));
    }
}