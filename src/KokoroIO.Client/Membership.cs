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
        /// <summary>
        /// メンバーシップIDを取得または設定します。
        /// </summary>
        [DefaultValue(null)]
        [DataMember, JsonProperty("id")]
        public string Id { get; set; }

        /// <summary>
        /// チャンネル情報を取得または設定します。
        /// </summary>
        [DefaultValue(null)]
        [DataMember, JsonProperty("channel")]
        public Channel Channel { get; set; }

        /// <summary>
        /// 権限を取得または設定します。
        /// </summary>
        [DefaultValue(default(Authority))]
        [DataMember, JsonProperty("authority")]
        [JsonConverter(typeof(StringEnumConverter))]
        public Authority Authority { get; set; }

        /// <summary>
        /// 通知をオフにしているかどうかを示す値を取得または設定します。
        /// </summary>
        [DefaultValue(false)]
        [DataMember, JsonProperty("disable_notification")]
        [Obsolete]
        public bool DisableNotification { get; set; }

        /// <summary>
        /// 通知ポリシーを取得または設定します。
        /// </summary>
        [DefaultValue(default(NotificationPolicy))]
        [DataMember, JsonProperty("notification_policy")]
        [JsonConverter(typeof(StringEnumConverter))]
        public NotificationPolicy NotificationPolicy { get; set; }

        /// <summary>
        /// 未読メッセージ表示ポリシーを取得または設定します。
        /// </summary>
        [DefaultValue(default(Authority))]
        [DataMember, JsonProperty("read_state_tracking_policy")]
        [JsonConverter(typeof(StringEnumConverter))]
        public ReadStateTrackingPolicy ReadStateTrackingPolicy { get; set; }

        /// <summary>
        /// 一番新しい既読メッセージを取得または設定します。
        /// </summary>
        [DefaultValue(null)]
        [DataMember, JsonProperty("latest_read_message_id")]
        public int? LatestReadMessageId { get; set; }

        /// <summary>
        /// 未読数を取得または設定します。
        /// </summary>
        [DefaultValue(0)]
        [DataMember, JsonProperty("unread_count")]
        public int UnreadCount { get; set; }

        /// <summary>
        /// チャット画面のチャンネル一覧ペインに表示するかどうかを示す値を取得または設定します。
        /// </summary>
        [DefaultValue(false)]
        [DataMember, JsonProperty("visible")]
        public bool Visible { get; set; }

        /// <summary>
        /// チャット画面のチャンネル一覧ペインにて未読数表示をオフにし、表示を薄くするかどうか示す値を取得または設定します。
        /// </summary>
        [DefaultValue(false)]
        [DataMember, JsonProperty("muted")]
        public bool Muted { get; set; }

        /// <summary>
        /// プロフィール情報を取得または設定します。
        /// </summary>
        [DefaultValue(null)]
        [DataMember, JsonProperty("profile")]
        public Profile Profile { get; set; }
    }
}