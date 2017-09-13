using System.IO;
using System.Linq;
using System.Net.Http;
using System.Threading.Tasks;
using Newtonsoft.Json;

namespace Shipwreck.KokoroIO
{
    public static class HttpRequestMessageExtensions
    {
        public static async Task<Message> GetMessageAsync(this HttpRequestMessage request, string callbackSecret)
        {
            var valid = callbackSecret == null
                        || (request.Headers.TryGetValues("Authorization", out var sv)
                            && sv.Count() == 1
                            && sv.First() == callbackSecret);

            if (valid)
            {
                try
                {
                    using (var s = new StreamReader(await request.Content.ReadAsStreamAsync().ConfigureAwait(false)))
                    using (var jr = new JsonTextReader(s))
                    {
                        return new JsonSerializer().Deserialize<Message>(jr);
                    }
                }
                catch { }
            }

            return null;
        }
    }
}