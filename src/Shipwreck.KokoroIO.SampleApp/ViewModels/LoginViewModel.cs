using System.Linq;
using System.Threading.Tasks;
using Shipwreck.KokoroIO.SampleApp.Properties;
using Shipwreck.KokoroIO.SampleApp.Views;

namespace Shipwreck.KokoroIO.SampleApp.ViewModels
{
    public sealed class LoginViewModel : ViewModelBase
    {
        private string _MailAddress = Settings.Default.MailAddress ?? string.Empty;

        public string MailAddress
        {
            get
            {
                return _MailAddress;
            }
            set
            {
                var v = value ?? string.Empty;
                if (v != _MailAddress)
                {
                    _MailAddress = v;
                    SendPropertyChanged();
                }
            }
        }

        public async Task<Client> BeginLogin(string password)
        {
            var preseve = false;
            var c = new Client();
            try
            {
                var ns = GetType().Assembly.GetName().Name;

                var tokens = await c.GetAccessTokensAsync(_MailAddress, password);
                var tk = tokens.FirstOrDefault(t => t.Name == ns) ?? tokens.FirstOrDefault();

                if (tk == null)
                {
                    tk = await c.PostAccessTokenAsync(_MailAddress, password, ns);
                }

                c.AccessToken = tk.Token;
                preseve = true;
                return c;


                //var lw = App.Current.MainWindow;

                //var mw = new MainWindow();
                //mw.Show();

                //lw.Close();
            }
            finally
            {
                if (!preseve)
                {
                    c.Dispose();
                }
            }
        }
    }
}