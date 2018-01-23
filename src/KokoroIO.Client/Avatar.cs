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
        /// �����`�摜�̏c���T�C�Y���擾�܂��͐ݒ肵�܂��B
        /// </summary>
        [DefaultValue(0)]
        [DataMember, JsonProperty("size")]
        public int Size { get; set; }

        /// <summary>
        /// �摜��URL���擾�܂��͐ݒ肵�܂��B
        /// </summary>
        [DefaultValue(null)]
        [DataMember, JsonProperty("url")]
        public string Url { get; set; }

        /// <summary>
        /// �f�t�H���g�A�o�^�[�ł��邩�ǂ��������l���擾�܂��͐ݒ肵�܂��B
        /// </summary>
        [DefaultValue(false)]
        [DataMember, JsonProperty("is_default")]
        public bool IsDefault { get; set; }
    }
}