using System;
using System.ComponentModel;
using System.Runtime.Serialization;
using Newtonsoft.Json;
using Newtonsoft.Json.Converters;

namespace KokoroIO
{
    [Serializable, DataContract]
    public class Channel
    {
        /// <summary>
        /// �`�����l��ID���擾�܂��͐ݒ肵�܂��B
        /// </summary>
        [DefaultValue(null)]
        [DataMember, JsonProperty("id")]
        public string Id { get; set; }

        /// <summary>
        /// �`�����l�������擾�܂��͐ݒ肵�܂��B
        /// </summary>
        [DefaultValue(null)]
        [DataMember, JsonProperty("channel_name")]
        public string ChannelName { get; set; }

        /// <summary>
        /// �`�����l���^�C�v���擾�܂��͐ݒ肵�܂��B
        /// </summary>
        [DefaultValue(default(ChannelKind))]
        [DataMember, JsonProperty("kind")]
        [JsonConverter(typeof(StringEnumConverter))]
        public ChannelKind Kind { get; set; }

        /// <summary>
        /// �A�[�J�C�u�ς��ǂ����������l���擾�܂��͐ݒ肵�܂��B
        /// </summary>
        [DefaultValue(false)]
        [DataMember, JsonProperty("archived")]
        public bool IsArchived { get; set; }

        /// <summary>
        /// �`�����l���������擾�܂��͐ݒ肵�܂��B
        /// </summary>
        [DefaultValue(null)]
        [DataMember, JsonProperty("description")]
        public string Description { get; set; }

        /// <summary>
        /// �ŐV���b�Z�[�W��ID���擾�܂��͐ݒ肵�܂��B
        /// </summary>
        [DefaultValue(null)]
        [DataMember, JsonProperty("latest_message_id")]
        public int? LatestMessageId { get; set; }

        /// <summary>
        /// �ŐV���b�Z�[�W���e�������擾�܂��͐ݒ肵�܂��B
        /// </summary>
        [DefaultValue(null)]
        [DataMember, JsonProperty("latest_message_published_at")]
        public DateTime? LatestMessagePublishedAt { get; set; }

        /// <summary>
        /// ���b�Z�[�W�����擾�܂��͐ݒ肵�܂��B
        /// </summary>
        [DefaultValue(0)]
        [DataMember, JsonProperty("messages_count")]
        public int MessagesCount { get; set; }

        /// <summary>
        /// �����o�[�V�b�v�����擾�܂��͐ݒ肵�܂��B
        /// </summary>
        [DefaultValue(null)]
        [DataMember, JsonProperty("membership")]
        public Membership Membership { get; set; }

        [DefaultValue(null)]
        [DataMember, JsonProperty("memberships")]
        public Membership[] Memberships { get; set; }

        public static bool IsValidId(string id)
        {
            if (string.IsNullOrEmpty(id))
            {
                return false;
            }
            foreach (var c in id)
            {
                if (!(('0' <= c && c <= '9') || 'A' <= c && c <= 'Z' || 'a' <= c && c <= 'z'))
                {
                    return false;
                }
            }
            return true;
        }
    }
}