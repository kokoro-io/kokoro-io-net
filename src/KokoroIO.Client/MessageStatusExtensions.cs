namespace KokoroIO
{
    public static class MessageStatusExtensions
    {
        internal const string ACTIVE = "active";
        internal const string DELETED_BY_PUBLISHER = "deleted_by_publisher";
        internal const string DELETED_BY_ANOTHER_MEMBER = "deleted_by_another_member";

        public static string ToApiString(this MessageStatus value)
        {
            switch (value)
            {
                case MessageStatus.Active:
                    return ACTIVE;

                case MessageStatus.DeletedByPublisher:
                    return DELETED_BY_PUBLISHER;

                case MessageStatus.DeletedByAnotherMember:
                    return DELETED_BY_ANOTHER_MEMBER;
            }

            return value.ToString("G").ToLower();
        }
    }
}