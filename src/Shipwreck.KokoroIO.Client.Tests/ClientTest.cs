using System.Linq;
using System.Threading.Tasks;
using Xunit;

namespace Shipwreck.KokoroIO
{
    public class ClientTest : TestBase
    {
        #region Rest API

        [Fact]
        public void GetProfileAsyncTest()
        {
            using (var c = GetClient())
            {
                var p = c.GetProfieAsync().GetAwaiter().GetResult();

                Assert.NotNull(p);
                Assert.Equal(ProfileType.User, p.Type);
            }
        }

        [Fact]
        public void GetRoomsAsyncTest()
        {
            using (var c = GetClient())
            {
                var rooms = c.GetRoomsAsync().GetAwaiter().GetResult();

                Assert.NotNull(rooms);
            }
        }

        [Fact]
        public void GetPublicRoomsAsyncTest()
        {
            using (var c = GetClient())
            {
                var rooms = c.GetPublicRoomsAsync().GetAwaiter().GetResult();

                Assert.NotNull(rooms);
            }
        }

        [Fact]
        public void GetPrivateRoomsAsyncTest()
        {
            using (var c = GetClient())
            {
                var rooms = c.GetPrivateRoomsAsync().GetAwaiter().GetResult();

                Assert.NotNull(rooms);
            }
        }

        [Fact]
        public void GetDirectMessageRoomsAsyncTest()
        {
            using (var c = GetClient())
            {
                var rooms = c.GetDirectMessageRoomsAsync().GetAwaiter().GetResult();

                Assert.NotNull(rooms);
            }
        }

        [Fact]
        public void GetMessagesAsyncTest()
        {
            using (var c = GetClient())
            {
                var rooms = c.GetPrivateRoomsAsync().GetAwaiter().GetResult();

                var dev = rooms.FirstOrDefault(r => r.ChannelName == "private/dev");

                if (dev == null)
                {
                    return;
                }

                var m = c.GetMessagesAsync(dev.Id).GetAwaiter().GetResult();

                Assert.NotNull(m);
            }
        }

        [Fact]
        public void PostMessageAsyncTest()
        {
            using (var c = GetClient())
            {
                var rooms = c.GetPrivateRoomsAsync().GetAwaiter().GetResult();

                var dev = rooms.FirstOrDefault(r => r.ChannelName == "private/dev");

                if (dev == null)
                {
                    return;
                }

                var m = c.PostMessageAsync(dev.Id, GetTestMessage(), false).GetAwaiter().GetResult();

                Assert.NotNull(m);
            }
        }

        #endregion Rest API

        #region WebSocket API

        [Fact]
        public async Task PingTest()
        {
            using (var c = GetClient())
            {
                await c.ConnectAsync().ConfigureAwait(false);

                await c.CloseAsync().ConfigureAwait(false);
            }
        }

        [Fact]
        public async Task MessageCreatedTest()
        {
            using (var c = GetClient())
            {
                var rooms = await c.GetPrivateRoomsAsync().ConfigureAwait(false);
                var dev = rooms.FirstOrDefault(r => r.ChannelName == "private/dev");
                if (dev == null)
                {
                    return;
                }

                await c.ConnectAsync().ConfigureAwait(false);

                await c.SubscribeAsync(dev).ConfigureAwait(false);

                var msg = $"{nameof(MessageCreatedTest)}見てるぅ～？";

                var received = false;
                c.MessageCreated += (s2, e2) =>
                {
                    Assert.Equal(msg, e2.Data.RawContent);
                    received = true;
                };

                do
                {
                    await Task.Delay(250).ConfigureAwait(false);

                    await c.PostMessageAsync(dev.Id, msg, false);
                }
                while (!received);

                await c.CloseAsync().ConfigureAwait(false);
            }
        }

        #endregion WebSocket API
    }
}