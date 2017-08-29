using System.Linq;
using Xunit;

namespace Shipwreck.KokoroIO
{
    public class BotClientTest : TestBase
    {
        [Fact]
        public void PostMessageAsyncTest()
        {
            Room dev;
            using (var c = GetClient())
            {
                var rooms = c.GetPrivateRoomsAsync().GetAwaiter().GetResult();

                dev = rooms.FirstOrDefault(r => r.ChannelName == "private/dev");

                if (dev == null)
                {
                    return;
                }
            }

            using (var c = GetBotClient())
            {
                var m = c.PostMessageAsync(dev.Id, GetTestMessage(), displayName: GetType().FullName).GetAwaiter().GetResult();

                Assert.NotNull(m);
            }
        }
    }
}