using System.Runtime.Serialization;

namespace KokoroIO
{
    [DataContract]
    public enum MessageStatus
    {
        [EnumMember(Value = MessageStatusExtensions.ACTIVE)]
        Active,

        [EnumMember(Value = MessageStatusExtensions.DELETED_BY_PUBLISHER)]
        DeletedByPublisher,

        [EnumMember(Value = MessageStatusExtensions.DELETED_BY_ANOTHER_MEMBER)]
        DeletedByAnotherMember
    }
}