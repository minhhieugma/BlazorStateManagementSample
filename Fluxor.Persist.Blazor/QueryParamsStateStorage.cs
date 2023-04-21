using Microsoft.AspNetCore.Components;
using Microsoft.AspNetCore.WebUtilities;
using Microsoft.Extensions.Logging;
using Microsoft.Extensions.Primitives;

namespace Fluxor.Persist.Storage;

/// <summary>
///     The states are stored in the query parameters
/// </summary>
public sealed class QueryParamsStateStorage : IStringStateStorage
{
    private readonly NavigationManager _navigationManager;
    private readonly ILogger _logger;

    private string _url;
    private readonly string _urlWithoutParams;
    private readonly Dictionary<string, StringValues> _parsedParams;

    public QueryParamsStateStorage(NavigationManager navigationManager,
        ILogger<QueryParamsStateStorage> logger)
    {
        _navigationManager = navigationManager;
        _logger = logger;

        _url = _navigationManager.Uri;
        var uri = new Uri(_url);
        _parsedParams = QueryHelpers.ParseQuery(uri.Query);
        _urlWithoutParams = uri.GetLeftPart(UriPartial.Path);
    }

    public ValueTask<string> GetStateJsonAsync(string statename)
    {
        try
        {
            _parsedParams.TryGetValue(statename, out var value);

            return ValueTask.FromResult(value.ToString());
        }
        catch (Exception e)
        {
            _logger.LogError(e, "Cannot extract value of {StateName} from {Uri}", statename, _navigationManager.Uri);

            return ValueTask.FromResult(string.Empty);
        }
    }

    public ValueTask StoreStateJsonAsync(string statename, string json)
    {
        try
        {
            ReplaceQueryParamValue(statename, json);

            _navigationManager.NavigateTo(_url, forceLoad: false, replace: true);
        }
        catch (Exception e)
        {
            _logger.LogError(e, "Cannot save value {NewValue} of {StateName} to {Uri}", json, statename, _navigationManager.Uri);
        }

        return ValueTask.CompletedTask;
    }

    private void ReplaceQueryParamValue(string queryParamName, string newValue)
    {
        _parsedParams.Remove(queryParamName);
        _parsedParams.Add(queryParamName, newValue);

        _url = QueryHelpers.AddQueryString(_urlWithoutParams, _parsedParams);
    }
}