using System.Runtime.Serialization;

namespace KokoroIO
{
    [DataContract]
    public enum AccessTokenKind
    {
        [EnumMember(Value = AccessTokenKindExtensions.USER)]
        User,

        [EnumMember(Value = AccessTokenKindExtensions.DEVICE)]
        Device,

        [EnumMember(Value = AccessTokenKindExtensions.ESSENTIAL)]
        Essential,
    }
}