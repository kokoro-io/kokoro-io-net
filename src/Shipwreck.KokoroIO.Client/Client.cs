using System;
using System.Collections.Generic;
using System.Net.Http;
using System.Text;
using System.Threading.Tasks;

namespace Shipwreck.KokoroIO
{
    public sealed class Client : ClientBase
    {
        public static new string DefaultAccessToken { get; set; }

        public Client()
        {
            AccessToken = DefaultAccessToken ?? ClientBase.DefaultAccessToken;
        }

        public Task<Profile> GetProfieAsync()
            => SendAsync<Profile>(new HttpRequestMessage(HttpMethod.Get, EndPoint + "/v1/profiles/me"));

        public Task<Room[]> GetRoomsAsync()
            => SendAsync<Room[]>(new HttpRequestMessage(HttpMethod.Get, EndPoint + "/v1/rooms"));

        // TODO: POST /v1/rooms

        public Task<Room[]> GetPublicRoomsAsync()
            => SendAsync<Room[]>(new HttpRequestMessage(HttpMethod.Get, EndPoint + "/v1/rooms/public"));

        public Task<Room[]> GetPrivateRoomsAsync()
            => SendAsync<Room[]>(new HttpRequestMessage(HttpMethod.Get, EndPoint + "/v1/rooms/private"));

        public Task<Room[]> GetDirectMessageRoomsAsync()
            => SendAsync<Room[]>(new HttpRequestMessage(HttpMethod.Get, EndPoint + "/v1/rooms/direct_message"));

        // TODO: /v1/rooms/{room_id}

        // TODO: /v1/rooms/{room_id}/manage_members/{member_id}

        public Task<Message[]> GetMessages(string roomId, int? limit = null, int? beforeId = null)
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

            var r = new HttpRequestMessage(HttpMethod.Post, EndPoint + $"/v1/rooms" + roomId + "/message");

            var d = new List<KeyValuePair<string, string>>(3)
            {
                new KeyValuePair<string, string>("message", message),
                new KeyValuePair<string, string>("nsfw", isNsfw ? "true":"false")
            };

            if (idempotentKey != Guid.Empty)
            {
                d.Add(new KeyValuePair<string, string>("idempotent_key", idempotentKey.ToString()));
            }

            var fc = new FormUrlEncodedContent(d);

            return SendAsync<Message>(r);
        }
    }
}