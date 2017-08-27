using System;
using System.Collections.Generic;
using System.ComponentModel;
using Newtonsoft.Json;

namespace Shipwreck.KokoroIO
{
    [Serializable]
    public class Message
    {
        [DefaultValue(0)]
        [JsonProperty("id")]
        public int Id { get; set; }

        [DefaultValue(typeof(Guid), "00000000-0000-0000-0000-000000000000")]
        [JsonProperty("idempotent_key")]
        public Guid IdempotentKey { get; set; }

        [DefaultValue(null)]
        [JsonProperty("display_name")]
        public string DisplayName { get; set; }

        [DefaultValue(null)]
        [JsonProperty("avatar")]
        public string Avatar { get; set; }

        [DefaultValue(null)]
        [JsonProperty("content")]
        public string Content { get; set; }

        [DefaultValue(null)]
        [JsonProperty("raw_content")]
        public string RawContent { get; set; }

        #region EmbeddedUrls

        private List<string> _EmbeddedUrls;

        [JsonProperty("embedded_urls")]
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

        [JsonProperty("embed_contents")]
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
        [JsonProperty("published_at")]
        public DateTime PublishedAt { get; set; }

        [DefaultValue(false)]
        [JsonProperty("nsfw")]
        public bool IsNsfw { get; set; }

        [DefaultValue(null)]
        [JsonProperty("room")]
        public Room Room { get; set; }

        [DefaultValue(null)]
        [JsonProperty("profile")]
        public Profile Profile { get; set; }
    }
}