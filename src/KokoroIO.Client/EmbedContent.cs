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
        /// �Ώۂ�URL���擾�܂��͐ݒ肵�܂��B
        /// </summary>
        [DefaultValue(null)]
        [DataMember, JsonProperty("url")]
        public string Url { get; set; }

        /// <summary>
        /// ���я����擾�܂��͐ݒ肵�܂��B
        /// </summary>
        [DefaultValue(0)]
        [DataMember, JsonProperty("position")]
        public int Position { get; set; }

        /// <summary>
        /// ���^�f�[�^���擾�܂��͐ݒ肵�܂��B
        /// </summary>
        [DefaultValue(null)]
        [DataMember, JsonProperty("data")]
        public EmbedData Data { get; set; }
    }
}