using Blazored.LocalStorage;
using Fluxor.Persist.Storage;

namespace BlazorSSR.Store;

public class LocalStateStorage : IStringStateStorage
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