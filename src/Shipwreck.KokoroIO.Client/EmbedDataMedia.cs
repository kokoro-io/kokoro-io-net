using System.ComponentModel;
using System.Runtime.Serialization;
using Newtonsoft.Json;
using Newtonsoft.Json.Converters;

namespace Shipwreck.KokoroIO
{
    /// <summary>
    /// Represents a media information.
    /// </summary>
    [DataContract]
    public class EmbedDataMedia
    {
        /// <summary>
        /// Gets the media type. Valid values, along with value-specific parameters, are described below.
        /// </summary>
        [DefaultValue(default(EmbedDataMediaType))]
        [DataMember, JsonProperty("type")]
        [JsonConverter(typeof(StringEnumConverter))]
        public EmbedDataMediaType Type { get; set; }

        /// <summary>
        /// Gets or sets a Thumbnail image URL
        /// </summary>
        [DefaultValue(null)]
        [DataMember, JsonProperty("thumbnail")]
        public EmbedDataImageInfo Thumbnail { get; set; }

        /// <summary>
        /// Gets or sets a Raw resouce URL
        /// </summary>
        [DefaultValue(null)]
        [DataMember, JsonProperty("raw_url")]
        public string RawUrl { get; set; }

        /// <summary>
        /// Gets or sets a URL of Media resouce. The resouce can be HTML page which contains the media.
        /// It doesn't have to be a raw media.
        /// </summary>
        [DefaultValue(null)]
        [DataMember, JsonProperty("location")]
        public string Location { get; set; }

        /// <summary>
        /// Gets a content restriction policy that applied to the media.
        /// </summary>
        [DefaultValue(RestrictionPolicy.Unknown)]
        [DataMember, JsonProperty("restriction_policy")]
        [JsonConverter(typeof(StringEnumConverter))]
        public RestrictionPolicy RestrictionPolicy { get; set; } = RestrictionPolicy.Unknown;
    }
}