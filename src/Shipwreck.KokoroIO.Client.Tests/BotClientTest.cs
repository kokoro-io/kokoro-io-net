using System.Linq;
using Xunit;

namespace Shipwreck.KokoroIO
{
    public class BotClientTest
    {
        [Fact]
        public void PostMessageAsyncTest()
        {
            Room dev;
            using (var c = TestHelper.GetClient())
            {
                var rooms = c.GetPrivateRoomsAsync().GetAwaiter().GetResult();

                dev = rooms.FirstOrDefault(r => r.ChannelName == "private/dev");

                if (dev == null)
                {
                    return;
                }
            }

            using (var c = TestHelper.GetBotClient())
            {
                var m = c.PostMessageAsync(dev.Id, "test", false).GetAwaiter().GetResult();

                Assert.NotNull(m);
            }
        }
    }
}