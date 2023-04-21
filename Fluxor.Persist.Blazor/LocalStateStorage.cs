using Blazored.LocalStorage;

namespace Fluxor.Persist.Storage;

/// <summary>
///     A single set of states for a single client (browser)
/// </summary>
public sealed class LocalStateStorage : IStringStateStorage
{
    private ILocalStorageService LocalStorage { get; set; }

    public LocalStateStorage(ILocalStorageService localStorage)
    {
        LocalStorage = localStorage;
    }

    public async ValueTask<string> GetStateJsonAsync(string statename)
    {
        var str = await LocalStorage.GetItemAsStringAsync(statename);

        return str;
    }

    public async ValueTask StoreStateJsonAsync(string statename, string json)
    {
        await LocalStorage.SetItemAsStringAsync(statename, json);
    }
}