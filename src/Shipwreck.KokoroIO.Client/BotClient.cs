using System;
using System.Collections.Generic;
using System.Net.Http;
using System.Threading.Tasks;

namespace Shipwreck.KokoroIO
{
    public sealed class BotClient : ClientBase
    {
        public static new string DefaultAccessToken { get; set; }

        public BotClient()
        {
            AccessToken = DefaultAccessToken ?? ClientBase.DefaultAccessToken;
        }

        public Task<Message> PostMessageAsync(string roomId, string message, bool isNsfw)
        {
            if (!Room.IsValidId(roomId))
            {
                return Task.FromException<Message>(new ArgumentException($"Invalid {nameof(roomId)}."));
            }

            var r = new HttpRequestMessage(HttpMethod.Post, EndPoint + $"/v1/bot/rooms" + roomId + "/message");

            r.Content = new FormUrlEncodedContent(new[]
            {
                new KeyValuePair<string,string>("message", message),
                new KeyValuePair<string,string>("nsfw", isNsfw ? "true":"false")
            });


            return SendAsync<Message>(r);
        }
    }
}