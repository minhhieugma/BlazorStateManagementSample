using BlazorSSRFluxor.Store.WeatherUseCase;
using Fluxor;

namespace BlazorSSRFluxor.Store.CounterUseCase;

public static class Reducers
{
    [ReducerMethod]
    public static CounterState ReduceIncrementCounterAction(CounterState state, IncrementCounterAction action) =>
        new(clickCount: state.ClickCount + 1);

    [ReducerMethod]
    public static WeatherState ReduceFetchDataAction(WeatherState state, FetchDataAction action) =>
        new(
            isLoading: true,
            forecasts: null);

    [ReducerMethod]
    public static WeatherState ReduceFetchDataResultAction(WeatherState state, FetchDataResultAction action) =>
        new(
            isLoading: false,
            forecasts: action.Forecasts);
}