namespace Fluxor.Persist.Storage.Store.Action;

public class CreateNewSessionAction
{
    public Type Type { get; set; } = default!;

    public string Payload { get; set; } = default!;
}