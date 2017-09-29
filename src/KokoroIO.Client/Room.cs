using System;
using System.ComponentModel;
using System.Runtime.Serialization;
using Newtonsoft.Json;
using Newtonsoft.Json.Converters;

namespace KokoroIO
{
    [Serializable, DataContract]
    public class Room
    {
        [DefaultValue(null)]
        [DataMember, JsonProperty("id")]
        public string Id { get; set; }

        [DefaultValue(null)]
        [DataMember, JsonProperty("channel_name")]
        public string ChannelName { get; set; }

        [DefaultValue(default(RoomKind))]
        [DataMember, JsonProperty("kind")]
        [JsonConverter(typeof(StringEnumConverter))]
        public RoomKind Kind { get; set; }

        [DefaultValue(false)]
        [DataMember, JsonProperty("archived")]
        public bool IsArchived { get; set; }

        [DefaultValue(null)]
        [DataMember, JsonProperty("description")]
        public string Description { get; set; }

        [DefaultValue(null)]
        [DataMember, JsonProperty("membership")]
        public Membership Membership { get; set; }

        [DefaultValue(null)]
        [DataMember, JsonProperty("memberships")]
        public Membership[] Memberships { get; set; }

        public static bool IsValidId(string id)
        {
            if (string.IsNullOrEmpty(id))
            {
                return false;
            }
            foreach (var c in id)
            {
                if (!(('0' <= c && c <= '9') || 'A' <= c && c <= 'Z' || 'a' <= c && c <= 'z'))
                {
                    return false;
                }
            }
            return true;
        }
    }
}