using Microsoft.Extensions.Configuration;

namespace Shipwreck.KokoroIO
{
    internal class TestHelper
    {
        private static IConfigurationRoot _Configuration;

        public static IConfigurationRoot Configuration
            => _Configuration ?? (_Configuration = new ConfigurationBuilder().AddUserSecrets<TestHelper>().Build());

        public static Client GetClient()
            => new Client()
            {
                AccessToken = Configuration["UserAccessToken"]
            };
        public static BotClient GetBotClient()
            => new BotClient()
            {
                AccessToken = Configuration["BotAccessToken"]
            };
    }
}