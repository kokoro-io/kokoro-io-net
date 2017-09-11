using System.Windows.Input;

namespace Shipwreck.KokoroIO.SampleApp.ViewModels
{
    public sealed class RoomViewModel : ViewModelBase
    {
        private readonly MainViewModel _Main;
        private readonly Room _Model;

        public RoomViewModel(MainViewModel main, Room model)
        {
            _Main = main;
            _Model = model;
        }

        public string Id => _Model.Id;

        public string ChannelName => _Model.ChannelName;

        public RoomKind Kind => _Model.Kind;

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

        private Command _OpenCommand;

        public ICommand OpenCommand
            => _OpenCommand ?? (_OpenCommand = new Command(() =>
            {
                if (_Main.CurrentPage is RoomPageViewModel rp && rp.Id == Id)
                {
                    return;
                }
                rp = new RoomPageViewModel(_Main, _Model);
                _Main.CurrentPage = rp;
            }));
    }
}