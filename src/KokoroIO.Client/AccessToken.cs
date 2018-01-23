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
        /// �A�N�Z�X�g�[�N��ID���擾�܂��͐ݒ肵�܂��B
        /// </summary>
        [DefaultValue(null)]
        [DataMember, JsonProperty("id")]
        public string Id { get; set; }

        /// <summary>
        /// �A�N�Z�X�g�[�N�������擾�܂��͐ݒ肵�܂��B
        /// </summary>
        [DefaultValue(null)]
        [DataMember, JsonProperty("name")]
        public string Name { get; set; }

        /// <summary>
        /// �A�N�Z�X�g�[�N�����擾�܂��͐ݒ肵�܂��B
        /// </summary>
        [DefaultValue(null)]
        [DataMember, JsonProperty("token")]
        public string Token { get; set; }

        /// <summary>
        /// �A�N�Z�X�g�[�N����ʂ��擾�܂��͐ݒ肵�܂��B
        /// </summary>
        [DefaultValue(default(AccessTokenKind))]
        [DataMember, JsonProperty("kind")]
        [JsonConverter(typeof(StringEnumConverter))]
        public AccessTokenKind Kind { get; set; }
    }
}