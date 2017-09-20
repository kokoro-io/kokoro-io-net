using System.Runtime.Serialization;

namespace KokoroIO
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