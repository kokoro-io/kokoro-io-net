namespace KokoroIO
{
    public static class RoomKindExtensions
    {
        internal const string PUBLIC_CHANNEL = "public_channel";
        internal const string PRIVATE_CHANNEL = "private_channel";
        internal const string DIRECT_MESSAGE = "direct_message";

        public static string ToApiString(this RoomKind value)
        {
            switch (value)
            {
                case RoomKind.PublicChannel:
                    return PUBLIC_CHANNEL;

                case RoomKind.PrivateChannel:
                    return PRIVATE_CHANNEL;

                case RoomKind.DirectMessage:
                    return DIRECT_MESSAGE;
            }

            return value.ToString("G").ToLower();
        }
    }
}