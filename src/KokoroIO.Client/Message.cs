﻿using System;
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
        [DefaultValue(0)]
        [DataMember, JsonProperty("id")]
        public int Id { get; set; }

        [DefaultValue(null)]
        [DataMember, JsonProperty("idempotent_key")]
        public Guid? IdempotentKey { get; set; }

        [DefaultValue(null)]
        [DataMember, JsonProperty("display_name")]
        public string DisplayName { get; set; }

        [DefaultValue(null)]
        [DataMember, JsonProperty("avatar")]
        public string Avatar { get; set; }

        [DefaultValue(null)]
        [DataMember, JsonProperty("avatars")]
        public Avatar[] Avatars { get; set; }

        [DefaultValue(false)]
        [DataMember, JsonProperty("expand_embed_contents")]
        public bool ExpandEmbedContents { get; set; }

        [DefaultValue(default(MessageStatus))]
        [DataMember, JsonProperty("status")]
        [JsonConverter(typeof(StringEnumConverter))]
        public MessageStatus Status { get; set; }

        #region Content

        [Obsolete("Use " + nameof(HtmlContent) + " instead.")]
        [IgnoreDataMember, JsonProperty("content")]
        public string Content
        {
            get => HtmlContent;
            set => HtmlContent = value;
        }

        public bool ShouldSerializeContent()
            => false;

        [DefaultValue(null)]
        [DataMember, JsonProperty("html_content")]
        public string HtmlContent { get; set; }

        [DefaultValue(null)]
        [DataMember, JsonProperty("plaintext_content")]
        public string PlainTextContent { get; set; }

        [DefaultValue(null)]
        [DataMember, JsonProperty("raw_content")]
        public string RawContent { get; set; }

        #endregion Content

        #region EmbeddedUrls

        private List<string> _EmbeddedUrls;

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

        [DefaultValue(typeof(DateTime), "0001-01-01T00:00:00")]
        [DataMember, JsonProperty("published_at")]
        public DateTime PublishedAt { get; set; }

        [DefaultValue(false)]
        [DataMember, JsonProperty("nsfw")]
        public bool IsNsfw { get; set; }

        [DefaultValue(null)]
        [DataMember, JsonProperty("channel")]
        public Channel Channel { get; set; }

        [DefaultValue(null)]
        [DataMember, JsonProperty("profile")]
        public Profile Profile { get; set; }
    }
}