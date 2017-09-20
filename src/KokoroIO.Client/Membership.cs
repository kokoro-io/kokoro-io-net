using System;
using System.ComponentModel;
using System.Runtime.Serialization;
using Newtonsoft.Json;

namespace KokoroIO
{
    [Serializable, DataContract]
    public class Membership
    {
        [DefaultValue(null)]
        [DataMember, JsonProperty("id")]
        public string Id { get; set; }

        [DefaultValue(null)]
        [DataMember, JsonProperty("room")]
        public Room Room { get; set; }

        [DefaultValue(default(Authority))]
        [DataMember, JsonProperty("authority")]
        public Authority Authority { get; set; }

        [DefaultValue(0)]
        [DataMember, JsonProperty("unread_count")]
        public int UnreadCount { get; set; }

        [DefaultValue(false)]
        [DataMember, JsonProperty("disable_notification")]
        public bool DisableNotification { get; set; }

        [DefaultValue(null)]
        [DataMember, JsonProperty("profile")]
        public Profile Profile { get; set; }
    }
}