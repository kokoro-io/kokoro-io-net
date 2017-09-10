using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Net.Http;
using System.Net.WebSockets;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using Newtonsoft.Json;
using Newtonsoft.Json.Linq;

namespace Shipwreck.KokoroIO
{
    public sealed class Client : ClientBase
    {
        public static new string DefaultAccessToken { get; set; }

        public Client()
        {
            AccessToken = DefaultAccessToken ?? ClientBase.DefaultAccessToken;
        }

        #region Rest API

        public Task<Profile> GetProfieAsync()
            => SendAsync<Profile>(new HttpRequestMessage(HttpMethod.Get, EndPoint + "/v1/profiles/me"));

        public Task<Room[]> GetRoomsAsync()
            => SendAsync<Room[]>(new HttpRequestMessage(HttpMethod.Get, EndPoint + "/v1/rooms"));

        public Task<Room> PostRoomAsync(string channelName, string description, RoomKind kind)
        {
            var r = new HttpRequestMessage(HttpMethod.Post, EndPoint + $"/v1/rooms");

            var d = new List<KeyValuePair<string, string>>(3)
            {
                new KeyValuePair<string, string>("room[channel_name]", channelName),
                new KeyValuePair<string, string>("room[description]", description),
                new KeyValuePair<string, string>("room[kind]",kind.ToApiString())
            };

            r.Content = new FormUrlEncodedContent(d);

            return SendAsync<Room>(r);
        }

        public Task<Room[]> GetPublicRoomsAsync()
            => SendAsync<Room[]>(new HttpRequestMessage(HttpMethod.Get, EndPoint + "/v1/rooms/public"));

        public Task<Room[]> GetPrivateRoomsAsync()
            => SendAsync<Room[]>(new HttpRequestMessage(HttpMethod.Get, EndPoint + "/v1/rooms/private"));

        public Task<Room[]> GetDirectMessageRoomsAsync()
            => SendAsync<Room[]>(new HttpRequestMessage(HttpMethod.Get, EndPoint + "/v1/rooms/direct_message"));

        public Task<Room> PutRoomAsync(string roomId, string channelName, string description)
        {
            if (!Room.IsValidId(roomId))
            {
                return Task.FromException<Room>(new ArgumentException($"Invalid {nameof(roomId)}."));
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

        public Task ManageMemberAsync(string roomId, int memberId, string authority)
        {
            if (!Room.IsValidId(roomId))
            {
                return Task.FromException<Room>(new ArgumentException($"Invalid {nameof(roomId)}."));
            }

            var r = new HttpRequestMessage(HttpMethod.Put, EndPoint + $"/v1/rooms/" + roomId + "/manage_member/" + memberId);

            var d = new[]
            {
                new KeyValuePair<string, string>("authority", authority)
            };

            r.Content = new FormUrlEncodedContent(d);

            return SendAsync(r);
        }

        public Task<Message[]> GetMessagesAsync(string roomId, int? limit = null, int? beforeId = null)
        {
            if (!Room.IsValidId(roomId))
            {
                return Task.FromException<Message[]>(new ArgumentException($"Invalid {nameof(roomId)}."));
            }

            var u = new StringBuilder(EndPoint).Append("/v1/rooms/").Append(roomId).Append("/messages");

            if (limit > 0)
            {
                u.Append("?limit=").Append(limit.Value);
            }

            if (beforeId != null)
            {
                u.Append(limit > 0 ? '&' : '?').Append("before_id=").Append(beforeId.Value);
            }

            return SendAsync<Message[]>(new HttpRequestMessage(HttpMethod.Get, u.ToString()));
        }

        public Task<Message> PostMessageAsync(string roomId, string message, bool isNsfw, Guid idempotentKey = default(Guid))
        {
            if (!Room.IsValidId(roomId))
            {
                return Task.FromException<Message>(new ArgumentException($"Invalid {nameof(roomId)}."));
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

        #endregion Rest API

        #region WebSocket API

        public string WebSocketEndPoint { get; set; } = "wss://kokoro.io/cable";

        private TaskCompletionSource<int> _Connected;

        public event EventHandler<EventArgs<Message>> MessageCreated;

        public event EventHandler<EventArgs<Message>> MessageUpdated;

        public event EventHandler<EventArgs<Profile>> ProfileUpdated;

        public event EventHandler Disconnected;

        private ClientWebSocket _WebSocket;

        private CancellationTokenSource _WebSocketCancellationTokenSource;

        public Task ConnectAsync()
        {
            _WebSocketCancellationTokenSource?.Cancel();
            if (_WebSocket != null)
            {
                switch (_WebSocket.State)
                {
                    case WebSocketState.Connecting:
                    case WebSocketState.Open:
                        return Task.CompletedTask;
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
                return Task.FromException(new InvalidOperationException());
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

                    jtw2.WritePropertyName("access_token");
                    jtw2.WriteValue(AccessToken);

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
                    return Task.FromException(new InvalidOperationException());
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
                return Task.CompletedTask;
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
                        }
                    }
                }
            }
            catch
            {
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
                    return Task.FromException(new InvalidOperationException());
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