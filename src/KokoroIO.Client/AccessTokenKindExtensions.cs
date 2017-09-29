namespace KokoroIO
{
    public static class AccessTokenKindExtensions
    {
        internal const string USER = "user";
        internal const string DEVICE = "device";
        internal const string ESSENTIAL = "essential";

        public static string ToApiString(this AccessTokenKind value)
        {
            switch (value)
            {
                case AccessTokenKind.User:
                    return USER;

                case AccessTokenKind.Device:
                    return DEVICE;

                case AccessTokenKind.Essential:
                    return ESSENTIAL;
            }

            return value.ToString("G").ToLower();
        }
    }
}