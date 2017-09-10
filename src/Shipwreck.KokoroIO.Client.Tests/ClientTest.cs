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
                var welcomed = false;
                var ping = 0;

                c.Connected += (s, e) =>
                {
                    Assert.False(welcomed);
                    welcomed = true;
                };
                c.Ping += (s, e) =>
                {
                    Assert.True(welcomed);
                    ping++;
                };

                await c.ConnectAsync().ConfigureAwait(false);

                while (ping < 1)
                {
                    await Task.Delay(250).ConfigureAwait(false);
                }
            }
        }

        #endregion WebSocket API
    }
}