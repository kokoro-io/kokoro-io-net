using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Net.Http;
using System.Net.Http.Headers;
using System.Net.WebSockets;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using Newtonsoft.Json;
using Newtonsoft.Json.Linq;

namespace KokoroIO
{
    public sealed class Client : ClientBase
    {
        public static new string DefaultAccessToken { get; set; }

        public Client()
        {
            AccessToken = DefaultAccessToken ?? ClientBase.DefaultAccessToken;
        }

        public Client(HttpClient httpClient)
            : base(httpClient)
        {
            AccessToken = DefaultAccessToken ?? ClientBase.DefaultAccessToken;
        }

        #region Rest API

        #region Token

        public Task<AccessToken[]> GetAccessTokensAsync()
        {
            var req = new HttpRequestMessage(HttpMethod.Get, EndPoint + "/v1/access_tokens");

            return SendAsync<AccessToken[]>(req);
        }

        public Task<AccessToken> PostAccessTokenAsync(string name)
        {
            var req = new HttpRequestMessage(HttpMethod.Post, EndPoint + "/v1/access_tokens");

            var d = new List<KeyValuePair<string, string>>(1)
            {
                new KeyValuePair<string, string>("name", name)
            };

            req.Content = new FormUrlEncodedContent(d);

            return SendAsync<AccessToken>(req);
        }

        public Task DeleteAccessTokenAsync(string accessTokenId)
        {
            var r = new HttpRequestMessage(HttpMethod.Delete, EndPoint + $"/v1/access_tokens/" + accessTokenId);

            return SendAsync(r).ContinueWith(t => t.Result.EnsureSuccessStatusCode());
        }

        #endregion Token

        #region Device

        public Task<Device[]> GetDevicesAsync()
        {
            var req = new HttpRequestMessage(HttpMethod.Get, EndPoint + "/v1/devices");
            return SendAsync<Device[]>(req);
        }

        public Task<Device[]> GetDevicesAsync(string email, string password)
        {
            var req = new HttpRequestMessage(HttpMethod.Get, EndPoint + "/v1/devices");
            req.Headers.Add("X-Account-Token", Convert.ToBase64String(new UTF8Encoding(false).GetBytes(email + ":" + password)));

            return SendAsync<Device[]>(req);
        }

        public Task<Device> PostDeviceAsync(string name, DeviceKind kind, string deviceIdentifier, string notificationIdentifier = null, bool subscribeNotification = false)
        {
            var req = new HttpRequestMessage(HttpMethod.Post, EndPoint + "/v1/devices");

            return PostDeviceAsyncCore(name, kind, deviceIdentifier, notificationIdentifier, subscribeNotification, req);
        }

        public Task<Device> PostDeviceAsync(string email, string password, string name, DeviceKind kind, string deviceIdentifier, string notificationIdentifier = null, bool subscribeNotification = false)
        {
            var req = new HttpRequestMessage(HttpMethod.Post, EndPoint + "/v1/devices");
            req.Headers.Add("X-Account-Token", Convert.ToBase64String(new UTF8Encoding(false).GetBytes(email + ":" + password)));

            return PostDeviceAsyncCore(name, kind, deviceIdentifier, notificationIdentifier, subscribeNotification, req);
        }

        private Task<Device> PostDeviceAsyncCore(string name, DeviceKind kind, string deviceIdentifier, string notificationIdentifier, bool subscribeNotification, HttpRequestMessage req)
        {
            var d = new[]
            {
                new KeyValuePair<string, string>("name", name),
                new KeyValuePair<string, string>("kind", kind.ToApiString()),
                new KeyValuePair<string, string>("device_identifier", deviceIdentifier),
                new KeyValuePair<string, string>("notification_identifier", notificationIdentifier),
                new KeyValuePair<string, string>("subscribe_notification", subscribeNotification ? "true" : "false")
            };

            req.Content = new FormUrlEncodedContent(d);

            return SendAsync<Device>(req);
        }

        public Task DeleteDeviceAsync(string deviceIdentifier)
        {
            var r = new HttpRequestMessage(HttpMethod.Delete, EndPoint + $"/v1/devices/" + Uri.EscapeDataString(deviceIdentifier));

            return SendAsync(r).ContinueWith(t => t.Result.EnsureSuccessStatusCode());
        }

        #endregion Device

        #region Membership

        public Task<Membership[]> GetMembershipsAsync(bool? archived = null, Authority? authority = null)
        {
            var u = new StringBuilder(EndPoint).Append("/v1/memberships");

            if (archived != null)
            {
                u.Append("?archived=").Append(archived.Value ? "true" : "false");
            }

            if (authority != null)
            {
                u.Append(archived.HasValue ? '&' : '?').Append("authority=").Append(authority.Value.ToApiString());
            }

            return SendAsync<Membership[]>(new HttpRequestMessage(HttpMethod.Get, u.ToString()));
        }

        public Task<Membership> PostMembershipAsync(
                                    string channelId,
                                    NotificationPolicy? notificationPolicy = null,
                                    ReadStateTrackingPolicy? trackingPolicy = null,
                                    bool? visible = null,
                                    bool? muted = null)
        {
            var r = new HttpRequestMessage(HttpMethod.Post, EndPoint + $"/v1/memberships");

            var d = new List<KeyValuePair<string, string>>(4)
            {
                new KeyValuePair<string, string>("channel_id", channelId),
            };

            if (notificationPolicy != null)
            {
                d.Add(new KeyValuePair<string, string>("notification_policy", notificationPolicy.Value.ToApiString()));
            }

            if (trackingPolicy != null)
            {
                d.Add(new KeyValuePair<string, string>("read_state_tracking_policy", trackingPolicy.Value.ToApiString()));
            }
            if (visible != null)
            {
                d.Add(new KeyValuePair<string, string>("visible", visible.Value ? "true" : "false"));
            }
            if (muted != null)
            {
                d.Add(new KeyValuePair<string, string>("muted", muted.Value ? "true" : "false"));
            }

            r.Content = new FormUrlEncodedContent(d);

            return SendAsync<Membership>(r);
        }

        public Task<Membership> PutMembershipAsync(
                                    string membershipId,
                                    NotificationPolicy? notificationPolicy = null,
                                    ReadStateTrackingPolicy? trackingPolicy = null,
                                    bool? visible = null,
                                    bool? muted = null,
                                    int? latestReadMessageId = null)
        {
            var r = new HttpRequestMessage(HttpMethod.Put, EndPoint + $"/v1/memberships/" + membershipId);

            var d = new List<KeyValuePair<string, string>>(5);

            if (notificationPolicy != null)
            {
                d.Add(new KeyValuePair<string, string>("notification_policy", notificationPolicy.Value.ToApiString()));
            }

            if (trackingPolicy != null)
            {
                d.Add(new KeyValuePair<string, string>("read_state_tracking_policy", trackingPolicy.Value.ToApiString()));
            }
            if (visible != null)
            {
                d.Add(new KeyValuePair<string, string>("visible", visible.Value ? "true" : "false"));
            }
            if (muted != null)
            {
                d.Add(new KeyValuePair<string, string>("muted", muted.Value ? "true" : "false"));
            }

            if (latestReadMessageId != null)
            {
                d.Add(new KeyValuePair<string, string>("latest_read_message_id", latestReadMessageId.Value.ToString()));
            }

            r.Content = new FormUrlEncodedContent(d);

            return SendAsync<Membership>(r);
        }

        public Task DeleteMembershipAsync(string membershipId)
        {
            var r = new HttpRequestMessage(HttpMethod.Delete, EndPoint + $"/v1/memberships/" + membershipId);

            return SendAsync(r).ContinueWith(t => t.Result.EnsureSuccessStatusCode());
        }

        public Task<Membership> JoinMembershipAsync(string membershipId)
        {
            var r = new HttpRequestMessage(HttpMethod.Put, EndPoint + $"/v1/memberships/" + membershipId + "/join");

            return SendAsync<Membership>(r);
        }

        #endregion Membership

        #region Profile

        public Task<Profile[]> GetProfilesAsync()
            => SendAsync<Profile[]>(new HttpRequestMessage(HttpMethod.Get, EndPoint + "/v1/profiles"));

        public Task<Profile> GetProfileAsync()
            => SendAsync<Profile>(new HttpRequestMessage(HttpMethod.Get, EndPoint + "/v1/profiles/me"));

        public Task<Profile> PutProfileAsync(string screenName = null, string displayName = null, Stream avatar = null)
        {
            var r = new HttpRequestMessage(HttpMethod.Put, EndPoint + $"/v1/profiles/me");

            var mp = new MultipartFormDataContent();
            if (screenName != null)
            {
                mp.Add(new StringContent(screenName), "screen_name");
            }
            if (displayName != null)
            {
                mp.Add(new StringContent(displayName), "display_name");
            }
            if (avatar != null)
            {
                var sc = new StreamContent(avatar);
                sc.Headers.ContentDisposition = new ContentDispositionHeaderValue("form-data")
                {
                    Name = "avatar",
                    FileName = "avatar.jpg"
                };

                string mimeType = "image/jpeg";
                if (avatar.CanSeek)
                {
                    var p = avatar.Position;

                    switch (avatar.ReadByte())
                    {
                        case 0x89:
                            mimeType = "image/png";
                            sc.Headers.ContentDisposition.FileName = "avatar.png";
                            break;

                        case 'G':
                            mimeType = "image/gif";
                            sc.Headers.ContentDisposition.FileName = "avatar.gif";
                            break;
                    }

                    avatar.Position = p;
                }

                sc.Headers.ContentType = new MediaTypeHeaderValue(mimeType);

                mp.Add(sc, "avatar");
            }

            r.Content = mp;

            return SendAsync<Profile>(r);
        }

        #endregion Profile

        #region Channel

        public Task<Channel> GetChannelAsync(string channelId)
        {
            if (!Channel.IsValidId(channelId))
            {
                return new ArgumentException($"Invalid {nameof(channelId)}.").ToTask<Channel>();
            }

            var r = new HttpRequestMessage(HttpMethod.Get, EndPoint + $"/v1/channels/" + channelId);

            return SendAsync<Channel>(r);
        }

        public Task<Channel[]> GetChannelsAsync(bool? archived = null)
            => SendAsync<Channel[]>(
                    new HttpRequestMessage(
                            HttpMethod.Get,
                            EndPoint + "/v1/channels"
                            + (archived == null ? null : archived.Value ? "?archived=true" : "?archived=false")));

        public Task<Channel> PostChannelAsync(string channelName, string description, ChannelKind kind)
        {
            var r = new HttpRequestMessage(HttpMethod.Post, EndPoint + $"/v1/channels");

            var d = new[]
            {
                new KeyValuePair<string, string>("channel[channel_name]", channelName),
                new KeyValuePair<string, string>("channel[description]", description),
                new KeyValuePair<string, string>("channel[kind]",kind.ToApiString())
            };

            r.Content = new FormUrlEncodedContent(d);

            return SendAsync<Channel>(r);
        }

        public Task<Channel> PostDirectMessageChannelAsync(string targetUserProfileId)
        {
            var r = new HttpRequestMessage(HttpMethod.Post, EndPoint + $"/v1/channels/direct_message");

            var d = new[]
            {
                new KeyValuePair<string, string>("target_user_profile_id", targetUserProfileId)
            };

            r.Content = new FormUrlEncodedContent(d);

            return SendAsync<Channel>(r);
        }

        public Task<Channel> PutChannelAsync(string channelId, string channelName, string description)
        {
            if (!Channel.IsValidId(channelId))
            {
                return new ArgumentException($"Invalid {nameof(channelId)}.").ToTask<Channel>();
            }

            var r = new HttpRequestMessage(HttpMethod.Put, EndPoint + $"/v1/channels/" + channelId);

            var d = new[]
            {
                new KeyValuePair<string, string>("channel[channel_name]", channelName),
                new KeyValuePair<string, string>("channel[description]", description)
            };

            r.Content = new FormUrlEncodedContent(d);

            return SendAsync<Channel>(r);
        }

        public Task<Channel> ArchiveChannelAsync(string channelId)
        {
            if (!Channel.IsValidId(channelId))
            {
                return new ArgumentException($"Invalid {nameof(channelId)}.").ToTask<Channel>();
            }

            var r = new HttpRequestMessage(HttpMethod.Put, EndPoint + $"/v1/channels/" + channelId + "/archive");

            return SendAsync<Channel>(r);
        }

        public Task<Channel> UnarchiveChannelAsync(string channelId)
        {
            if (!Channel.IsValidId(channelId))
            {
                return new ArgumentException($"Invalid {nameof(channelId)}.").ToTask<Channel>();
            }

            var r = new HttpRequestMessage(HttpMethod.Put, EndPoint + $"/v1/channels/" + channelId + "/unarchive");

            return SendAsync<Channel>(r);
        }

        public Task<Channel> GetChannelMembershipsAsync(string channelId)
        {
            if (!Channel.IsValidId(channelId))
            {
                return new ArgumentException($"Invalid {nameof(channelId)}.").ToTask<Channel>();
            }

            var r = new HttpRequestMessage(HttpMethod.Get, EndPoint + $"/v1/channels/" + channelId + "/memberships");

            return SendAsync<Channel>(r);
        }

        public Task ManageMemberAsync(string channelId, int memberId, Authority authority)
        {
            if (!Channel.IsValidId(channelId))
            {
                return new ArgumentException($"Invalid {nameof(channelId)}.").ToTask<Channel>();
            }

            var r = new HttpRequestMessage(HttpMethod.Put, EndPoint + $"/v1/channels/" + channelId + "/manage_members/" + memberId);

            var d = new[]
            {
                new KeyValuePair<string, string>("authority", authority.ToApiString())
            };

            r.Content = new FormUrlEncodedContent(d);

            return SendAsync(r).ContinueWith(t => t.Result.EnsureSuccessStatusCode());
        }

        #endregion Channel

        #region Message

        public Task<Message[]> GetMessagesAsync(string channelId, int? limit = null, int? beforeId = null, int? afterId = null)
        {
            if (!Channel.IsValidId(channelId))
            {
                return new ArgumentException($"Invalid {nameof(channelId)}.").ToTask<Message[]>();
            }

            var u = new StringBuilder(EndPoint).Append("/v1/channels/").Append(channelId).Append("/messages");

            var ol = u.Length;

            if (limit > 0)
            {
                u.Append("?limit=").Append(limit.Value);
            }

            if (beforeId != null)
            {
                u.Append(u.Length > ol ? '&' : '?').Append("before_id=").Append(beforeId.Value);
            }
            if (afterId != null)
            {
                u.Append(u.Length > ol ? '&' : '?').Append("after_id=").Append(afterId.Value);
            }

            return SendAsync<Message[]>(new HttpRequestMessage(HttpMethod.Get, u.ToString()));
        }

        public Task<Message> PostMessageAsync(string channelId, string message, bool isNsfw, bool? expandEmbedContents = null, Guid idempotentKey = default(Guid))
        {
            if (!Channel.IsValidId(channelId))
            {
                return new ArgumentException($"Invalid {nameof(channelId)}.").ToTask<Message>();
            }

            var r = new HttpRequestMessage(HttpMethod.Post, EndPoint + $"/v1/channels/" + channelId + "/messages");

            var d = new List<KeyValuePair<string, string>>(4)
            {
                new KeyValuePair<string, string>("message", message),
                new KeyValuePair<string, string>("nsfw", isNsfw ? "true":"false")
            };

            if (expandEmbedContents != null)
            {
                d.Add(new KeyValuePair<string, string>("expand_embed_contents", expandEmbedContents.Value ? "true" : "false"));
            }

            if (idempotentKey != Guid.Empty)
            {
                d.Add(new KeyValuePair<string, string>("idempotent_key", idempotentKey.ToString()));
            }

            r.Content = new FormUrlEncodedContent(d);

            return SendAsync<Message>(r);
        }

        public Task<Message> DeleteMessageAsync(int messageId)
        {
            var r = new HttpRequestMessage(HttpMethod.Delete, EndPoint + $"/v1/messages/" + messageId);

            return SendAsync<Message>(r);
        }

        #endregion Message

        #endregion Rest API

        #region WebSocket API

        public string WebSocketEndPoint { get; set; } = "wss://kokoro.io/cable";

        private TaskCompletionSource<int> _Connected;

        public event EventHandler<EventArgs<Message>> MessageCreated;

        public event EventHandler<EventArgs<Message>> MessageUpdated;

        public event EventHandler<EventArgs<Profile>> ProfileUpdated;

        public event EventHandler<EventArgs<Channel[]>> ChannelsUpdated;

        public event EventHandler<EventArgs<Channel>> ChannelArchived;

        public event EventHandler<EventArgs<Channel>> ChannelUnarchived;

        public event EventHandler<EventArgs<Membership>> MemberJoined;

        public event EventHandler<EventArgs<Membership>> MemberLeaved;

        public event EventHandler<EventArgs<Exception>> SocketError;

        public event EventHandler Disconnected;

        private ClientWebSocket _WebSocket;

        private CancellationTokenSource _WebSocketCancellationTokenSource;

        public ClientState State
        {
            get
            {
                if (_WebSocket != null)
                {
                    switch (_WebSocket.State)
                    {
                        case WebSocketState.Connecting:
                            return ClientState.Connecting;

                        case WebSocketState.Open:
                            return ClientState.Connected;
                    }
                }
                return ClientState.Disconnected;
            }
        }

        public DateTime? LastPingAt { get; private set; }

        public Task ConnectAsync()
        {
            _WebSocketCancellationTokenSource?.Cancel();
            if (_WebSocket != null)
            {
                switch (_WebSocket.State)
                {
                    case WebSocketState.Connecting:
                    case WebSocketState.Open:
                        return Polyfills.CompletedTask;
                }
            }
            if (_Connected?.Task != null)
            {
                return _Connected.Task;
            }

            _WebSocket = new ClientWebSocket();
            _WebSocketCancellationTokenSource = new CancellationTokenSource();
            _WebSocket.Options.SetRequestHeader("X-Access-Token", AccessToken);
            _Connected = new TaskCompletionSource<int>();

            var r = _WebSocket.ConnectAsync(new Uri(WebSocketEndPoint), _WebSocketCancellationTokenSource.Token);

            var tcs = new TaskCompletionSource<int>();

            r.ContinueWith(t =>
            {
                if (t.Status == TaskStatus.RanToCompletion)
                {
                    ReceiveCore();
                }
            });
            return _Connected.Task;
        }

        public Task SubscribeAsync(params Channel[] channels)
            => SubscribeAsync((IEnumerable<Channel>)channels);

        public Task SubscribeAsync(IEnumerable<Channel> channels)
            => SubscribeAsync(channels.Select(r => r.Id));

        public Task SubscribeAsync(IEnumerable<string> channelIds)
        {
            var ws = _WebSocket;
            var ct = _WebSocketCancellationTokenSource?.Token;

            if (ws == null)
            {
                return new InvalidOperationException().ToTask<object>();
            }

            ArraySegment<byte> b;

            using (var ms = new MemoryStream())
            using (var sw = new StreamWriter(ms, new UTF8Encoding(false), 128, true))
            using (var jtw = new JsonTextWriter(sw))
            {
                jtw.WriteStartObject();

                jtw.WritePropertyName("command");
                jtw.WriteValue("message");

                jtw.WritePropertyName("identifier");
                jtw.WriteValue("{\"channel\":\"ChatChannel\"}");

                using (var sw2 = new StringWriter())
                using (var jtw2 = new JsonTextWriter(sw2))
                {
                    jtw2.WriteStartObject();

                    jtw2.WritePropertyName("channels");
                    jtw2.WriteStartArray();
                    foreach (var id in channelIds)
                    {
                        jtw2.WriteValue(id);
                    }
                    jtw2.WriteEndArray();

                    jtw2.WritePropertyName("action");
                    jtw2.WriteValue("subscribe");

                    jtw2.WriteEndObject();

                    jtw2.Flush();

                    jtw.WritePropertyName("data");
                    jtw.WriteValue(sw2.ToString());
                }

                jtw.WriteEndObject();

                jtw.Flush();
                sw.Flush();

                if (!ms.TryGetBuffer(out b))
                {
                    return new InvalidOperationException().ToTask<object>();
                }
            }

            return ws.SendAsync(b, WebSocketMessageType.Text, true, ct ?? default(CancellationToken));
        }

        public Task CloseAsync()
        {
            var ws = _WebSocket;
            _WebSocket = null;
            _WebSocketCancellationTokenSource?.Cancel();
            if (ws == null
                || (ws.State != WebSocketState.Open
                    || ws.State != WebSocketState.CloseReceived
                    || ws.State != WebSocketState.CloseSent))
            {
                return Polyfills.CompletedTask;
            }
            return ws.CloseAsync(WebSocketCloseStatus.NormalClosure, null, default(CancellationToken));
        }

        private async void ReceiveCore()
        {
            try
            {
                var ws = _WebSocket;
                var ct = _WebSocketCancellationTokenSource?.Token ?? default(CancellationToken);

                if (ws?.State == WebSocketState.Open)
                {
                    var buf = new byte[1024];
                    var js = new JsonSerializer();

                    while (ws?.State == WebSocketState.Open)
                    {
                        using (var ms = new MemoryStream())
                        {
                            while (ws?.State == WebSocketState.Open)
                            {
                                ct.ThrowIfCancellationRequested();

                                var res = await ws.ReceiveAsync(new ArraySegment<byte>(buf, 0, buf.Length), ct).ConfigureAwait(false);
                                ms.Write(buf, 0, res.Count);

                                if (ms.Length > 0 && res.EndOfMessage)
                                {
                                    break;
                                }
                                await Task.Delay(1).ConfigureAwait(false);
                            }

                            ms.Position = 0;

                            using (var sr = new StreamReader(ms, Encoding.UTF8))
                            using (var jtr = new JsonTextReader(sr))
                            {
                                var jo = js.Deserialize<JObject>(jtr);

                                switch (jo?.Property("type")?.Value?.Value<string>())
                                {
                                    case "welcome":
                                        await SendSubscribeAsync(ws, ct).ConfigureAwait(false);
                                        continue;

                                    case "ping":
                                        LastPingAt = DateTime.Now;
                                        continue;

                                    case "confirm_subscription":
                                        _Connected?.TrySetResult(0);
                                        _Connected = null;
                                        continue;
                                }

                                try
                                {
                                    var msg = jo?.Property("message")?.Value?.Value<JObject>();
                                    var eventName = msg?.Property("event")?.Value?.Value<string>();
                                    switch (eventName)
                                    {
                                        case "message_created":
                                            DispatchEvent(msg, MessageCreated);
                                            break;

                                        case "message_updated":
                                            DispatchEvent(msg, MessageUpdated);
                                            break;

                                        case "profile_updated":
                                            DispatchEvent(msg, ProfileUpdated);
                                            break;

                                        case "channels_updated":
                                            DispatchEvent(msg, ChannelsUpdated);
                                            break;

                                        case "channel_archived":
                                            DispatchEvent(msg, ChannelArchived);
                                            break;

                                        case "channel_unarchived":
                                            DispatchEvent(msg, ChannelUnarchived);
                                            break;

                                        case "member_joined":
                                            DispatchEvent(msg, MemberJoined);
                                            break;

                                        case "member_leaved":
                                            DispatchEvent(msg, MemberLeaved);
                                            break;
                                    }
                                }
                                catch (Exception ex2)
                                {
                                    try
                                    {
                                        SocketError?.Invoke(this, new EventArgs<Exception>(ex2));
                                    }
                                    catch { }
                                }
                            }
                        }
                    }
                }
            }
            catch (Exception ex)
            {
                try
                {
                    SocketError?.Invoke(this, new EventArgs<Exception>(ex));
                }
                catch { }

                DisposeWebSocket();
            }
            finally
            {
                LastPingAt = null;
                Disconnected?.Invoke(this, EventArgs.Empty);
            }
        }

        private void DispatchEvent<T>(JObject msg, EventHandler<EventArgs<T>> handler)
            where T : class
        {
            if (handler != null)
            {
                var mo = msg.Property("data")?.Value?.ToObject<T>();
                if (mo != null)
                {
                    handler(this, new EventArgs<T>(mo));
                }
            }
        }

        private Task SendSubscribeAsync(ClientWebSocket ws, CancellationToken ct)
        {
            ArraySegment<byte> b;
            using (var ms = new MemoryStream())
            using (var sw = new StreamWriter(ms, new UTF8Encoding(false), 128, true))
            using (var jtw = new JsonTextWriter(sw))
            {
                jtw.WriteStartObject();

                jtw.WritePropertyName("command");
                jtw.WriteValue("subscribe");

                jtw.WritePropertyName("identifier");
                jtw.WriteValue("{\"channel\":\"ChatChannel\"}");

                jtw.WriteEndObject();

                jtw.Flush();
                sw.Flush();

                if (!ms.TryGetBuffer(out b))
                {
                    return new InvalidOperationException().ToTask<object>();
                }
            }
            return ws.SendAsync(b, WebSocketMessageType.Text, true, ct);
        }

        #endregion WebSocket API

        protected override void Dispose(bool disposing)
        {
            DisposeWebSocket();
            base.Dispose(disposing);
        }

        private void DisposeWebSocket()
        {
            try
            {
                LastPingAt = null;
                _Connected?.TrySetCanceled();
                _WebSocket?.Dispose();
                _WebSocket = null;
                _WebSocketCancellationTokenSource?.Cancel();
                _WebSocketCancellationTokenSource = null;
            }
            catch { }
        }
    }
}