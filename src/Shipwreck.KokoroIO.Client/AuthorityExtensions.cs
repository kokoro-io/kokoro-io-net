namespace Shipwreck.KokoroIO
{
    public static class AuthorityExtensions
    {
        internal const string ADMINISTRATOR = "administrator";
        internal const string MAINTAINER = "maintainer";
        internal const string MEMBER = "member";
        internal const string INVITED = "invited";

        public static string ToApiString(this Authority value)
        {
            switch (value)
            {
                case Authority.Administrator:
                    return ADMINISTRATOR;

                case Authority.Maintainer:
                    return MAINTAINER;

                case Authority.Member:
                    return MEMBER;

                case Authority.Invited:
                    return INVITED;
            }

            return value.ToString("G").ToLower();
        }
    }
}