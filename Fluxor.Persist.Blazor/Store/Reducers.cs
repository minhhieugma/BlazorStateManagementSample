using Fluxor.Persist.Storage.Store.Action;

namespace Fluxor.Persist.Storage.Store;

public static class Reducers
{
    [ReducerMethod]
    public static HistoryStackState ReduceAddTrailAction(HistoryStackState state, AddTrailAction action) =>
        new(state.Trails.Concat(new List<HistoryStackState.Trail> { action.Trail }).ToArray(), state.Payload);


    [ReducerMethod]
    public static HistoryStackState ReduceClearStagesAction(HistoryStackState state, ClearStagesAction action)
        => HistoryStackState.Empty;

    [ReducerMethod]
    public static HistoryStackState ReduceBackwardTraverseUntilAction(HistoryStackState state, BackwardTraverseUntilAction action)
    {
        var trails = state.Trails.ToList();

        while (trails.Any())
        {
            var current = trails.Last();

            if (current.Type == action?.Type)
                break;

            trails.Remove(current);
        }

        return new HistoryStackState(trails.ToArray(), state.Payload);
    }

    [ReducerMethod]
    public static HistoryStackState ReduceBackStageAction(HistoryStackState state, BackStageAction action) =>
        new(state.Trails.Any() ? state.Trails[..^1] : Array.Empty<HistoryStackState.Trail>(), state.Payload);

    [ReducerMethod]
    public static HistoryStackState ReduceUpdateTrailAction(HistoryStackState state, UpdateTrailAction action)
    {
        return new HistoryStackState(state.Trails[..^1].Concat(new List<HistoryStackState.Trail> { action.Trail }).ToArray(), state.Payload);
    }
}