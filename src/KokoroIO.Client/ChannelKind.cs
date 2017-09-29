using System.Runtime.Serialization;

namespace KokoroIO
{
    [DataContract]
    public enum ChannelKind
    {
        [EnumMember(Value = ChannelKindExtensions.PUBLIC_CHANNEL)]
        PublicChannel,

        [EnumMember(Value = ChannelKindExtensions.PRIVATE_CHANNEL)]
        PrivateChannel,

        [EnumMember(Value = ChannelKindExtensions.DIRECT_MESSAGE)]
        DirectMessage
    }

}