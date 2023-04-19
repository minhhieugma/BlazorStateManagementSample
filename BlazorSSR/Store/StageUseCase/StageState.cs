using System.Text.Json.Serialization;
using Fluxor;

namespace BlazorSSR.Store.StageUseCase;

[FeatureState(Name = nameof(StageState))]
public class StageState
{
    public Trail[] Trails { get; } = Array.Empty<Trail>();

    public string CountryCode { get; } = "VN";

    // Required for creating initial state
    private StageState() { }

    [JsonConstructor]
    public StageState(Trail[] trails, string countryCode)
    {
        Trails = trails.ToArray();
        CountryCode = countryCode;
    }

    public static StageState Empty
    {
        get => new();
    }

    public class Trail
    {
        public string ComponentType { get; set; } = default!;

        [JsonConverter(typeof(DictionaryStringObjectJsonConverter))]
        public Dictionary<string, object> ComponentParameters { get; set; } = new();

        [JsonIgnore]
        public Type Type
        {
            get => GetTypeFromName(ComponentType);
            set => ComponentType = value.FullName!;
        }

        private Type GetTypeFromName(string fullName)
        {
            if (fullName == null)
                return null;
            var type = Type.GetType(fullName);

            return type!;
        }

        // Required for creating initial state
        public Trail() { }
    }
}