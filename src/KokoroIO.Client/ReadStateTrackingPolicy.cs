using System.Runtime.Serialization;

namespace KokoroIO
{
    [DataContract]
    public enum ReadStateTrackingPolicy
    {
        [EnumMember(Value = ReadStateTrackingPolicyExtensions.KEEP_LATEST)]
        KeepLatest,

        [EnumMember(Value = ReadStateTrackingPolicyExtensions.CONSUME_LAST)]
        ConsumeLast,

        [EnumMember(Value = ReadStateTrackingPolicyExtensions.CONSUME_LATEST)]
        ConsumeLatest
    }
}