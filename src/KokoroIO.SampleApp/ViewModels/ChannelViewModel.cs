using System.Windows.Input;

namespace KokoroIO.SampleApp.ViewModels
{
    public sealed class ChannelViewModel : ViewModelBase
    {
        private readonly MainViewModel _Main;
        private readonly Channel _Model;

        public ChannelViewModel(MainViewModel main, Channel model)
        {
            _Main = main;
            _Model = model;
        }

        public string Id => _Model.Id;

        public string ChannelName => _Model.ChannelName;

        public ChannelKind Kind => _Model.Kind;

        private bool _IsOpen;

        public bool IsOpen
        {
            get
            {
                return _IsOpen;
            }
            set
            {
                if (value != _IsOpen)
                {
                    _IsOpen = value;
                    SendPropertyChanged();
                }
            }
        }

        private int _NewMessageCount;

        public int NewMessageCount
        {
            get
            {
                return _NewMessageCount;
            }
            set
            {
                if (value != _NewMessageCount)
                {
                    _NewMessageCount = value;
                    SendPropertyChanged(nameof(NewMessageCount));
                }
            }
        }

        private Command _OpenCommand;

        public ICommand OpenCommand
            => _OpenCommand ?? (_OpenCommand = new Command(() =>
            {
                if (_Main.CurrentPage is ChannelPageViewModel rp && rp.Id == Id)
                {
                    return;
                }
                rp = new ChannelPageViewModel(_Main, _Model);
                _Main.CurrentPage = rp;
                NewMessageCount = 0;
            }));
    }
}