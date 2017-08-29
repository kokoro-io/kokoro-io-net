using System.Linq;
using Xunit;

namespace Shipwreck.KokoroIO
{
    public class ClientTest : TestBase
    {
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
    }

}