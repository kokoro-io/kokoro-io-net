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
        /// �v���t�B�[��ID���擾�܂��͐ݒ肵�܂��B
        /// </summary>
        [DefaultValue(null)]
        [DataMember, JsonProperty("id")]
        public string Id { get; set; }

        /// <summary>
        /// ��ނ��擾�܂��͐ݒ肵�܂��B
        /// </summary>
        [DefaultValue(default(ProfileType))]
        [DataMember, JsonProperty("type")]
        [JsonConverter(typeof(StringEnumConverter))]
        public ProfileType Type { get; set; }

        /// <summary>
        /// �X�N���[���l�[�����擾�܂��͐ݒ肵�܂��B
        /// </summary>
        [DefaultValue(null)]
        [DataMember, JsonProperty("screen_name")]
        public string ScreenName { get; set; }

        /// <summary>
        /// �f�B�X�v���C�l�[�����擾�܂��͐ݒ肵�܂��B
        /// </summary>
        [DefaultValue(null)]
        [DataMember, JsonProperty("display_name")]
        public string DisplayName { get; set; }

        /// <summary>
        /// �A�o�^�[URL���擾�܂��͐ݒ肵�܂��B
        /// </summary>
        [DefaultValue(null)]
        [DataMember, JsonProperty("avatar")]
        public string Avatar { get; set; }

        [DefaultValue(null)]
        [DataMember, JsonProperty("avatars")]
        public Avatar[] Avatars { get; set; }

        /// <summary>
        /// �A�[�J�C�u�ς��ǂ����������l���擾�܂��͐ݒ肵�܂��B
        /// </summary>
        [DefaultValue(false)]
        [DataMember, JsonProperty("archived")]
        public bool IsArchived { get; set; }

        /// <summary>
        /// ���҂���Ă���`�����l���̐����擾�܂��͐ݒ肵�܂��B
        /// </summary>
        [DefaultValue(0)]
        [DataMember, JsonProperty("invited_channels_count")]
        public int InvitedChannelsCount { get; set; }
    }
}