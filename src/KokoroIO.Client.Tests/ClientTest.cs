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

        [Fact]
        public async Task PostAccessTokenAsyncTest_ByAccount()
        {
            using (var c = GetClient())
            {
                c.AccessToken = null;
                var em = Configuration["MailAddress"] ?? Environment.GetEnvironmentVariable("MAIL_ADDRESS");
                var pw = Configuration["Password"] ?? Environment.GetEnvironmentVariable("PASSWORD");

                var n = nameof(PostAccessTokenAsyncTest_ByAccount) + "/" + DateTime.Now.Ticks + "==";

                var p = await c.PostDeviceAsync(em, pw, n, DeviceKind.Unknown, n);

                c.AccessToken = p.AccessToken.Token;

                var devs = await c.GetDevicesAsync();
                Assert.True(devs.Any(d => d.DeviceIdentifier == n));

                c.AccessToken = (await c.PostAccessTokenAsync("unit-test")).Token;

                await c.DeleteDeviceAsync(p.DeviceIdentifier);

                devs = await c.GetDevicesAsync();
                Assert.False(devs.Any(d => d.DeviceIdentifier == n));
            }
        }

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

        private async Task<Membership> GetTestChannelMembershipAsync(Client c)
        {
            var mss = await c.GetMembershipsAsync().ConfigureAwait(false);
            var ms = mss.FirstOrDefault(m => m.Channel.Id == TestChannelId);
            return ms;
        }

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
                var ms = await GetTestChannelMembershipAsync(c);

                await c.PutMembershipAsync(ms.Id, NotificationPolicy.OnlyMentions, ReadStateTrackingPolicy.ConsumeLast);

                var ch = await c.GetChannelAsync(ms.Channel.Id);

                Assert.Equal(NotificationPolicy.OnlyMentions, ch.Membership.NotificationPolicy);
                Assert.Equal(ReadStateTrackingPolicy.ConsumeLast, ch.Membership.ReadStateTrackingPolicy);

                await c.PutMembershipAsync(ms.Id, NotificationPolicy.Nothing, ReadStateTrackingPolicy.ConsumeLatest);

                ch = await c.GetChannelAsync(ms.Channel.Id);

                Assert.Equal(NotificationPolicy.Nothing, ch.Membership.NotificationPolicy);
                Assert.Equal(ReadStateTrackingPolicy.ConsumeLatest, ch.Membership.ReadStateTrackingPolicy);

                await c.PutMembershipAsync(ms.Id, NotificationPolicy.AllMessages, ReadStateTrackingPolicy.KeepLatest);

                ch = await c.GetChannelAsync(ms.Channel.Id);

                Assert.Equal(NotificationPolicy.AllMessages, ch.Membership.NotificationPolicy);
                Assert.Equal(ReadStateTrackingPolicy.KeepLatest, ch.Membership.ReadStateTrackingPolicy);
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
                var dev = await GetTestChannelAsync(c);

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
                var dev = await GetTestChannelAsync(c);

                var m = await c.GetMessagesAsync(dev.Id);

                Assert.NotNull(m);

                var fm = m.OrderBy(e => e.RawContent.Length).LastOrDefault();

                Assert.NotInRange(fm.HtmlContent.Length, int.MinValue, 0);
                Assert.NotInRange(fm.PlainTextContent.Length, int.MinValue, -1);
                Assert.NotInRange(fm.RawContent.Length, int.MinValue, 0);
            }
        }

        [Fact]
        public async Task PostMessageAsyncTest()
        {
            using (var c = GetClient())
            {
                var dev = await GetTestChannelAsync(c);

                var m = await c.PostMessageAsync(dev.Id, GetTestMessage(), false, expandEmbedContents: true);

                Assert.NotNull(m);
            }
        }

        [Fact]
        public async Task DeleteMessageAsyncTest()
        {
            using (var c = GetClient())
            {
                var dev = await GetTestChannelAsync(c);

                var m = await c.PostMessageAsync(dev.Id, GetTestMessage(), false);

                Assert.NotNull(m);

                await c.DeleteMessageAsync(m.Id);
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
                var dev = await GetTestChannelAsync(c);

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
                var dev = await GetTestChannelAsync(c);

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
                var dev = await GetTestChannelAsync(c);

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
                var dev = await GetTestChannelAsync(c);

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