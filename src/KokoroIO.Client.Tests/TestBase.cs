using System;
using System.Runtime.CompilerServices;
using Microsoft.Extensions.Configuration;

namespace KokoroIO
{
    public abstract class TestBase
    {
        private static IConfigurationRoot _Configuration;

        public static IConfigurationRoot Configuration
            => _Configuration ?? (_Configuration = new ConfigurationBuilder().AddUserSecrets<TestBase>().Build());

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

        public string GetTestMessage([CallerMemberName] string memberName = null)
            => $"{GetType().FullName}#{memberName} posted this message from {Environment.MachineName} at {DateTime.Now:u}";
    }
}