using System.Windows;
using KokoroIO.SampleApp.Properties;
using KokoroIO.SampleApp.ViewModels;

namespace KokoroIO.SampleApp.Views
{
    /// <summary>
    /// LoginWindow.xaml の相互作用ロジック
    /// </summary>
    public partial class LoginWindow : Window
    {
        public LoginWindow()
        {
            InitializeComponent();

            passwordBox.Password = Settings.Default.Password;
        }

        private async void Button_Click(object sender, RoutedEventArgs e)
        {
            try
            {
                var vm = (LoginViewModel)DataContext;
                var c = await vm.BeginLogin(passwordBox.Password);

                var mw = new MainWindow();
                mw.DataContext = new MainViewModel(c);
                mw.Show();

                var sd = Settings.Default;
                sd.MailAddress = vm.MailAddress;
                sd.Password = passwordBox.Password;
                sd.Save();

                Close();
            }
            catch
            {
                MessageBox.Show(this, "ログインに失敗しました。");
            }
        }
    }
}