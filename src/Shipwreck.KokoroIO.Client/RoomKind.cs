using System.Runtime.Serialization;

namespace Shipwreck.KokoroIO
{
    [DataContract]
    public enum RoomKind
    {
        [EnumMember(Value = "public_channel")]
        PublicChannel,

        [EnumMember(Value = "private_channel")]
        PrivateChannel,

        [EnumMember(Value = "direct_message")]
        DirectMessage
    }
}