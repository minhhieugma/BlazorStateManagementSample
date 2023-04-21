namespace Fluxor.Persist.Storage.Store.Action;

/// <summary>
///     Backward Traverse to a previous trail
///     Stop when the target type is found
/// </summary>
public sealed record BackwardTraverseUntilAction(Type? Type);