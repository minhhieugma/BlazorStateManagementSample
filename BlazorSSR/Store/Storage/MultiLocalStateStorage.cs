using Blazored.LocalStorage;
using Fluxor.Persist.Storage;
using Microsoft.AspNetCore.Components;
using Microsoft.AspNetCore.WebUtilities;
using Microsoft.Extensions.Primitives;

namespace BlazorSSR.Store.Storage;

/// <summary>
///     Each session will have different set of states
/// </summary>
public sealed class MultiLocalStateStorage : IStringStateStorage
{
    private readonly NavigationManager _navigationManager;
    private readonly ILogger _logger;
    private readonly ILocalStorageService _localStorage;

    private string _url;
    private readonly string _urlWithoutParams;
    private readonly Dictionary<string, StringValues> _parsedParams;
    private readonly SemaphoreSlim _semaphoreSlim = new(1);

    public MultiLocalStateStorage(
        ILocalStorageService localStorage,
        NavigationManager navigationManager,
        ILogger<MultiLocalStateStorage> logger)
    {
        _navigationManager = navigationManager;
        _logger = logger;
        _localStorage = localStorage;

        _url = _navigationManager.Uri;

        var uri = new Uri(_url);
        _parsedParams = QueryHelpers.ParseQuery(uri.Query);
        _urlWithoutParams = uri.GetLeftPart(UriPartial.Path);
    }

    public async ValueTask<string> GetStateJsonAsync(string statename)
    {
        try
        {
            var sessionId = GetSessionId();
            if (string.IsNullOrEmpty(sessionId))
                return null;

            var localStorageKey = await GetLocalStorageKey(statename);

            var str = await _localStorage.GetItemAsStringAsync(localStorageKey);

            return str;
        }
        catch (Exception e)
        {
            _logger.LogError(e, "Cannot extract value of {StateName} from {Uri}", statename, _navigationManager.Uri);

            return null;
        }
    }

    public async ValueTask StoreStateJsonAsync(string statename, string json)
    {
        var sessionId = GetSessionId();
        if (string.IsNullOrEmpty(sessionId))
            return;


        var localStorageKey = await GetLocalStorageKey(statename);

        await _localStorage.SetItemAsStringAsync(localStorageKey, json);
    }

    private string GetLocalStorageKeyOrCreate(string stateName, Session session, List<Session> sessions)
    {
        session.Keys.TryGetValue(stateName, out var localStorageKey);
        if (!string.IsNullOrEmpty(localStorageKey)) return localStorageKey;

        localStorageKey = Guid.NewGuid().ToString();
        session.Keys.Add(stateName, localStorageKey);

        return localStorageKey;
    }

    private async Task<string> GetLocalStorageKey(string stateName)
    {
        await _semaphoreSlim.WaitAsync();
        var sessionId = CreateNewSessionIdIfNot();

        var (sessions, session) = await GetOrCreateSessionAsync(sessionId);

        var localStorageKey = GetLocalStorageKeyOrCreate(stateName, session, sessions);

        await _localStorage.SetItemAsync("sessions", sessions);
        _semaphoreSlim.Release();

        return localStorageKey;
    }

    private async Task<(List<Session> Sessions, Session Session)> GetOrCreateSessionAsync(StringValues sessionId)
    {
        var sessions = new List<Session>();
        await GetOrCreateSessionListAsync();
        OnlyKeepLast7DaySessions();

        var session = sessions.SingleOrDefault(p => p.SessionId == sessionId);
        if (session is null)
        {
            session = new Session(sessionId!);
            sessions.Add(session);
        }
        else
        {
            session.LastModified = DateTime.UtcNow;
        }

        return (sessions, session);

        async Task GetOrCreateSessionListAsync()
        {
            sessions = await _localStorage.GetItemAsync<List<Session>>("sessions");
            if (sessions is null)
            {
                sessions = new List<Session>();
            }
        }

        void OnlyKeepLast7DaySessions()
        {
            sessions = sessions
                .Where(p => p.LastModified >= DateTime.UtcNow.AddDays(-7))
                .ToList();
        }
    }

    private string? GetSessionId()
    {
        _parsedParams.TryGetValue("session", out var sessionId);
        if (string.IsNullOrWhiteSpace(sessionId.ToString()))
        {
            // sessionId = Guid.NewGuid().ToString();
            // ReplaceQueryParamValue("session", sessionId!);
            // _navigationManager.NavigateTo(_url, forceLoad: false, replace: true);

            return null;
        }

        return sessionId;
    }

    private StringValues CreateNewSessionIdIfNot()
    {
        _parsedParams.TryGetValue("session", out var sessionId);
        if (string.IsNullOrWhiteSpace(sessionId.ToString()))
        {
            sessionId = Guid.NewGuid().ToString();
            ReplaceQueryParamValue("session", sessionId!);
            _navigationManager.NavigateTo(_url, forceLoad: false, replace: true);
        }

        return sessionId;
    }

    private void ReplaceQueryParamValue(string queryParamName, string newValue)
    {
        _parsedParams.Remove(queryParamName);
        _parsedParams.Add(queryParamName, newValue);

        _url = QueryHelpers.AddQueryString(_urlWithoutParams, _parsedParams);
    }

    public sealed class Session
    {
        public string SessionId { get; set; }
        public DateTime LastModified { get; set; }
        public Dictionary<string, string> Keys { get; set; }

        public Session(string sessionId)
        {
            SessionId = sessionId;
            LastModified = DateTime.UtcNow;
            Keys = new Dictionary<string, string>();
        }
    }
}