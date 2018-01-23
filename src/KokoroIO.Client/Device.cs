using System;
using System.ComponentModel;
using System.Runtime.Serialization;
using Newtonsoft.Json;
using Newtonsoft.Json.Converters;

namespace KokoroIO
{
    [Serializable, DataContract]
    public class Device
    {
        /// <summary>
        /// �f�o�C�X�����擾�܂��͐ݒ肵�܂��B
        /// </summary>
        [DefaultValue(null)]
        [DataMember, JsonProperty("name")]
        public string Name { get; set; }

        /// <summary>
        /// �f�o�C�X��ʂ��擾�܂��͐ݒ肵�܂��B
        /// </summary>
        [DefaultValue(default(DeviceKind))]
        [DataMember, JsonProperty("kind")]
        [JsonConverter(typeof(StringEnumConverter))]
        public DeviceKind Kind { get; set; }

        /// <summary>
        /// �f�o�C�X����肷�邽�߂̔C�ӂ̈�ӂ̕�������擾�܂��͐ݒ肵�܂��B
        /// </summary>
        [DefaultValue(null)]
        [DataMember, JsonProperty("device_identifier")]
        public string DeviceIdentifier { get; set; }

        /// <summary>
        /// �v�b�V���ʒm�𑗂邽�߂�identifier���擾�܂��͐ݒ肵�܂��B
        /// </summary>
        [DefaultValue(null)]
        [DataMember, JsonProperty("notification_identifier")]
        public string NotificationIdentifier { get; set; }

        /// <summary>
        /// �v�b�V���ʒm���󂯎�肽�����ǂ����������l���擾�܂��͐ݒ肵�܂��B
        /// </summary>
        [DefaultValue(false)]
        [DataMember, JsonProperty("subscribe_notification")]
        public bool SubscribeNotification { get; set; }

        /// <summary>
        /// �Ō�Ƀf�o�C�X���g�p�����������擾�܂��͐ݒ肵�܂��B
        /// </summary>
        [DefaultValue(null)]
        [DataMember, JsonProperty("last_activity_at")]
        public DateTime? LastActivityAt { get; set; }

        /// <summary>
        /// �v�b�V���ʒm�T�[�r�X�ɓo�^����Ă��邩�ǂ����������l���擾�܂��͐ݒ肵�܂��B
        /// </summary>
        [DefaultValue(false)]
        [DataMember, JsonProperty("push_registered")]
        public bool PushRegistered { get; set; }

        /// <summary>
        /// �f�o�C�X�p�A�N�Z�X�g�[�N�����擾�܂��͐ݒ肵�܂��B
        /// </summary>
        [DefaultValue(null)]
        [DataMember, JsonProperty("access_token")]
        public AccessToken AccessToken { get; set; }
    }
}