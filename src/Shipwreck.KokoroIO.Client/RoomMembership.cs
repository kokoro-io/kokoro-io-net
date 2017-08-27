using System;
using System.ComponentModel;
using Newtonsoft.Json;

namespace Shipwreck.KokoroIO
{
    [Serializable]
    public class RoomMembership
    {
        // TODO: Enum?
        [DefaultValue(null)]
        [JsonProperty("authority")]
        public string Authority { get; set; }

        [DefaultValue(0)]
        [JsonProperty("unread_count")]
        public int UnreadCount { get; set; }

        [DefaultValue(false)]
        [JsonProperty("disable_notification")]
        public bool DisableNotification { get; set; }
    }
}