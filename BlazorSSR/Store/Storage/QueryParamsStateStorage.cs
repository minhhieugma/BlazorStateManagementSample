using Fluxor.Persist.Storage;
using Microsoft.AspNetCore.Components;
using Microsoft.AspNetCore.WebUtilities;
using Microsoft.Extensions.Primitives;

namespace BlazorSSR.Store.Storage;

public sealed class QueryParamsStateStorage : IStringStateStorage
{
    private readonly NavigationManager _navigationManager;
    private readonly ILogger _logger;

    public QueryParamsStateStorage(NavigationManager navigationManager,
        ILogger<QueryParamsStateStorage> logger)
    {
        _navigationManager = navigationManager;
        _logger = logger;
    }

    public ValueTask<string> GetStateJsonAsync(string statename)
    {
        try
        {
            var parsedParams = GetParsedParams();
            parsedParams.TryGetValue(statename, out var value);

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
            var parsedParams = GetParsedParams();

            var newUri = ReplaceQueryParamValue(parsedParams, statename, json);

            _navigationManager.NavigateTo(newUri, forceLoad: false, replace: true);
        }
        catch (Exception e)
        {
            _logger.LogError(e, "Cannot save value {NewValue} of {StateName} to {Uri}", json, statename, _navigationManager.Uri);
        }

        return ValueTask.CompletedTask;
    }

    private string ReplaceQueryParamValue(
        Dictionary<string, StringValues> parsedParams, string queryParamName, string newValue)
    {
        parsedParams.Remove(queryParamName);
        parsedParams.Add(queryParamName, newValue);

        var uri = GetCurrentUri();
        var path = uri.GetLeftPart(UriPartial.Path);


        var newUri = QueryHelpers.AddQueryString(path, parsedParams);

        return newUri;
    }

    private Dictionary<string, StringValues> GetParsedParams()
    {
        var uri = GetCurrentUri();

        var parsedParams = QueryHelpers.ParseQuery(uri.Query);

        return parsedParams;
    }

    private Uri GetCurrentUri()
    {
        var uri = new Uri(_navigationManager.Uri);
        return uri;
    }
}