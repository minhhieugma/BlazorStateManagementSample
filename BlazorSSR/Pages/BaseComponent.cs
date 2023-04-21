using Fluxor;
using Fluxor.Persist.Middleware;
using Microsoft.AspNetCore.Components;
using Microsoft.AspNetCore.WebUtilities;

namespace BlazorSSR.Pages;

public abstract class BaseComponent : ComponentBase, IAsyncDisposable
{
    [Inject] public IActionSubscriber ActionSubscriber { get; private set; } = default!;

    [Inject] public IDispatcher Dispatcher { get; private set; } = default!;

    [Inject] public NavigationManager NavigationManager { get; set; } = default!;

    public string SessionId { get; set; }

    protected override void OnInitialized()
    {
        base.OnInitialized();

        var parsedParams = QueryHelpers.ParseQuery(new Uri(NavigationManager.Uri).Query);
        if (parsedParams.TryGetValue("session", out var sessionId))
            SessionId = sessionId.ToString();
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