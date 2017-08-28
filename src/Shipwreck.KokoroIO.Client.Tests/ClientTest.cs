using Microsoft.Extensions.Configuration;
using Xunit;

namespace Shipwreck.KokoroIO
{
    public class ClientTest
    {
        [Fact]
        public void GetProfileAsyncTest()
        {
            using (var c = TestHelper.GetClient())
            {
                var p = c.GetProfieAsync().GetAwaiter().GetResult();

                Assert.NotNull(p);
                Assert.Equal(ProfileType.User, p.Type);
            }
        }

        [Fact]
        public void GetRoomsAsyncTest()
        {
            using (var c = TestHelper.GetClient())
            {
                var rooms = c.GetRoomsAsync().GetAwaiter().GetResult();

                Assert.NotNull(rooms);
            }
        }

        [Fact]
        public void GetPublicRoomsAsyncTest()
        {
            using (var c = TestHelper.GetClient())
            {
                var rooms = c.GetPublicRoomsAsync().GetAwaiter().GetResult();

                Assert.NotNull(rooms);
            }
        }

        [Fact]
        public void GetPrivateRoomsAsyncTest()
        {
            using (var c = TestHelper.GetClient())
            {
                var rooms = c.GetPrivateRoomsAsync().GetAwaiter().GetResult();

                Assert.NotNull(rooms);
            }
        }

        [Fact]
        public void GetDirectMessageRoomsAsyncTest()
        {
            using (var c = TestHelper.GetClient())
            {
                var rooms = c.GetDirectMessageRoomsAsync().GetAwaiter().GetResult();

                Assert.NotNull(rooms);
            }
        }
    }
}