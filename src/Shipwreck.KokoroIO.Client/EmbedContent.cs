using System;
using System.ComponentModel;
using Newtonsoft.Json;

namespace Shipwreck.KokoroIO
{
    [Serializable]
    public class EmbedContent
    {
        [DefaultValue(null)]
        [JsonProperty("url")]
        public string Url { get; set; }

        [DefaultValue(null)]
        [JsonProperty("data")]
        public string Data { get; set; }
    }
}