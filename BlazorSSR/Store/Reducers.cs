using BlazorSSR.Store.StageUseCase;
using BlazorSSR.Store.WeatherUseCase;
using Fluxor;

namespace BlazorSSR.Store.CounterUseCase;

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


    [ReducerMethod]
    public static StageState ReduceAddTrailAction(StageState state, AddTrailAction action) =>
        new(state.Trails.Concat(new[] { action.Trail }).ToArray());

    [ReducerMethod]
    public static StageState ReduceBackwardTraverseUntilAction(StageState state, BackwardTraverseUntilAction action)
    {
        var trails = state.Trails.ToList();

        while (trails.Any())
        {
            var current = trails.Last();

            if (current.Type == action?.Type)
                break;

            trails.Remove(current);
        }

        return new StageState(trails.ToArray());
    }

    [ReducerMethod]
    public static StageState ReduceBackStageAction(StageState state, BackStageAction action) =>
        new(state.Trails.Any() ? state.Trails[..^1] : Array.Empty<StageState.Trail>());

    [ReducerMethod]
    public static StageState ReduceUpdateTrailAction(StageState state, UpdateTrailAction action)
    {
        return new StageState(state.Trails[..^1].Concat(new[] { action.Trail }).ToArray());
    }
}