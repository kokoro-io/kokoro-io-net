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

        #endregion Token

        #region Device

        public Task<Device[]> GetDevicesAsync(string email, string password)
        {
            var req = new HttpRequestMessage(HttpMethod.Get, EndPoint + "/v1/devices");
            req.Headers.Add("X-Account-Token", Convert.ToBase64String(new UTF8Encoding(false).GetBytes(email + ":" + password)));

            return SendAsync<Device[]>(req);
        }

        public Task<Device> PostDeviceAsync(string email, string password, string name, DeviceKind kind, string deviceIdentifier, string notificationIdentifier = null, bool subscribeNotification = false)
        {
            var req = new HttpRequestMessage(HttpMethod.Post, EndPoint + "/v1/devices");
            req.Headers.Add("X-Account-Token", Convert.ToBase64String(new UTF8Encoding(false).GetBytes(email + ":" + password)));

            var d = new List<KeyValuePair<string, string>>(5)
            {
                new KeyValuePair<string, string>("name", name),
                new KeyValuePair<string, string>("kind", kind.ToApiString()),
                new KeyValuePair<string, string>("device_identifier", deviceIdentifier)
            };

            if (notificationIdentifier != null)
            {
                d.Add(new KeyValuePair<string, string>("notification_identifier", notificationIdentifier));
                d.Add(new KeyValuePair<string, string>("subscribe_notification", subscribeNotification ? "true" : "false"));
            }

            req.Content = new FormUrlEncodedContent(d);

            return SendAsync<Device>(req);
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

        public Task<Membership> PostMembershipAsync(string roomId, bool? disableNotification = null)
        {
            var r = new HttpRequestMessage(HttpMethod.Post, EndPoint + $"/v1/memberships");

            var d = new List<KeyValuePair<string, string>>(2)
            {
                new KeyValuePair<string, string>("room_id", roomId),
            };

            if (disableNotification != null)
            {
                d.Add(new KeyValuePair<string, string>("disable_notification", disableNotification.Value ? "true" : "false"));
            }

            r.Content = new FormUrlEncodedContent(d);

            return SendAsync<Membership>(r);
        }

        public Task DeleteMembershipAsync(string membershipId)
        {
            var r = new HttpRequestMessage(HttpMethod.Delete, EndPoint + $"/v1/memberships/" + membershipId);

            return SendAsync(r).ContinueWith(t => t.Result.EnsureSuccessStatusCode());
        }

        public Task<Membership> PutMembershipAsync(string membershipId, bool? disableNotification = null)
        {
            var r = new HttpRequestMessage(HttpMethod.Put, EndPoint + $"/v1/memberships/" + membershipId);

            var d = new List<KeyValuePair<string, string>>(1);

            if (disableNotification != null)
            {
                d.Add(new KeyValuePair<string, string>("disable_notification", disableNotification.Value ? "true" : "false"));
            }

            r.Content = new FormUrlEncodedContent(d);

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

        #region Room

        public Task<Room> PostRoomAsync(string channelName, string description, RoomKind kind)
        {
            var r = new HttpRequestMessage(HttpMethod.Post, EndPoint + $"/v1/rooms");

            var d = new[]
            {
                new KeyValuePair<string, string>("room[channel_name]", channelName),
                new KeyValuePair<string, string>("room[description]", description),
                new KeyValuePair<string, string>("room[kind]",kind.ToApiString())
            };

            r.Content = new FormUrlEncodedContent(d);

            return SendAsync<Room>(r);
        }

        public Task<Room[]> GetRoomsAsync(bool? archived = null)
            => SendAsync<Room[]>(
                    new HttpRequestMessage(
                            HttpMethod.Get,
                            EndPoint + "/v1/rooms"
                            + GetArchivedQuery(archived)));

        private static string GetArchivedQuery(bool? archived)
            => (archived == null ? null : archived.Value ? "?archived=true" : "?archived=false");

        public Task<Room[]> GetPublicRoomsAsync(bool? archived = null)
            => SendAsync<Room[]>(new HttpRequestMessage(HttpMethod.Get, EndPoint + "/v1/rooms/public" + GetArchivedQuery(archived)));

        public Task<Room[]> GetPrivateRoomsAsync(bool? archived = null)
            => SendAsync<Room[]>(new HttpRequestMessage(HttpMethod.Get, EndPoint + "/v1/rooms/private" + GetArchivedQuery(archived)));

        public Task<Room[]> GetDirectMessageRoomsAsync(bool? archived = null)
            => SendAsync<Room[]>(new HttpRequestMessage(HttpMethod.Get, EndPoint + "/v1/rooms/direct_message" + GetArchivedQuery(archived)));

        public Task<Room> PostDirectMessageRoomAsync(string targetUserProfileId)
        {
            var r = new HttpRequestMessage(HttpMethod.Post, EndPoint + $"/v1/rooms/direct_message");

            var d = new[]
            {
                new KeyValuePair<string, string>("target_user_profile_id", targetUserProfileId)
            };

            r.Content = new FormUrlEncodedContent(d);

            return SendAsync<Room>(r);
        }

        public Task<Room> PutRoomAsync(string roomId, string channelName, string description)
        {
            if (!Room.IsValidId(roomId))
            {
                return new ArgumentException($"Invalid {nameof(roomId)}.").ToTask<Room>();
            }

            var r = new HttpRequestMessage(HttpMethod.Put, EndPoint + $"/v1/rooms/" + roomId);

            var d = new[]
            {
                new KeyValuePair<string, string>("room[channel_name]", channelName),
                new KeyValuePair<string, string>("room[description]", description)
            };

            r.Content = new FormUrlEncodedContent(d);

            return SendAsync<Room>(r);
        }

        public Task<Room> ArchiveRoomAsync(string roomId)
        {
            if (!Room.IsValidId(roomId))
            {
                return new ArgumentException($"Invalid {nameof(roomId)}.").ToTask<Room>();
            }

            var r = new HttpRequestMessage(HttpMethod.Delete, EndPoint + $"/v1/rooms/" + roomId + "/archive");

            return SendAsync<Room>(r);
        }

        public Task<Room> UnarchiveRoomAsync(string roomId)
        {
            if (!Room.IsValidId(roomId))
            {
                return new ArgumentException($"Invalid {nameof(roomId)}.").ToTask<Room>();
            }

            var r = new HttpRequestMessage(HttpMethod.Put, EndPoint + $"/v1/rooms/" + roomId + "/unarchive");

            return SendAsync<Room>(r);
        }

        public Task ManageMemberAsync(string roomId, int memberId, Authority authority)
        {
            if (!Room.IsValidId(roomId))
            {
                return new ArgumentException($"Invalid {nameof(roomId)}.").ToTask<Room>();
            }

            var r = new HttpRequestMessage(HttpMethod.Put, EndPoint + $"/v1/rooms/" + roomId + "/manage_members/" + memberId);

            var d = new[]
            {
                new KeyValuePair<string, string>("authority", authority.ToApiString())
            };

            r.Content = new FormUrlEncodedContent(d);

            return SendAsync(r).ContinueWith(t => t.Result.EnsureSuccessStatusCode());
        }

        #endregion Room

        #region Message

        public Task<Message[]> GetMessagesAsync(string roomId, int? limit = null, int? beforeId = null, int? afterId = null)
        {
            if (!Room.IsValidId(roomId))
            {
                return new ArgumentException($"Invalid {nameof(roomId)}.").ToTask<Message[]>();
            }

            var u = new StringBuilder(EndPoint).Append("/v1/rooms/").Append(roomId).Append("/messages");

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

        public Task<Message> PostMessageAsync(string roomId, string message, bool isNsfw, Guid idempotentKey = default(Guid))
        {
            if (!Room.IsValidId(roomId))
            {
                return new ArgumentException($"Invalid {nameof(roomId)}.").ToTask<Message>();
            }

            var r = new HttpRequestMessage(HttpMethod.Post, EndPoint + $"/v1/rooms/" + roomId + "/messages");

            var d = new List<KeyValuePair<string, string>>(3)
            {
                new KeyValuePair<string, string>("message", message),
                new KeyValuePair<string, string>("nsfw", isNsfw ? "true":"false")
            };

            if (idempotentKey != Guid.Empty)
            {
                d.Add(new KeyValuePair<string, string>("idempotent_key", idempotentKey.ToString()));
            }

            r.Content = new FormUrlEncodedContent(d);

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

        public Task SubscribeAsync(params Room[] rooms)
            => SubscribeAsync((IEnumerable<Room>)rooms);

        public Task SubscribeAsync(IEnumerable<Room> rooms)
            => SubscribeAsync(rooms.Select(r => r.Id));

        public Task SubscribeAsync(IEnumerable<string> roomIds)
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

                    jtw2.WritePropertyName("rooms");
                    jtw2.WriteStartArray();
                    foreach (var id in roomIds)
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
                                        continue;

                                    case "confirm_subscription":
                                        _Connected?.TrySetResult(0);
                                        _Connected = null;
                                        continue;
                                }

                                try
                                {
                                    var msg = jo?.Property("message")?.Value?.Value<JObject>();
                                    switch (msg?.Property("event")?.Value?.Value<string>())
                                    {
                                        case "message_created":
                                            {
                                                var h = MessageCreated;
                                                if (h != null)
                                                {
                                                    var mo = msg.Property("data")?.Value?.ToObject<Message>();
                                                    if (mo != null)
                                                    {
                                                        h(this, new EventArgs<Message>(mo));
                                                    }
                                                }
                                            }
                                            break;

                                        case "message_updated":
                                            {
                                                var h = MessageUpdated;
                                                if (h != null)
                                                {
                                                    var mo = msg.Property("data")?.Value?.ToObject<Message>();
                                                    if (mo != null)
                                                    {
                                                        h(this, new EventArgs<Message>(mo));
                                                    }
                                                }
                                            }
                                            break;

                                        case "profile_updated":
                                            {
                                                var h = ProfileUpdated;
                                                if (h != null)
                                                {
                                                    var mo = msg.Property("data")?.Value?.ToObject<Profile>();
                                                    if (mo != null)
                                                    {
                                                        h(this, new EventArgs<Profile>(mo));
                                                    }
                                                }
                                            }
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
            Disconnected?.Invoke(this, EventArgs.Empty);
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