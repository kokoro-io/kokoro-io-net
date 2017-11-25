namespace KokoroIO
{
    public static class NotificationPolicyExtensions
    {
        internal const string ALL_MESSAGES = "all_messages";
        internal const string ONLY_MENTIONS = "only_mentions";
        internal const string NOTHING = "nothing";

        public static string ToApiString(this NotificationPolicy value)
        {
            switch (value)
            {
                case NotificationPolicy.AllMessages:
                    return ALL_MESSAGES;

                case NotificationPolicy.OnlyMentions:
                    return ONLY_MENTIONS;

                case NotificationPolicy.Nothing:
                    return NOTHING;
            }

            return value.ToString("G").ToLower();
        }
    }
}