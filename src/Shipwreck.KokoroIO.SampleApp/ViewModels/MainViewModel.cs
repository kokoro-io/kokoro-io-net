using System.Collections.ObjectModel;

namespace Shipwreck.KokoroIO.SampleApp.ViewModels
{
    public sealed class MainViewModel : ViewModelBase
    {
        private readonly Client _Client;

        internal MainViewModel(Client client)
        {
            _Client = client;
        }

        private ObservableCollection<RoomViewModel> _PublicRooms;
        private ObservableCollection<RoomViewModel> _PrivateRooms;
        private ObservableCollection<RoomViewModel> _DirectMessages;

        public ObservableCollection<RoomViewModel> PublicRooms
            => InitRooms()._PublicRooms;

        public ObservableCollection<RoomViewModel> PrivateRooms
            => InitRooms()._PrivateRooms;

        public ObservableCollection<RoomViewModel> DirectMessages
            => InitRooms()._DirectMessages;

        private MainViewModel InitRooms()
        {
            if (_PublicRooms == null)
            {
                _PublicRooms = new ObservableCollection<RoomViewModel>();
                _PrivateRooms = new ObservableCollection<RoomViewModel>();
                _DirectMessages = new ObservableCollection<RoomViewModel>();

                LoadRooms();
            }
            return this;
        }

        private async void LoadRooms()
        {
            try
            {
                var rs = await _Client.GetRoomsAsync();
                foreach (var r in rs)
                {
                    var vm = new RoomViewModel(r);
                    if (r.Kind == RoomKind.PublicChannel)
                    {
                        _PublicRooms.Add(vm);
                    }
                    if (r.Kind == RoomKind.PrivateChannel)
                    {
                        _PrivateRooms.Add(vm);
                    }
                    if (r.Kind == RoomKind.DirectMessage)
                    {
                        _DirectMessages.Add(vm);
                    }
                }

                await _Client.ConnectAsync();
            }
            catch
            {
            }
        }
    }
}