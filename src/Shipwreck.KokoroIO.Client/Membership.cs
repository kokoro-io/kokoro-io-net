using System;
using System.ComponentModel;
using System.Runtime.Serialization;
using Newtonsoft.Json;

namespace Shipwreck.KokoroIO
{
    [Serializable, DataContract]
    public class Membership
    {
        // TODO: Enum?
        [DefaultValue(null)]
        [DataMember, JsonProperty("authority")]
        public string Authority { get; set; }

        [DefaultValue(0)]
        [DataMember, JsonProperty("unread_count")]
        public int UnreadCount { get; set; }

        [DefaultValue(false)]
        [DataMember, JsonProperty("disable_notification")]
        public bool DisableNotification { get; set; }
    }
}