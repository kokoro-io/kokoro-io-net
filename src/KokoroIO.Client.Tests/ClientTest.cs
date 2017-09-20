using System.Linq;
using System.Threading.Tasks;
using Xunit;

namespace KokoroIO
{
    public class ClientTest : TestBase
    {
        #region Rest API

        #region AccessToken

        [Fact]
        public async Task GetAccessTokensAsyncTest()
        {
            using (var c = GetClient())
            {
                var p = await c.GetAccessTokensAsync();

                Assert.NotNull(p);
            }
        }

        // TODO: PostAccessTokenAsync

        #endregion AccessToken

        #region Device

        // TODO: GetDevicesAsync
        // TODO: PostAccessTokenAsync

        #endregion Device

        #region Membership

        [Fact]
        public async Task GetMembershipsAsync()
        {
            using (var c = GetClient())
            {
                var p = await c.GetMembershipsAsync();

                Assert.NotNull(p);
            }
        }

        // TODO: PostMembershipAsync
        // TODO: DeleteMembershipAsync
        [Fact]
        public async Task PutMembershipAsync()
        {
            using (var c = GetClient())
            {
                var p = await c.GetMembershipsAsync();

                Assert.NotNull(p);

                var ms = p.Single(m => m.Room.ChannelName == "private/dev");

                await c.PutMembershipAsync(ms.Id, true);
                await c.PutMembershipAsync(ms.Id, false);
            }
        }

        #endregion Membership

        #region MyRegion

        [Fact]
        public async Task GetProfilesAsyncTest()
        {
            using (var c = GetClient())
            {
                var p = await c.GetProfilesAsync();

                Assert.NotNull(p);
            }
        }

        [Fact]
        public async Task GetProfileAsyncTest()
        {
            using (var c = GetClient())
            {
                var p = await c.GetProfileAsync();

                Assert.NotNull(p);
                Assert.Equal(ProfileType.User, p.Type);
            }
        }

        #endregion MyRegion

        #region Room

        [Fact]
        public async Task GetRoomsAsyncTest()
        {
            using (var c = GetClient())
            {
                var rooms = await c.GetRoomsAsync();

                Assert.NotNull(rooms);
            }
        }

        [Fact]
        public async Task GetPublicRoomsAsyncTest()
        {
            using (var c = GetClient())
            {
                var rooms = await c.GetPublicRoomsAsync();

                Assert.NotNull(rooms);
            }
        }

        [Fact]
        public async Task GetPrivateRoomsAsyncTest()
        {
            using (var c = GetClient())
            {
                var rooms = await c.GetPrivateRoomsAsync();

                Assert.NotNull(rooms);
            }
        }

        [Fact]
        public void GetDirectMessageRoomsAsyncTest()
        {
            using (var c = GetClient())
            {
                var rooms = c.GetDirectMessageRoomsAsync();

                Assert.NotNull(rooms);
            }
        }

        #endregion Room

        #region Message

        [Fact]
        public async Task GetMessagesAsyncTest()
        {
            using (var c = GetClient())
            {
                var rooms = await c.GetPrivateRoomsAsync();

                var dev = rooms.FirstOrDefault(r => r.ChannelName == "private/dev");

                if (dev == null)
                {
                    return;
                }

                var m = await c.GetMessagesAsync(dev.Id);

                Assert.NotNull(m);
            }
        }

        [Fact]
        public async Task PostMessageAsyncTest()
        {
            using (var c = GetClient())
            {
                var rooms = await c.GetPrivateRoomsAsync();

                var dev = rooms.FirstOrDefault(r => r.ChannelName == "private/dev");

                if (dev == null)
                {
                    return;
                }

                var m = await c.PostMessageAsync(dev.Id, GetTestMessage(), false);

                Assert.NotNull(m);
            }
        }

        #endregion Message

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