using System.Linq;
using System.Threading.Tasks;
using Xunit;

namespace KokoroIO
{
    public class BotClientTest : TestBase
    {
        [Fact]
        public async Task PostMessageAsyncTest()
        {
            Room dev;
            using (var c = GetClient())
            {
                var memberships = await c.GetMembershipsAsync();

                dev = memberships
                            .Select(ms => ms.Room)
                            .FirstOrDefault(r => r.Kind == RoomKind.PrivateChannel
                                                && !r.IsArchived
                                                && r.ChannelName == "private/dev");

                if (dev == null)
                {
                    return;
                }
            }

            using (var c = GetBotClient())
            {
                var m = await c.PostMessageAsync(dev.Id, GetTestMessage(), displayName: GetType().FullName);

                Assert.NotNull(m);
            }
        }
    }
}