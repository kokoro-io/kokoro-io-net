using System;
using System.ComponentModel;
using System.Runtime.Serialization;
using Newtonsoft.Json;
using Newtonsoft.Json.Converters;

namespace KokoroIO
{
    [Serializable, DataContract]
    public class Membership
    {
        [DefaultValue(null)]
        [DataMember, JsonProperty("id")]
        public string Id { get; set; }

        [DefaultValue(null)]
        [DataMember, JsonProperty("channel")]
        public Channel Channel { get; set; }

        [DefaultValue(default(Authority))]
        [DataMember, JsonProperty("authority")]
        [JsonConverter(typeof(StringEnumConverter))]
        public Authority Authority { get; set; }

        [DefaultValue(0)]
        [DataMember, JsonProperty("unread_count")]
        public int UnreadCount { get; set; }

        [DefaultValue(false)]
        [DataMember, JsonProperty("disable_notification")]
        [Obsolete]
        public bool DisableNotification { get; set; }

        [DefaultValue(default(NotificationPolicy))]
        [DataMember, JsonProperty("notification_policy")]
        [JsonConverter(typeof(StringEnumConverter))]
        public NotificationPolicy NotificationPolicy { get; set; }

        [DefaultValue(default(Authority))]
        [DataMember, JsonProperty("read_state_tracking_policy")]
        [JsonConverter(typeof(StringEnumConverter))]
        public ReadStateTrackingPolicy ReadStateTrackingPolicy { get; set; }

        [DefaultValue(null)]
        [DataMember, JsonProperty("latest_read_message_id")]
        public int? LatestReadMessageId { get; set; }

        [DefaultValue(null)]
        [DataMember, JsonProperty("profile")]
        public Profile Profile { get; set; }
    }
}