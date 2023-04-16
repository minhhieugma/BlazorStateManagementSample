using BlazorSSR.Store.StageUseCase;

namespace BlazorSSR.Store;

public sealed class UpdateTrailAction
{
    public StageState.Trail Trail { get; init; } = new();
}