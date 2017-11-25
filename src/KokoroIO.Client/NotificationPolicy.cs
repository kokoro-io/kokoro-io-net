using System.Runtime.Serialization;

namespace KokoroIO
{
    [DataContract]
    public enum NotificationPolicy
    {
        [EnumMember(Value = NotificationPolicyExtensions.ALL_MESSAGES)]
        AllMessages,

        [EnumMember(Value = NotificationPolicyExtensions.ONLY_MENTIONS)]
        OnlyMentions,

        [EnumMember(Value = NotificationPolicyExtensions.NOTHING)]
        Nothing
    }
}