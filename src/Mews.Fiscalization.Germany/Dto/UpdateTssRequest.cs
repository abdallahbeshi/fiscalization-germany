using Newtonsoft.Json;
using Newtonsoft.Json.Converters;

namespace Mews.Fiscalization.Germany.Dto
{
    public sealed class UpdateTssRequest
    {
        [JsonProperty("state")]
        [JsonConverter(typeof(StringEnumConverter))]
        public TssState State { get; set; }
    }
}