using System;
using System.Collections.Generic;
using System.IO;
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

        public event EventHandler Connected;

        public event EventHandler Ping;

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
            _WebSocket = new ClientWebSocket();
            _WebSocketCancellationTokenSource = new CancellationTokenSource();
            _WebSocket.Options.SetRequestHeader("X-Access-Token", AccessToken);

            var r = _WebSocket.ConnectAsync(new Uri(WebSocketEndPoint), _WebSocketCancellationTokenSource.Token);
            r.ContinueWith(t =>
            {
                if (t.Status == TaskStatus.RanToCompletion)
                {
                    ReceiveCore();
                }
            });
            return r;
        }

        private async void ReceiveCore()
        {
            var ws = _WebSocket;
            var ct = _WebSocketCancellationTokenSource?.Token ?? default(CancellationToken);

            if (ws?.State == WebSocketState.Open)
            {
                using (var ms = new MemoryStream())
                using (var sw = new StreamWriter(ms, Encoding.UTF8, 128, true))
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

                    if (!ms.TryGetBuffer(out var b))
                    {
                        throw new Exception();
                    }

                    await ws.SendAsync(b, WebSocketMessageType.Text, true, ct).ConfigureAwait(false);
                }

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
                                    Connected?.Invoke(this, EventArgs.Empty);
                                    continue;

                                case "ping":
                                    Ping?.Invoke(this, EventArgs.Empty);
                                    continue;
                            }
                        }
                    }
                }
            }
        }

        #endregion WebSocket API

        protected override void Dispose(bool disposing)
        {
            try
            {
                _WebSocket?.Dispose();
                _WebSocket = null;
                _WebSocketCancellationTokenSource?.Cancel();
                _WebSocketCancellationTokenSource = null;
            }
            catch { }
            base.Dispose(disposing);
        }
    }
}