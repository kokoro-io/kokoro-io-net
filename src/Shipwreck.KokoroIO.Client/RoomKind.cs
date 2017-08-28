using System.Runtime.Serialization;

namespace Shipwreck.KokoroIO
{
    [DataContract]
    public enum RoomKind
    {
        [EnumMember(Value = RoomKindExtensions.PUBLIC_CHANNEL)]
        PublicChannel,

        [EnumMember(Value = RoomKindExtensions.PRIVATE_CHANNEL)]
        PrivateChannel,

        [EnumMember(Value = RoomKindExtensions.DIRECT_MESSAGE)]
        DirectMessage
    }

}