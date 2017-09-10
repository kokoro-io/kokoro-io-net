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

        public Task<Message> PostMessageAsync(string roomId, string message, string displayName = null, bool? isNsfw = null)
        {
            if (!Room.IsValidId(roomId))
            {
                return new ArgumentException($"Invalid {nameof(roomId)}.").ToTask<Message>();
            }

            var r = new HttpRequestMessage(HttpMethod.Post, EndPoint + $"/v1/bot/rooms/" + roomId + "/messages");

            var d = new List<KeyValuePair<string, string>>(3)
            {
                new KeyValuePair<string, string>("message", message),
            };

            if (displayName != null)
            {
                d.Add(new KeyValuePair<string, string>("display_name", displayName));
            }

            if (isNsfw != null)
            {
                d.Add(new KeyValuePair<string, string>("nsfw", isNsfw.Value ? "true" : "false"));
            }

            r.Content = new FormUrlEncodedContent(d);

            return SendAsync<Message>(r);
        }
    }
}