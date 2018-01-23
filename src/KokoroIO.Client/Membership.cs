using System;
using System.ComponentModel;
using System.Runtime.Serialization;
using Newtonsoft.Json;
using Newtonsoft.Json.Converters;

namespace KokoroIO
{
    [Serializable, DataContract]
    public class Membership
    {
        /// <summary>
        /// �����o�[�V�b�vID���擾�܂��͐ݒ肵�܂��B
        /// </summary>
        [DefaultValue(null)]
        [DataMember, JsonProperty("id")]
        public string Id { get; set; }

        /// <summary>
        /// �`�����l�������擾�܂��͐ݒ肵�܂��B
        /// </summary>
        [DefaultValue(null)]
        [DataMember, JsonProperty("channel")]
        public Channel Channel { get; set; }

        /// <summary>
        /// �������擾�܂��͐ݒ肵�܂��B
        /// </summary>
        [DefaultValue(default(Authority))]
        [DataMember, JsonProperty("authority")]
        [JsonConverter(typeof(StringEnumConverter))]
        public Authority Authority { get; set; }

        /// <summary>
        /// �ʒm���I�t�ɂ��Ă��邩�ǂ����������l���擾�܂��͐ݒ肵�܂��B
        /// </summary>
        [DefaultValue(false)]
        [DataMember, JsonProperty("disable_notification")]
        [Obsolete]
        public bool DisableNotification { get; set; }

        /// <summary>
        /// �ʒm�|���V�[���擾�܂��͐ݒ肵�܂��B
        /// </summary>
        [DefaultValue(default(NotificationPolicy))]
        [DataMember, JsonProperty("notification_policy")]
        [JsonConverter(typeof(StringEnumConverter))]
        public NotificationPolicy NotificationPolicy { get; set; }

        /// <summary>
        /// ���ǃ��b�Z�[�W�\���|���V�[���擾�܂��͐ݒ肵�܂��B
        /// </summary>
        [DefaultValue(default(Authority))]
        [DataMember, JsonProperty("read_state_tracking_policy")]
        [JsonConverter(typeof(StringEnumConverter))]
        public ReadStateTrackingPolicy ReadStateTrackingPolicy { get; set; }

        /// <summary>
        /// ��ԐV�������ǃ��b�Z�[�W���擾�܂��͐ݒ肵�܂��B
        /// </summary>
        [DefaultValue(null)]
        [DataMember, JsonProperty("latest_read_message_id")]
        public int? LatestReadMessageId { get; set; }

        /// <summary>
        /// ���ǐ����擾�܂��͐ݒ肵�܂��B
        /// </summary>
        [DefaultValue(0)]
        [DataMember, JsonProperty("unread_count")]
        public int UnreadCount { get; set; }

        /// <summary>
        /// �`���b�g��ʂ̃`�����l���ꗗ�y�C���ɕ\�����邩�ǂ����������l���擾�܂��͐ݒ肵�܂��B
        /// </summary>
        [DefaultValue(false)]
        [DataMember, JsonProperty("visible")]
        public bool Visible { get; set; }

        /// <summary>
        /// �`���b�g��ʂ̃`�����l���ꗗ�y�C���ɂĖ��ǐ��\�����I�t�ɂ��A�\���𔖂����邩�ǂ��������l���擾�܂��͐ݒ肵�܂��B
        /// </summary>
        [DefaultValue(false)]
        [DataMember, JsonProperty("muted")]
        public bool Muted { get; set; }

        /// <summary>
        /// �v���t�B�[�������擾�܂��͐ݒ肵�܂��B
        /// </summary>
        [DefaultValue(null)]
        [DataMember, JsonProperty("profile")]
        public Profile Profile { get; set; }
    }
}