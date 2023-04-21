using System.Text.Json;
using BlazorSSR.Data;
using Fluxor;

namespace BlazorSSR.Store;

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

        var a = JsonSerializer.Serialize(forecasts);

        dispatcher.Dispatch(new FetchDataResultAction(forecasts));
    }
}