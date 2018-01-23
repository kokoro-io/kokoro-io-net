using System;
using System.ComponentModel;
using System.Runtime.Serialization;
using Newtonsoft.Json;

namespace KokoroIO
{
    [Serializable, DataContract]
    public class EmbedContent
    {
        /// <summary>
        /// 対象のURLを取得または設定します。
        /// </summary>
        [DefaultValue(null)]
        [DataMember, JsonProperty("url")]
        public string Url { get; set; }

        /// <summary>
        /// 並び順を取得または設定します。
        /// </summary>
        [DefaultValue(0)]
        [DataMember, JsonProperty("position")]
        public int Position { get; set; }

        /// <summary>
        /// メタデータを取得または設定します。
        /// </summary>
        [DefaultValue(null)]
        [DataMember, JsonProperty("data")]
        public EmbedData Data { get; set; }
    }
}