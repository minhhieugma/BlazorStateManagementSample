namespace Fluxor.Persist.Storage.Store.Action;

public sealed class AddTrailAction
{
    public HistoryStackState.Trail Trail { get; init; } = new();
}