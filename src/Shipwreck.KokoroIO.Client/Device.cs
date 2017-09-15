using System;
using System.ComponentModel;
using System.Runtime.Serialization;
using Newtonsoft.Json;

namespace Shipwreck.KokoroIO
{
    [Serializable, DataContract]
    public class Device
    {
        [DefaultValue(null)]
        [DataMember, JsonProperty("name")]
        public string Name { get; set; }

        [DefaultValue(default(DeviceKind))]
        [DataMember, JsonProperty("kind")]
        public DeviceKind Kind { get; set; }

        [DefaultValue(null)]
        [DataMember, JsonProperty("device_identifier")]
        public string DeviceIdentifier { get; set; }

        [DefaultValue(null)]
        [DataMember, JsonProperty("notification_identifier")]
        public string NotificationIdentifier { get; set; }

        [DefaultValue(null)]
        [DataMember, JsonProperty("subscribe_notification")]
        public string SubscribeNotification { get; set; }

        [DefaultValue(null)]
        [DataMember, JsonProperty("last_activity_at")]
        public DateTime? LastActivityAt { get; set; }

        [DefaultValue(null)]
        [DataMember, JsonProperty("access_token")]
        public AccessToken AccessToken { get; set; }
    }
}