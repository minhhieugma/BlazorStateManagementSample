using Fluxor;
using Fluxor.Persist.Middleware;
using Microsoft.AspNetCore.Components;

namespace BlazorSSR.Pages;

public abstract class BaseComponent : ComponentBase, IAsyncDisposable
{
    [Inject] public IActionSubscriber ActionSubscriber { get; private set; } = default!;

    [Inject] public IDispatcher Dispatcher { get; private set; } = default!;


    protected override void OnInitialized()
    {
        base.OnInitialized();
    }

    protected override async Task OnInitializedAsync()
    {
        await base.OnInitializedAsync();

        ActionSubscriber.SubscribeToAction<InitializePersistMiddlewareResultSuccessAction>(this, async result =>
        {
            // we now have state, we can re-render to reflect binding changes
            await InvokeAsync(StateHasChanged);
        });
    }

    public virtual ValueTask DisposeAsync()
    {
        ActionSubscriber.UnsubscribeFromAllActions(this);

        return ValueTask.CompletedTask;
    }
}