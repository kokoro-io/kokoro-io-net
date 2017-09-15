using System.Runtime.Serialization;

namespace Shipwreck.KokoroIO
{
    using static DeviceKindExtensions;

    [DataContract]
    public enum DeviceKind
    {
        [EnumMember(Value = UNKNOWN)]
        Unknown,

        [EnumMember(Value = IOS)]
        Ios,

        [EnumMember(Value = ANDROID)]
        Android,

        [EnumMember(Value = UWP)]
        Uwp,

        [EnumMember(Value = CHROME)]
        Chrome,

        [EnumMember(Value = FIREFOX)]
        Firefox
    }
}