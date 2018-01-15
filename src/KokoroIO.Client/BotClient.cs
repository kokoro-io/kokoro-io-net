using System;
using System.Collections.Generic;
using System.Net.Http;
using System.Threading.Tasks;

namespace KokoroIO
{
    public sealed class BotClient : ClientBase
    {
        public static new string DefaultAccessToken { get; set; }

        public BotClient()
        {
            AccessToken = DefaultAccessToken ?? ClientBase.DefaultAccessToken;
        }

        public BotClient(HttpClient httpClient)
            : base(httpClient)
        {
            AccessToken = DefaultAccessToken ?? ClientBase.DefaultAccessToken;
        }

        public Task<Message> PostMessageAsync(string channelId, string message, string displayName = null, bool? isNsfw = null, bool? expandEmbedContents = null)
        {
            if (!Channel.IsValidId(channelId))
            {
                return new ArgumentException($"Invalid {nameof(channelId)}.").ToTask<Message>();
            }

            var r = new HttpRequestMessage(HttpMethod.Post, EndPoint + $"/v1/bot/channels/" + channelId + "/messages");

            var d = new List<KeyValuePair<string, string>>(4)
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

            if (expandEmbedContents != null)
            {
                d.Add(new KeyValuePair<string, string>("expand_embed_contents", expandEmbedContents.Value ? "true" : "false"));
            }

            r.Content = new FormUrlEncodedContent(d);

            return SendAsync<Message>(r);
        }
    }
}