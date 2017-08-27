using System.Runtime.Serialization;

namespace Shipwreck.KokoroIO
{
    public enum ProfileType
    {
        [EnumMember(Value = "user")]
        User,

        [EnumMember(Value = "bot")]
        Bot,
    }
}