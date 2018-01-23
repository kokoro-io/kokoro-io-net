using System;
using System.ComponentModel;
using System.Runtime.Serialization;
using Newtonsoft.Json;

namespace KokoroIO
{
    [Serializable, DataContract]
    public class Avatar
    {
        /// <summary>
        /// 正方形画像の縦横サイズを取得または設定します。
        /// </summary>
        [DefaultValue(0)]
        [DataMember, JsonProperty("size")]
        public int Size { get; set; }

        /// <summary>
        /// 画像のURLを取得または設定します。
        /// </summary>
        [DefaultValue(null)]
        [DataMember, JsonProperty("url")]
        public string Url { get; set; }

        /// <summary>
        /// デフォルトアバターであるかどうか示す値を取得または設定します。
        /// </summary>
        [DefaultValue(false)]
        [DataMember, JsonProperty("is_default")]
        public bool IsDefault { get; set; }
    }
}