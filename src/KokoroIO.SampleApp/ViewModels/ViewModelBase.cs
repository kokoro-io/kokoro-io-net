using System.ComponentModel;
using System.Runtime.CompilerServices;

namespace KokoroIO.SampleApp.ViewModels
{
    public abstract class ViewModelBase : INotifyPropertyChanged
    {
        protected void SendPropertyChanged([CallerMemberName]string propertyName = null)
        {
            PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(propertyName));
        }

        public event PropertyChangedEventHandler PropertyChanged;
    }
}