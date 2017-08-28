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

        #region Diposable�p�^�[��

        /// <summary>
        /// �C���X�^���X���j������Ă��邩�ǂ����������l���擾���܂��B
        /// </summary>
        protected bool IsDisposed { get; private set; }

        /// <summary>
        /// �A���}�l�[�W ���\�[�X�̉������у��Z�b�g�Ɋ֘A�t�����Ă���A�v���P�[�V������`�̃^�X�N�����s���܂��B
        /// </summary>
        public void Dispose()
        {
            Dispose(true);
            GC.SuppressFinalize(this);
        }

        /// <summary>
        /// �I�u�W�F�N�g���K�x�W �R���N�V�����ɂ����W�����O�ɁA���̃I�u�W�F�N�g�����\�[�X��������A���̑��̃N���[���A�b�v��������s�ł���悤�ɂ��܂��B
        /// </summary>
        ~ClientBase()
        {
            Dispose(false);
        }

        /// <summary>
        /// �A���}�l�[�W ���\�[�X�̉������у��Z�b�g�Ɋ֘A�t�����Ă���A�v���P�[�V������`�̃^�X�N�����s���܂��B
        /// </summary>
        /// <param name="disposing">���\�b�h��<see cref="Dispose()" />����Ăяo���ꂽ�ꍇ��<c>true</c>�B���̑��̏ꍇ��<c>false</c>�B</param>
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

        #endregion Diposable�p�^�[��
    }
}