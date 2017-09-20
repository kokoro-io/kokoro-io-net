using System;
using System.IO;
using System.Threading.Tasks;
using System.Web;
using Newtonsoft.Json;

namespace KokoroIO
{
    public sealed class IncomingWebhookHandler
    {
        private readonly Action<Message> _Callback;

        public IncomingWebhookHandler(Action<Message> callback)
        {
            _Callback = callback;
        }

        public string CallbackSecret { get; set; }

        public async Task HandleAsync(HttpContext httpContext)
        {
            var req = httpContext.Request;
            var sv = req.Headers.Get("Authorization");
            var valid = CallbackSecret == null || sv == CallbackSecret;

            var res = httpContext.Response;

            res.ContentType = "text/plain; charset=utf-8";
            res.StatusCode = valid ? 200 : 400;
            await res.Output.WriteLineAsync(valid ? "OK" : "NG").ConfigureAwait(false);
            res.Output.Flush();

            if (valid)
            {
                try
                {
                    using (var s = new StreamReader(req.InputStream))
                    using (var jr = new JsonTextReader(s))
                    {
                        _Callback(new JsonSerializer().Deserialize<Message>(jr));
                    }
                }
                catch { }
            }
        }
    }
}