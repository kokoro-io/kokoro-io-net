using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;

namespace KokoroIO.Proxy.Controllers
{
    [Route("api/v1")]
    public class KokoroIOController : Controller
    {
        private Client GetClient()
        {
            var c = new Client();

            if (Request.Headers.TryGetValue("X-Access-Token", out var at))
            {
                c.AccessToken = at.First();
            }

            return c;
        }

        #region Rest API

        #region Token

        [HttpGet]
        [Route("access_tokens")]
        public async Task<AccessToken[]> GetAccessTokensAsync()
        {
            using (var c = GetClient())
            {
                return await c.GetAccessTokensAsync().ConfigureAwait(false);
            }
        }

        [HttpPost]
        [Route("access_tokens")]
        public async Task<AccessToken> PostAccessTokenAsync(string name)
        {
            using (var c = GetClient())
            {
                return await c.PostAccessTokenAsync(name).ConfigureAwait(false);
            }
        }

        [HttpDelete]
        [Route("access_tokens/{accessTokenId}")]
        public async Task DeleteAccessTokenAsync(string accessTokenId)
        {
            using (var c = GetClient())
            {
                await c.DeleteAccessTokenAsync(accessTokenId).ConfigureAwait(false);
            }
        }

        #endregion Token

        #region Bot

        [HttpPost]
        [Route("bot/channels/{channelId}/messages")]
        public async Task<Message> PostBotMessageAsync(string channelId,  string message,  string display_name = null, bool? nsfw = null)
        {
            using (var c = new BotClient())
            {
                if (Request.Headers.TryGetValue("X-Access-Token", out var at))
                {
                    c.AccessToken = at.First();
                }

                return await c.PostMessageAsync(channelId, message, display_name, nsfw);
            }
        }

        #endregion Bot

        #region Device

        private bool TryGetClientWithAccount(out Client c, out string mailAddress, out string password)
        {
            c = GetClient();
            if (c.AccessToken == null)
            {
                if (Request.Headers.TryGetValue("X-Account-Token", out var at))
                {
                    try
                    {
                        var s = Encoding.UTF8.GetString(Convert.FromBase64String(at.First()));
                        var i = s.IndexOf(':');
                        if (i >= 0)
                        {
                            mailAddress = s.Substring(0, i);
                            password = s.Substring(i + 1);
                            return true;
                        }
                    }
                    catch { }
                }
            }

            mailAddress = null;
            password = null;
            return false;
        }

        [HttpGet]
        [Route("devices")]
        public async Task<Device[]> GetDevicesAsync()
        {
            var r = TryGetClientWithAccount(out var c, out var m, out var p);
            using (c)
            {
                if (r)
                {
                    return await c.GetDevicesAsync(m, p).ConfigureAwait(false);
                }
                return await c.GetDevicesAsync().ConfigureAwait(false);
            }
        }

        [HttpPost]
        [Route("devices")]
        public async Task<Device> PostDeviceAsync(string name,  DeviceKind kind, string device_identifier,  string notification_identifier = null,  bool subscribe_notification = false)
        {
            var r = TryGetClientWithAccount(out var c, out var m, out var p);
            using (c)
            {
                if (r)
                {
                    return await c.PostDeviceAsync(m, p, name, kind, device_identifier, notification_identifier, subscribe_notification).ConfigureAwait(false);
                }
                return await c.PostDeviceAsync(name, kind, device_identifier, notification_identifier, subscribe_notification).ConfigureAwait(false);
            }
        }

        [HttpDelete]
        [Route("devices/{deviceIdentifier}")]
        public async Task DeleteDeviceAsync(string deviceIdentifier)
        {
            using (var c = GetClient())
            {
                await c.DeleteDeviceAsync(deviceIdentifier).ConfigureAwait(false);
            }
        }

        #endregion Device

        #region Membership

        [HttpGet]
        [Route("memberships")]
        public async Task<Membership[]> GetMembershipsAsync(bool? archived = null, Authority? authority = null)
        {
            using (var c = GetClient())
            {
                return await c.GetMembershipsAsync(archived, authority).ConfigureAwait(false);
            }
        }

        [HttpPost]
        [Route("memberships")]
        public async Task<Membership> PostMembershipAsync(string channel_id,  bool? disable_notification = null)
        {
            using (var c = GetClient())
            {
                return await c.PostMembershipAsync(channel_id, disable_notification).ConfigureAwait(false);
            }
        }

        [HttpDelete]
        [Route("memberships/{membershipId}")]
        public async Task DeleteMembershipAsync(string membershipId)
        {
            using (var c = GetClient())
            {
                await c.DeleteMembershipAsync(membershipId).ConfigureAwait(false);
            }
        }

        [HttpPut]
        [Route("memberships")]
        public async Task<Membership> PutMembershipAsync(string channel_id,  bool? disable_notification = null)
        {
            using (var c = GetClient())
            {
                return await c.PutMembershipAsync(channel_id, disable_notification).ConfigureAwait(false);
            }
        }

        #endregion Membership

        #region Profile

        [HttpGet]
        [Route("profiles")]
        public async Task<Profile[]> GetProfilesAsync()
        {
            using (var c = GetClient())
            {
                return await c.GetProfilesAsync().ConfigureAwait(false);
            }
        }

        [HttpGet]
        [Route("profiles/me")]
        public async Task<Profile> GetProfileAsync()
        {
            using (var c = GetClient())
            {
                return await c.GetProfileAsync().ConfigureAwait(false);
            }
        }

        [HttpPost]
        [Route("profiles/me")]
        public async Task<Profile> PutProfileAsync(string screen_name = null,  string display_name = null,  Stream avatar = null)
        {
            using (var c = GetClient())
            {
                return await c.PutProfileAsync(screen_name, display_name, avatar).ConfigureAwait(false);
            }
        }

        #endregion Profile

        #region Channel

        [HttpGet]
        [Route("channels/{channelId}")]
        public async Task<Channel> GetChannelAsync(string channelId)
        {
            using (var c = GetClient())
            {
                return await c.GetChannelAsync(channelId).ConfigureAwait(false);
            }
        }

        [HttpGet]
        [Route("channels")]
        public async Task<Channel[]> GetChannelsAsync(bool? archived = null)
        {
            using (var c = GetClient())
            {
                return await c.GetChannelsAsync(archived).ConfigureAwait(false);
            }
        }

        [HttpPost]
        [Route("channels")]
        public async Task<Channel> PostChannelAsync(string channel_name,  string description,  ChannelKind kind)
        {
            using (var c = GetClient())
            {
                return await c.PostChannelAsync(channel_name, description, kind).ConfigureAwait(false);
            }
        }

        [HttpPost]
        [Route("channels/direct_message")]
        public async Task<Channel> PostDirectMessageChannelAsync(string target_user_profile_id)
        {
            using (var c = GetClient())
            {
                return await c.PostDirectMessageChannelAsync(target_user_profile_id).ConfigureAwait(false);
            }
        }

        [HttpPut]
        [Route("channels/{channelId}")]
        public async Task<Channel> PutChannelAsync(string channelId,  string channel_name,  string description)
        {
            using (var c = GetClient())
            {
                return await c.PutChannelAsync(channelId, channel_name, description).ConfigureAwait(false);
            }
        }

        [HttpDelete]
        [Route("channels/{channelId}/archive")]
        public async Task<Channel> ArchiveChannelAsync(string channelId)
        {
            using (var c = GetClient())
            {
                return await c.ArchiveChannelAsync(channelId).ConfigureAwait(false);
            }
        }

        [HttpPut]
        [Route("channels/{channelId}/unarchive")]
        public async Task<Channel> UnarchiveChannelAsync(string channelId)
        {
            using (var c = GetClient())
            {
                return await c.UnarchiveChannelAsync(channelId).ConfigureAwait(false);
            }
        }

        [HttpGet]
        [Route("channels/{channelId}/memberships")]
        public async Task<Channel> GetChannelMembershipsAsync(string channelId)
        {
            using (var c = GetClient())
            {
                return await c.GetChannelAsync(channelId).ConfigureAwait(false);
            }
        }

        [HttpPut]
        [Route("channels/{channelId}/manage_members/{memberId}")]
        public async Task ManageMemberAsync(string channelId, int memberId, Authority authority)
        {
            using (var c = GetClient())
            {
                await c.ManageMemberAsync(channelId, memberId, authority).ConfigureAwait(false);
            }
        }

        #endregion Channel

        #region Message

        [HttpGet]
        [Route("channels/{channelId}/messages")]
        public async Task<Message[]> GetMessagesAsync(string channelId, int? limit = null, int? before_id = null, int? after_id = null)
        {
            using (var c = GetClient())
            {
                return await c.GetMessagesAsync(channelId, limit, before_id, after_id).ConfigureAwait(false);
            }
        }

        [HttpPost]
        [Route("channels/{channelId}/messages")]
        public async Task<Message> PostMessageAsync(string channelId,  string message,  bool nsfw,  Guid idempotent_key = default(Guid))
        {
            using (var c = GetClient())
            {
                return await c.PostMessageAsync(channelId, message, nsfw, idempotent_key);
            }
        }

        [HttpDelete]
        [Route("messages/{messageId}")]
        public async Task DeleteMessageAsync(int messageId)
        {
            using (var c = GetClient())
            {
                await c.DeleteMessageAsync(messageId).ConfigureAwait(false);
            }
        }

        #endregion Message

        #endregion Rest API
    }
}