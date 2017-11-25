namespace KokoroIO
{
    public static class ReadStateTrackingPolicyExtensions
    {
        internal const string KEEP_LATEST = "keep_latest";
        internal const string CONSUME_LAST = "consume_last";
        internal const string CONSUME_LATEST = "consume_latest";

        public static string ToApiString(this ReadStateTrackingPolicy value)
        {
            switch (value)
            {
                case ReadStateTrackingPolicy.KeepLatest:
                    return KEEP_LATEST;

                case ReadStateTrackingPolicy.ConsumeLast:
                    return CONSUME_LAST;

                case ReadStateTrackingPolicy.ConsumeLatest:
                    return CONSUME_LATEST;
            }

            return value.ToString("G").ToLower();
        }
    }
}