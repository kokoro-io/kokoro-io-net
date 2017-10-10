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
        {
            var c = new Client()
            {
                AccessToken = Configuration["UserAccessToken"] ?? Environment.GetEnvironmentVariable("USER_ACCESS_TOKEN")
            };

            var ep = Environment.GetEnvironmentVariable("END_POINT");
            if (!string.IsNullOrEmpty(ep))
            {
                c.EndPoint = ep;
            }

            var wsep = Environment.GetEnvironmentVariable("WEB_SOCKET_END_POINT");
            if (!string.IsNullOrEmpty(wsep))
            {
                c.WebSocketEndPoint = wsep;
            }

            return c;
        }

        public static BotClient GetBotClient()
        {
            var c = new BotClient()
            {
                AccessToken = Configuration["BotAccessToken"] ?? Environment.GetEnvironmentVariable("BOT_ACCESS_TOKEN")
            };

            var ep = Environment.GetEnvironmentVariable("END_POINT");
            if (!string.IsNullOrEmpty(ep))
            {
                c.EndPoint = ep;
            }

            return c;
        }

        public string GetTestMessage([CallerMemberName] string memberName = null)
            => $"{GetType().FullName}#{memberName} posted this message from {Environment.MachineName} at {DateTime.Now:u}";
    }
}