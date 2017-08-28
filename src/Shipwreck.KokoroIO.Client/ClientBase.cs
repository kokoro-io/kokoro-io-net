using System;
using System.IO;
using System.Net.Http;
using System.Threading;
using System.Threading.Tasks;
using Newtonsoft.Json;

namespace Shipwreck.KokoroIO
{
    public abstract class ClientBase : IDisposable
    {
        private HttpClient _HttpClient;

        private HttpClient HttpClient
        {
            get
            {
                if (_HttpClient == null)
                {
                    var h = new HttpClientHandler();
                    _HttpClient = new HttpClient(h);
                }
                return _HttpClient;
            }
        }

        public static string DefaultAccessToken { get; set; }

        public string EndPoint { get; set; } = "https://kokoro.io/api";

        public string AccessToken { get; set; } = DefaultAccessToken;

        protected async Task<T> SendAsync<T>(HttpRequestMessage request, CancellationToken cancellationToken = default(CancellationToken))
        {
            request.Headers.Add("X-Access-Token", AccessToken);

            var res = await HttpClient.SendAsync(request, cancellationToken).ConfigureAwait(false);

            res.EnsureSuccessStatusCode();

            using (var s = await res.Content.ReadAsStreamAsync().ConfigureAwait(false))
            using (var sr = new StreamReader(s))
            using (var jr = new JsonTextReader(sr))
            {
                return new JsonSerializer().Deserialize<T>(jr);
            }
        }

        #region Diposableパターン

        /// <summary>
        /// インスタンスが破棄されているかどうかを示す値を取得します。
        /// </summary>
        protected bool IsDisposed { get; private set; }

        /// <summary>
        /// アンマネージ リソースの解放およびリセットに関連付けられているアプリケーション定義のタスクを実行します。
        /// </summary>
        public void Dispose()
        {
            Dispose(true);
            GC.SuppressFinalize(this);
        }

        /// <summary>
        /// オブジェクトがガベジ コレクションにより収集される前に、そのオブジェクトがリソースを解放し、その他のクリーンアップ操作を実行できるようにします。
        /// </summary>
        ~ClientBase()
        {
            Dispose(false);
        }

        /// <summary>
        /// アンマネージ リソースの解放およびリセットに関連付けられているアプリケーション定義のタスクを実行します。
        /// </summary>
        /// <param name="disposing">メソッドが<see cref="Dispose()" />から呼び出された場合は<c>true</c>。その他の場合は<c>false</c>。</param>
        protected virtual void Dispose(bool disposing)
        {
            if (IsDisposed)
            {
                return;
            }

            IsDisposed = true;

            if (disposing)
            {
                _HttpClient?.Dispose();
            }
            _HttpClient = null;
        }

        #endregion Diposableパターン
    }
}