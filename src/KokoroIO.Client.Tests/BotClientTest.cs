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
            var dev = await GetTestChannelAsync();

            using (var c = GetBotClient())
            {
                var m = await c.PostMessageAsync(dev.Id, GetTestMessage(), displayName: GetType().FullName, expandEmbedContents: true);

                Assert.NotNull(m);
            }
        }
    }
}