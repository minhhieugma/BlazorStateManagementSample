using BlazorSSR.Store.StageUseCase;

namespace BlazorSSR.Store;

public sealed class AddTrailAction
{
    public StageState.Trail Trail { get; init; } = new();
}