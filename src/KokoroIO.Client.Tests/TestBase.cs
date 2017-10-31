using System;
using System.Linq;
using System.Runtime.CompilerServices;
using System.Threading.Tasks;
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

        protected string OtherUserId => Configuration["OtherUserId"] ?? Environment.GetEnvironmentVariable("OTHER_USER_ID");

        private string _TestChannelId;

        protected string TestChannelId
        {
            get
            {
                if (_TestChannelId == null)
                {
                    using (var c = GetClient())
                    {
                        var mss = c.GetMembershipsAsync().GetAwaiter().GetResult();
                        _TestChannelId = mss.Single(m => m.Channel.ChannelName == "private/unit-test").Channel.Id;
                    }
                }
                return _TestChannelId;
            }
        }

        protected async Task<Channel> GetTestChannelAsync()
        {
            using (var c = GetClient())
            {
                return await GetTestChannelAsync(c).ConfigureAwait(false);
            }
        }

        protected Task<Channel> GetTestChannelAsync(Client client)
            => client.GetChannelAsync(TestChannelId);

        public string GetTestMessage([CallerMemberName] string memberName = null)
            => $"{GetType().FullName}#{memberName} posted this message from {Environment.MachineName} at {DateTime.Now:u}";
    }
}