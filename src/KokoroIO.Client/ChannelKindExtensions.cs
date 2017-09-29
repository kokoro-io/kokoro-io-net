namespace KokoroIO
{
    public static class ChannelKindExtensions
    {
        internal const string PUBLIC_CHANNEL = "public_channel";
        internal const string PRIVATE_CHANNEL = "private_channel";
        internal const string DIRECT_MESSAGE = "direct_message";

        public static string ToApiString(this ChannelKind value)
        {
            switch (value)
            {
                case ChannelKind.PublicChannel:
                    return PUBLIC_CHANNEL;

                case ChannelKind.PrivateChannel:
                    return PRIVATE_CHANNEL;

                case ChannelKind.DirectMessage:
                    return DIRECT_MESSAGE;
            }

            return value.ToString("G").ToLower();
        }
    }
}