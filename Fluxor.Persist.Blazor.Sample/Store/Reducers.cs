using BlazorSSR.Store.WeatherUseCase;
using Fluxor;
using Fluxor.Persist.Storage.Store;
using Fluxor.Persist.Storage.Store.Action;

namespace BlazorSSR.Store;

public static class Reducers
{
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


    [ReducerMethod]
    public static HistoryStackState ReduceSelectCountryAction(HistoryStackState state, SelectCountryAction action)
    {
        var payload = state.PayloadAs<SimplePayload>();
        payload.CountryCode = action.CountryCode;

        return new HistoryStackState(state.Trails[..^1], payload);
    }


    [ReducerMethod]
    public static HistoryStackState ReduceCreateNewSessionAction(HistoryStackState state, CreateNewSessionAction action)

    {
        return new HistoryStackState(new[]
        {
            new HistoryStackState.Trail
            {
                Type = action.Type
            }
        }, action.Payload);
    }
}