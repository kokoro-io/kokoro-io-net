using System.Runtime.Serialization;

namespace Shipwreck.KokoroIO
{
    [DataContract]
    public enum ProfileType
    {
        [EnumMember(Value = "user")]
        User,

        [EnumMember(Value = "bot")]
        Bot,
    }
}