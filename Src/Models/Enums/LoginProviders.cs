using Newtonsoft.Json;
using Newtonsoft.Json.Converters;

namespace Contactr.Models.Enums
{
    [JsonConverter(typeof(StringEnumConverter))]
    public enum LoginProviders
    {
        Google
    }
}