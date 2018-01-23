using System;
using System.ComponentModel;
using System.Runtime.Serialization;
using Newtonsoft.Json;
using Newtonsoft.Json.Converters;

namespace KokoroIO
{
    [Serializable, DataContract]
    public class Channel
    {
        /// <summary>
        /// チャンネルIDを取得または設定します。
        /// </summary>
        [DefaultValue(null)]
        [DataMember, JsonProperty("id")]
        public string Id { get; set; }

        /// <summary>
        /// チャンネル名を取得または設定します。
        /// </summary>
        [DefaultValue(null)]
        [DataMember, JsonProperty("channel_name")]
        public string ChannelName { get; set; }

        /// <summary>
        /// チャンネルタイプを取得または設定します。
        /// </summary>
        [DefaultValue(default(ChannelKind))]
        [DataMember, JsonProperty("kind")]
        [JsonConverter(typeof(StringEnumConverter))]
        public ChannelKind Kind { get; set; }

        /// <summary>
        /// アーカイブ済かどうかを示す値を取得または設定します。
        /// </summary>
        [DefaultValue(false)]
        [DataMember, JsonProperty("archived")]
        public bool IsArchived { get; set; }

        /// <summary>
        /// チャンネル説明を取得または設定します。
        /// </summary>
        [DefaultValue(null)]
        [DataMember, JsonProperty("description")]
        public string Description { get; set; }

        /// <summary>
        /// 最新メッセージのIDを取得または設定します。
        /// </summary>
        [DefaultValue(null)]
        [DataMember, JsonProperty("latest_message_id")]
        public int? LatestMessageId { get; set; }

        /// <summary>
        /// 最新メッセージ投稿日時を取得または設定します。
        /// </summary>
        [DefaultValue(null)]
        [DataMember, JsonProperty("latest_message_published_at")]
        public DateTime? LatestMessagePublishedAt { get; set; }

        /// <summary>
        /// メッセージ数を取得または設定します。
        /// </summary>
        [DefaultValue(0)]
        [DataMember, JsonProperty("messages_count")]
        public int MessagesCount { get; set; }

        /// <summary>
        /// メンバーシップ情報を取得または設定します。
        /// </summary>
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