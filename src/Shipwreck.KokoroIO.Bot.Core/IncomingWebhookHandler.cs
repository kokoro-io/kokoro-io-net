using System;
using System.Collections.Generic;
using System.IO;
using System.Text;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Http;
using Newtonsoft.Json;

namespace Shipwreck.KokoroIO
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
            var valid = CallbackSecret == null
                        || (req.Headers.TryGetValue("Authorization", out var sv)
                            && sv.Count == 1
                            && sv[0] == CallbackSecret);

            var res = httpContext.Response;

            res.ContentType = "text/plain; charset=utf-8";
            res.StatusCode = valid ? 200 : 400;
            using (var w = new StreamWriter(res.Body, new UTF8Encoding(false)))
            {
                await w.WriteLineAsync(valid ? "OK" : "NG").ConfigureAwait(false);
                w.Flush();
            }
            if (valid)
            {
                try
                {
                    using (var s = new StreamReader(req.Body))
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
