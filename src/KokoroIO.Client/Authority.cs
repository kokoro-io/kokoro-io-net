using System.Runtime.Serialization;

namespace KokoroIO
{
    [DataContract]
    public enum Authority
    {
        [EnumMember(Value = AuthorityExtensions.ADMINISTRATOR)]
        Administrator,

        [EnumMember(Value = AuthorityExtensions.MAINTAINER)]
        Maintainer,

        [EnumMember(Value = AuthorityExtensions.MEMBER)]
        Member,

        [EnumMember(Value = AuthorityExtensions.INVITED)]
        Invited
    }
}