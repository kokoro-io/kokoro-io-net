using System;
using System.ComponentModel;
using System.Runtime.Serialization;
using Newtonsoft.Json;
using Newtonsoft.Json.Converters;

namespace KokoroIO
{
    [Serializable, DataContract]
    public class Device
    {
        /// <summary>
        /// デバイス名を取得または設定します。
        /// </summary>
        [DefaultValue(null)]
        [DataMember, JsonProperty("name")]
        public string Name { get; set; }

        /// <summary>
        /// デバイス種別を取得または設定します。
        /// </summary>
        [DefaultValue(default(DeviceKind))]
        [DataMember, JsonProperty("kind")]
        [JsonConverter(typeof(StringEnumConverter))]
        public DeviceKind Kind { get; set; }

        /// <summary>
        /// デバイスを特定するための任意の一意の文字列を取得または設定します。
        /// </summary>
        [DefaultValue(null)]
        [DataMember, JsonProperty("device_identifier")]
        public string DeviceIdentifier { get; set; }

        /// <summary>
        /// プッシュ通知を送るためのidentifierを取得または設定します。
        /// </summary>
        [DefaultValue(null)]
        [DataMember, JsonProperty("notification_identifier")]
        public string NotificationIdentifier { get; set; }

        /// <summary>
        /// プッシュ通知を受け取りたいかどうかを示す値を取得または設定します。
        /// </summary>
        [DefaultValue(false)]
        [DataMember, JsonProperty("subscribe_notification")]
        public bool SubscribeNotification { get; set; }

        /// <summary>
        /// 最後にデバイスを使用した日時を取得または設定します。
        /// </summary>
        [DefaultValue(null)]
        [DataMember, JsonProperty("last_activity_at")]
        public DateTime? LastActivityAt { get; set; }

        /// <summary>
        /// プッシュ通知サービスに登録されているかどうかを示す値を取得または設定します。
        /// </summary>
        [DefaultValue(false)]
        [DataMember, JsonProperty("push_registered")]
        public bool PushRegistered { get; set; }

        /// <summary>
        /// デバイス用アクセストークンを取得または設定します。
        /// </summary>
        [DefaultValue(null)]
        [DataMember, JsonProperty("access_token")]
        public AccessToken AccessToken { get; set; }
    }
}