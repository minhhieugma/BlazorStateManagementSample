namespace Fluxor.Persist.Storage.Store.Action;

public sealed class UpdateTrailAction
{
    public HistoryStackState.Trail Trail { get; init; } = new();
}