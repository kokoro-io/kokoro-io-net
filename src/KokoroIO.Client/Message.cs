using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Runtime.Serialization;
using Newtonsoft.Json;
using Newtonsoft.Json.Converters;

namespace KokoroIO
{
    [Serializable, DataContract]
    public class Message
    {
        /// <summary>
        /// メッセージIDを取得または設定します。
        /// </summary>
        [DefaultValue(0)]
        [DataMember, JsonProperty("id")]
        public int Id { get; set; }

        /// <summary>
        /// 冪等性IDを取得または設定します。
        /// </summary>
        [DefaultValue(null)]
        [DataMember, JsonProperty("idempotent_key")]
        public Guid? IdempotentKey { get; set; }

        /// <summary>
        /// 発言時の表示名を取得または設定します。
        /// </summary>
        [DefaultValue(null)]
        [DataMember, JsonProperty("display_name")]
        public string DisplayName { get; set; }

        /// <summary>
        /// 発言時のアバターURLを取得または設定します。
        /// </summary>
        [DefaultValue(null)]
        [DataMember, JsonProperty("avatar")]
        public string Avatar { get; set; }

        [DefaultValue(null)]
        [DataMember, JsonProperty("avatars")]
        public Avatar[] Avatars { get; set; }

        /// <summary>
        /// URLが表すコンテンツを展開するかどうかを示す値を取得または設定します。
        /// </summary>
        [DefaultValue(false)]
        [DataMember, JsonProperty("expand_embed_contents")]
        public bool ExpandEmbedContents { get; set; }

        /// <summary>
        /// 発言の状態を取得または設定します。
        /// </summary>
        [DefaultValue(default(MessageStatus))]
        [DataMember, JsonProperty("status")]
        [JsonConverter(typeof(StringEnumConverter))]
        public MessageStatus Status { get; set; }

        #region Content

        /// <summary>
        /// 発言内容を取得または設定します。
        /// </summary>
        [Obsolete("Use " + nameof(HtmlContent) + " instead.")]
        [IgnoreDataMember, JsonProperty("content")]
        public string Content
        {
            get => HtmlContent;
            set => HtmlContent = value;
        }

        public bool ShouldSerializeContent()
            => false;

        /// <summary>
        /// 発言内容を取得または設定します。
        /// </summary>
        [DefaultValue(null)]
        [DataMember, JsonProperty("html_content")]
        public string HtmlContent { get; set; }

        /// <summary>
        /// プレインテキスト形式に変換した発言内容を取得または設定します。
        /// </summary>
        [DefaultValue(null)]
        [DataMember, JsonProperty("plaintext_content")]
        public string PlainTextContent { get; set; }

        /// <summary>
        /// 発言内容を取得または設定します。
        /// </summary>
        [DefaultValue(null)]
        [DataMember, JsonProperty("raw_content")]
        public string RawContent { get; set; }

        #endregion Content

        #region EmbeddedUrls

        private List<string> _EmbeddedUrls;

        /// <summary>
        /// 埋め込みURLの配列を取得または設定します。
        /// </summary>
        [DataMember, JsonProperty("embedded_urls")]
        [DesignerSerializationVisibility(DesignerSerializationVisibility.Content)]
        public IList<string> EmbeddedUrls
        {
            get => CollectionHelper.Get(ref _EmbeddedUrls);
            set => CollectionHelper.Set(ref _EmbeddedUrls, value);
        }

        public bool ShouldSerializeEmbeddedUrls()
            => _EmbeddedUrls?.Count > 0;

        public void ResetEmbeddedUrls()
            => _EmbeddedUrls?.Clear();

        #endregion EmbeddedUrls

        #region EmbedContents

        private List<EmbedContent> _EmbedContents;

        /// <summary>
        /// 埋め込みコンテンツの配列を取得または設定します。
        /// </summary>
        [DataMember, JsonProperty("embed_contents")]
        [DesignerSerializationVisibility(DesignerSerializationVisibility.Content)]
        public IList<EmbedContent> EmbedContents
        {
            get => CollectionHelper.Get(ref _EmbedContents);
            set => CollectionHelper.Set(ref _EmbedContents, value);
        }

        public bool ShouldSerializeEmbedContents()
            => _EmbedContents?.Count > 0;

        public void ResetEmbedContents()
            => _EmbedContents?.Clear();

        #endregion EmbedContents

        /// <summary>
        /// 発言日時を取得または設定します。
        /// </summary>
        [DefaultValue(typeof(DateTime), "0001-01-01T00:00:00")]
        [DataMember, JsonProperty("published_at")]
        public DateTime PublishedAt { get; set; }

        /// <summary>
        /// NSFW(Not suitable for work)かどうかを示す値を取得または設定します。
        /// </summary>
        [DefaultValue(false)]
        [DataMember, JsonProperty("nsfw")]
        public bool IsNsfw { get; set; }

        /// <summary>
        /// 発言があったチャンネルを取得または設定します。
        /// </summary>
        [DefaultValue(null)]
        [DataMember, JsonProperty("channel")]
        public Channel Channel { get; set; }

        /// <summary>
        /// 発言者情報を取得または設定します。
        /// </summary>
        [DefaultValue(null)]
        [DataMember, JsonProperty("profile")]
        public Profile Profile { get; set; }
    }
}