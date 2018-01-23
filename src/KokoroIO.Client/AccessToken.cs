using System;
using System.ComponentModel;
using System.Runtime.Serialization;
using Newtonsoft.Json;
using Newtonsoft.Json.Converters;

namespace KokoroIO
{
    [Serializable, DataContract]
    public class AccessToken
    {
        /// <summary>
        /// アクセストークンIDを取得または設定します。
        /// </summary>
        [DefaultValue(null)]
        [DataMember, JsonProperty("id")]
        public string Id { get; set; }

        /// <summary>
        /// アクセストークン名を取得または設定します。
        /// </summary>
        [DefaultValue(null)]
        [DataMember, JsonProperty("name")]
        public string Name { get; set; }

        /// <summary>
        /// アクセストークンを取得または設定します。
        /// </summary>
        [DefaultValue(null)]
        [DataMember, JsonProperty("token")]
        public string Token { get; set; }

        /// <summary>
        /// アクセストークン種別を取得または設定します。
        /// </summary>
        [DefaultValue(default(AccessTokenKind))]
        [DataMember, JsonProperty("kind")]
        [JsonConverter(typeof(StringEnumConverter))]
        public AccessTokenKind Kind { get; set; }
    }
}