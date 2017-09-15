using System;
using System.Linq;
using System.Net.NetworkInformation;
using System.Security.Cryptography;
using System.Text;
using System.Threading.Tasks;
using Shipwreck.KokoroIO.SampleApp.Properties;

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

                var hash = Encoding.Default.GetBytes(ns).Concat(NetworkInterface.GetAllNetworkInterfaces().Where(ni => ni.OperationalStatus == OperationalStatus.Up).OrderBy(ni => ni.Name).SelectMany(ni => ni.GetPhysicalAddress().GetAddressBytes())).ToArray();

                hash = HashAlgorithm.Create().ComputeHash(hash);

                var di = Convert.ToBase64String(hash);
                var dev = await c.PostDeviceAsync(_MailAddress, password, $"{Environment.MachineName} ({ns})", DeviceKind.Unknown, di);

                c.AccessToken = dev.AccessToken.Token;
                preseve = true;
                return c;
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