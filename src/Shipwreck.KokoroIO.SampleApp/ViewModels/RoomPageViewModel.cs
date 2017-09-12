using System.Collections.ObjectModel;
using System.Linq;

namespace Shipwreck.KokoroIO.SampleApp.ViewModels
{
    public sealed class RoomPageViewModel : PageViewModelBase
    {
        private readonly Room _Model;

        internal RoomPageViewModel(MainViewModel main, Room model)
            : base(main)
        {
            _Model = model;
        }

        public string Id => _Model.Id;

        public string ChannelName => _Model.ChannelName;

        public RoomKind Kind => _Model.Kind;

        private ObservableCollection<MessageViewModel> _Messages;

        public ObservableCollection<MessageViewModel> Messages
        {
            get
            {
                if (_Messages == null)
                {
                    _Messages = new ObservableCollection<MessageViewModel>();
                    BeginLoadMessages();
                }
                return _Messages;
            }
        }

        #region IsLoading

        private bool _IsLoading;

        public bool IsLoading
        {
            get
            {
                return _IsLoading;
            }
            private set
            {
                if (value != _IsLoading)
                {
                    _IsLoading = value;
                    SendPropertyChanged();
                }
            }
        }

        #endregion IsLoading

        #region HasPrevious

        private bool _HasPrevious = true;

        public bool HasPrevious
        {
            get
            {
                return _HasPrevious;
            }
            private set
            {
                if (value != _HasPrevious)
                {
                    _HasPrevious = value;
                    SendPropertyChanged();
                }
            }
        }

        #endregion HasPrevious

        public void BeginPrepend()
            => BeginLoadMessages(true);

        public void BeginAppend()
            => BeginLoadMessages(false);

        private async void BeginLoadMessages(bool prepend = false)
        {
            if (IsLoading)
            {
                return;
            }
            try
            {
                IsLoading = true;

                const int PAGE_SIZE = 60;

                int? bid, aid;

                if (Messages.Count == 0)
                {
                    aid = bid = null;
                }
                else if (prepend)
                {
                    if (!HasPrevious)
                    {
                        IsLoading = false;
                        return;
                    }
                    bid = _Messages.First().Id;
                    aid = null;
                }
                else
                {
                    bid = null;
                    aid = _Messages.Last().Id;
                }

                var messages = await Main.Client.GetMessagesAsync(_Model.Id, PAGE_SIZE, beforeId: bid, afterId: aid);

                HasPrevious &= aid != null || messages.Length >= PAGE_SIZE;

                var i = 0;

                foreach (var m in messages.OrderBy(e => e.Id))
                {
                    var vm = new MessageViewModel(this, m);
                    for (; ; i++)
                    {
                        var prev = i == 0 ? null : _Messages[i - 1];
                        var next = i >= _Messages.Count ? null : _Messages[i];

                        if (!(prev?.Id > vm.Id)
                            || !(vm.Id >= next?.Id))
                        {
                            _Messages.Insert(i, vm);
                            i++;

                            vm.SetIsMerged(prev);
                            next?.SetIsMerged(vm);
                            break;
                        }
                    }
                }
            }
            catch { }
            finally
            {
                IsLoading = false;
            }
        }
    }
}