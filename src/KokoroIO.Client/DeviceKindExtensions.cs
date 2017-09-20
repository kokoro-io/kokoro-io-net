namespace KokoroIO
{
    public static class DeviceKindExtensions
    {
        internal const string UNKNOWN = "unknown";
        internal const string OFFICIAL_WEB = "official_web";
        internal const string IOS = "ios";
        internal const string ANDROID = "android";
        internal const string UWP = "uwp";
        internal const string CHROME = "chrome";
        internal const string FIREFOX = "firefox";

        public static string ToApiString(this DeviceKind value)
        {
            switch (value)
            {
                case DeviceKind.Unknown:
                    return UNKNOWN;

                case DeviceKind.OfficialWeb:
                    return OFFICIAL_WEB;

                case DeviceKind.Ios:
                    return IOS;

                case DeviceKind.Android:
                    return ANDROID;

                case DeviceKind.Uwp:
                    return UWP;

                case DeviceKind.Chrome:
                    return CHROME;

                case DeviceKind.Firefox:
                    return FIREFOX;
            }

            return value.ToString("G").ToLower();
        }
    }
}