using System.Collections.Generic;
using System.Collections.ObjectModel;

namespace Shipwreck.KokoroIO.SampleApp.ViewModels
{
    public sealed class MainViewModel : ViewModelBase
    {
        internal MainViewModel(Client client)
        {
            Client = client;
        }

        internal Client Client { get; }

        #region Room

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
                var rs = await Client.GetRoomsAsync();
                foreach (var r in rs)
                {
                    var vm = new RoomViewModel(this, r);
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

                await Client.ConnectAsync();
            }
            catch
            {
            }
        }

        private void UpdateCurrentRoom()
        {
            if (_PublicRooms == null)
            {
                return;
            }
            var cp = _CurrentPage as RoomPageViewModel;

            foreach (var r in _PublicRooms)
            {
                r.IsOpen = r.Id == cp?.Id;
            }
            foreach (var r in _PrivateRooms)
            {
                r.IsOpen = r.Id == cp?.Id;
            }
            foreach (var r in _DirectMessages)
            {
                r.IsOpen = r.Id == cp?.Id;
            }
        }

        #endregion Room

        private PageViewModelBase _CurrentPage;

        public PageViewModelBase CurrentPage
        {
            get
            {
                return _CurrentPage;
            }
            internal set
            {
                if (value != _CurrentPage)
                {
                    _CurrentPage = value;
                    SendPropertyChanged(nameof(CurrentPage));
                    UpdateCurrentRoom();
                }
            }
        }
    }
}