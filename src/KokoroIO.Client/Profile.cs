using System;
using System.ComponentModel;
using System.Runtime.Serialization;
using Newtonsoft.Json;
using Newtonsoft.Json.Converters;

namespace KokoroIO
{
    [Serializable, DataContract]
    public class Profile
    {
        /// <summary>
        /// プロフィールIDを取得または設定します。
        /// </summary>
        [DefaultValue(null)]
        [DataMember, JsonProperty("id")]
        public string Id { get; set; }

        /// <summary>
        /// 種類を取得または設定します。
        /// </summary>
        [DefaultValue(default(ProfileType))]
        [DataMember, JsonProperty("type")]
        [JsonConverter(typeof(StringEnumConverter))]
        public ProfileType Type { get; set; }

        /// <summary>
        /// スクリーンネームを取得または設定します。
        /// </summary>
        [DefaultValue(null)]
        [DataMember, JsonProperty("screen_name")]
        public string ScreenName { get; set; }

        /// <summary>
        /// ディスプレイネームを取得または設定します。
        /// </summary>
        [DefaultValue(null)]
        [DataMember, JsonProperty("display_name")]
        public string DisplayName { get; set; }

        /// <summary>
        /// アバターURLを取得または設定します。
        /// </summary>
        [DefaultValue(null)]
        [DataMember, JsonProperty("avatar")]
        public string Avatar { get; set; }

        [DefaultValue(null)]
        [DataMember, JsonProperty("avatars")]
        public Avatar[] Avatars { get; set; }

        /// <summary>
        /// アーカイブ済かどうかを示す値を取得または設定します。
        /// </summary>
        [DefaultValue(false)]
        [DataMember, JsonProperty("archived")]
        public bool IsArchived { get; set; }

        /// <summary>
        /// 招待されているチャンネルの数を取得または設定します。
        /// </summary>
        [DefaultValue(0)]
        [DataMember, JsonProperty("invited_channels_count")]
        public int InvitedChannelsCount { get; set; }
    }
}