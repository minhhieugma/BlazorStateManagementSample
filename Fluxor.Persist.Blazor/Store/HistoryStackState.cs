using System.Reflection;
using System.Text.Json;
using System.Text.Json.Serialization;

namespace Fluxor.Persist.Storage.Store;

[FeatureState(Name = nameof(HistoryStackState))]
public class HistoryStackState
{
    public Trail[] Trails { get; protected set; } = Array.Empty<Trail>();

    public string Payload { get; set; }

    private object? _payload;

    public T PayloadAs<T>()
    {
        _payload ??= JsonSerializer.Deserialize<T>(Payload ?? "{}");

        return ((T) _payload!)!;
    }

    // Required for creating initial state
    protected HistoryStackState() { }

    [JsonConstructor]
    public HistoryStackState(Trail[] trails, string payload)
    {
        Trails = trails.ToArray();
        Payload = payload;
    }

    public HistoryStackState(Trail[] trails, object payload)
    {
        Trails = trails.ToArray();
        Payload = JsonSerializer.Serialize(payload);
    }


    public static HistoryStackState Empty
    {
        get => new();
    }

    public class Trail
    {
        public string? ComponentType { get; set; }

        [JsonConverter(typeof(DictionaryStringObjectJsonConverter))]
        public Dictionary<string, object> ComponentParameters { get; set; } = new();

        [JsonIgnore]
        public Type? Type
        {
            get => GetTypeFromName(ComponentType);
            set => ComponentType = value?.FullName!;
        }

        private Type? GetTypeFromName(string? fullName)
        {
            if (string.IsNullOrEmpty(fullName))
                return null;
            var type = Assembly.GetEntryAssembly()?.GetType(fullName);

            return type!;
        }

        // Required for creating initial state
        public Trail() { }
    }
}