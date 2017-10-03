using System;
using System.IO;
using System.Linq;
using System.Net.Http;
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

        [Fact]
        public async Task PostAccessTokenAsyncTest()
        {
            using (var c = GetClient())
            {
                var n = nameof(GetAccessTokensAsyncTest) + DateTime.Now.Ticks;
                var p = await c.PostAccessTokenAsync(n);
                await c.DeleteAccessTokenAsync(p.Id);
            }
        }

        #endregion AccessToken

        #region Device

        // TODO: GetDevicesAsync(X-Account-Token)
        // TODO: PostAccessTokenAsync(X-Account-Token)

        [Fact]
        public async Task PostDeviceAsyncTest()
        {
            using (var c = GetClient())
            {
                var n = nameof(PostDeviceAsyncTest) + "/" + DateTime.Now.Ticks + "==";

                var devs = await c.GetDevicesAsync();
                Assert.False(devs.Any(d => d.DeviceIdentifier == n));

                var p = await c.PostDeviceAsync(n, DeviceKind.Unknown, n);

                devs = await c.GetDevicesAsync();
                Assert.True(devs.Any(d => d.DeviceIdentifier == n));

                await c.DeleteDeviceAsync(p.DeviceIdentifier);

                devs = await c.GetDevicesAsync();
                Assert.False(devs.Any(d => d.DeviceIdentifier == n));
            }
        }

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

                var ms = p.Single(m => m.Channel.ChannelName == "private/dev");

                await c.PutMembershipAsync(ms.Id, true);
                await c.PutMembershipAsync(ms.Id, false);
            }
        }

        #endregion Membership

        #region Profile

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

        [Fact]
        public async Task PutProfileAsyncTest()
        {
            using (var c = GetClient())
            {
                var memberships = await c.GetMembershipsAsync();

                var dev = memberships
                                .Select(ms => ms.Channel)
                                .Single(r => r.Kind == ChannelKind.PrivateChannel
                                            && !r.IsArchived
                                            && r.ChannelName == "private/dev");

                var p = await c.GetProfileAsync();

                byte[] originalAvatar;
                using (var hc = new HttpClient())
                {
                    originalAvatar = await hc.GetByteArrayAsync(p.Avatars?.OrderByDescending(a => a.Size).FirstOrDefault()?.Url ?? p.Avatar);
                }

                var rd = new Random();
                using (var fs = new FileStream("testimage.jpg", FileMode.Open))
                {
                    await c.PutProfileAsync("test" + rd.Next(), "自動テストに乗っ取られ太郎", fs);
                }

                await c.PostMessageAsync(dev.Id, GetTestMessage(), false);

                using (var ms = new MemoryStream(originalAvatar))
                {
                    await c.PutProfileAsync(p.ScreenName, p.DisplayName, ms);
                }
            }
        }

        #endregion Profile

        #region Channel

        [Fact]
        public async Task GetChannelsAsyncTest()
        {
            using (var c = GetClient())
            {
                var channels = await c.GetChannelsAsync();

                Assert.NotNull(channels);

                var ch = channels.First();

                Assert.NotNull(await c.GetChannelAsync(ch.Id));
            }
        }

        [Fact]
        public async Task GetChannelMembershipsAsyncTest()
        {
            using (var c = GetClient())
            {
                var mss = await c.GetMembershipsAsync();
                var ms = mss.First();

                var rmss = await c.GetChannelMembershipsAsync(ms.Channel.Id);

                Assert.True(rmss.Memberships.Any(m => m.Profile.Id == ms.Profile.Id));
            }
        }

        #endregion Channel

        #region Message

        [Fact]
        public async Task GetMessagesAsyncTest()
        {
            using (var c = GetClient())
            {
                var memberships = await c.GetMembershipsAsync();

                var dev = memberships
                                .Select(ms => ms.Channel)
                                .Single(r => r.Kind == ChannelKind.PrivateChannel
                                            && !r.IsArchived
                                            && r.ChannelName == "private/dev");

                var m = await c.GetMessagesAsync(dev.Id);

                Assert.NotNull(m);
            }
        }

        [Fact]
        public async Task PostMessageAsyncTest()
        {
            using (var c = GetClient())
            {
                var memberships = await c.GetMembershipsAsync();

                var dev = memberships
                                .Select(ms => ms.Channel)
                                .Single(r => r.Kind == ChannelKind.PrivateChannel
                                            && !r.IsArchived
                                            && r.ChannelName == "private/dev");

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
                var memberships = await c.GetMembershipsAsync();

                var dev = memberships
                                .Select(ms => ms.Channel)
                                .Single(r => r.Kind == ChannelKind.PrivateChannel
                                            && !r.IsArchived
                                            && r.ChannelName == "private/dev");

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

        [Fact]
        public async Task MessageUpdatedTest()
        {
            using (var c = GetClient())
            {
                var memberships = await c.GetMembershipsAsync();

                var dev = memberships
                                .Select(ms => ms.Channel)
                                .Single(r => r.Kind == ChannelKind.PrivateChannel
                                            && !r.IsArchived
                                            && r.ChannelName == "private/dev");

                await c.ConnectAsync().ConfigureAwait(false);

                await c.SubscribeAsync(dev).ConfigureAwait(false);

                var msg = $"{nameof(MessageUpdatedTest)}見てるぅ～？ https://github.com/kokoro-io/kokoro-io-net";

                var received = false;
                c.MessageUpdated += (s2, e2) =>
                {
                    Assert.Equal(msg, e2.Data.RawContent);
                    received = true;
                };

                do
                {
                    await Task.Delay(1000).ConfigureAwait(false);

                    await c.PostMessageAsync(dev.Id, msg, false);
                }
                while (!received);

                await c.CloseAsync().ConfigureAwait(false);
            }
        }

        [Fact]
        public async Task ProfileUpdatedTest()
        {
            using (var c = GetClient())
            {
                var memberships = await c.GetMembershipsAsync();

                var dev = memberships
                                .Select(ms => ms.Channel)
                                .Single(r => r.Kind == ChannelKind.PrivateChannel
                                            && !r.IsArchived
                                            && r.ChannelName == "private/dev");

                var profile = await c.GetProfileAsync().ConfigureAwait(false);

                await c.ConnectAsync().ConfigureAwait(false);

                await c.SubscribeAsync(dev).ConfigureAwait(false);

                var msg = $"{nameof(ProfileUpdatedTest)}見てるぅ～？";
                try
                {
                    var received = false;
                    c.ProfileUpdated += (s2, e2) =>
                    {
                        received = true;
                    };

                    var i = 0;
                    do
                    {
                        Assert.True(i < 10, "Timeouted");

                        await Task.Delay(1000).ConfigureAwait(false);

                        await c.PutProfileAsync(profile.ScreenName, "ProfileUpdatedTest" + i++);
                        await c.PostMessageAsync(dev.Id, msg, false);
                    }
                    while (!received);
                }
                finally
                {
                    await c.PutProfileAsync(profile.ScreenName, profile.DisplayName);
                }
                await c.CloseAsync().ConfigureAwait(false);
            }
        }

        [Fact]
        public async Task ChannelsUpdatedTest()
        {
            using (var c = GetClient())
            {
                var memberships = await c.GetMembershipsAsync();

                var dev = memberships
                                .Select(ms => ms.Channel)
                                .Single(r => r.Kind == ChannelKind.PrivateChannel
                                            && !r.IsArchived
                                            && r.ChannelName == "private/dev");

                var profile = await c.GetProfileAsync().ConfigureAwait(false);

                await c.ConnectAsync().ConfigureAwait(false);

                await c.SubscribeAsync(dev).ConfigureAwait(false);

                var msg = $"{nameof(ChannelsUpdatedTest)}見てるぅ～？";
                try
                {
                    var received = false;
                    c.ChannelsUpdated += (s2, e2) =>
                    {
                        received = true;
                    };

                    var i = 0;
                    do
                    {
                        Assert.True(i < 10, "Timeouted");

                        await Task.Delay(1000).ConfigureAwait(false);

                        await c.PutChannelAsync(dev.Id, dev.ChannelName, $"{nameof(ChannelsUpdatedTest)}見てるぅ～？" + i++).ConfigureAwait(false);
                    }
                    while (!received);
                }
                finally
                {
                    await c.PutChannelAsync(dev.Id, dev.ChannelName, dev.Description);
                }
                await c.CloseAsync().ConfigureAwait(false);
            }
        }

        #endregion WebSocket API
    }
}