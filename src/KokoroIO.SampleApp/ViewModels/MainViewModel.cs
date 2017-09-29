using System;
using System.Collections.ObjectModel;
using System.IO;
using System.Linq;
using System.Windows.Media;

namespace KokoroIO.SampleApp.ViewModels
{
    public sealed class MainViewModel : ViewModelBase
    {
        internal MainViewModel(Client client)
        {
            Client = client;

            Client.MessageCreated += Client_MessageCreated;
        }

        private void Client_MessageCreated(object sender, EventArgs<Message> e)
        {
            try
            {
                var rid = e.Data.Channel.Id; ;
                for (var i = 0; i < 3; i++)
                {
                    var r = i == 0 ? _PublicChannels : i == 1 ? _PrivateChannels : _DirectMessages;

                    if (r != null)
                    {
                        var rvm = r.FirstOrDefault(rm => rm.Id == rid);

                        if (rvm != null)
                        {
                            rvm.NewMessageCount++;

                            App.Current.Dispatcher.BeginInvoke((Action)(() =>
                            {
                                var mp = new MediaPlayer();
                                mp.Open(new Uri(Path.Combine(Path.GetDirectoryName(GetType().Assembly.Location), "Resources", "notify.mp3")));
                                mp.Play();
                            }));
                            return;
                        }
                    }
                }
            }
            catch { }
        }

        internal Client Client { get; }

        #region Channel

        private ObservableCollection<ChannelViewModel> _PublicChannels;
        private ObservableCollection<ChannelViewModel> _PrivateChannels;
        private ObservableCollection<ChannelViewModel> _DirectMessages;

        public ObservableCollection<ChannelViewModel> PublicChannels
            => InitChannels()._PublicChannels;

        public ObservableCollection<ChannelViewModel> PrivateChannels
            => InitChannels()._PrivateChannels;

        public ObservableCollection<ChannelViewModel> DirectMessages
            => InitChannels()._DirectMessages;

        private MainViewModel InitChannels()
        {
            if (_PublicChannels == null)
            {
                _PublicChannels = new ObservableCollection<ChannelViewModel>();
                _PrivateChannels = new ObservableCollection<ChannelViewModel>();
                _DirectMessages = new ObservableCollection<ChannelViewModel>();

                LoadChannels();
            }
            return this;
        }

        private async void LoadChannels()
        {
            try
            {
                var rs = await Client.GetMembershipsAsync();
                foreach (var ms in rs)
                {
                    var c = ms.Channel;
                    var vm = new ChannelViewModel(this, c);
                    if (c.Kind == ChannelKind.PublicChannel)
                    {
                        _PublicChannels.Add(vm);
                    }
                    if (c.Kind == ChannelKind.PrivateChannel)
                    {
                        _PrivateChannels.Add(vm);
                    }
                    if (c.Kind == ChannelKind.DirectMessage)
                    {
                        _DirectMessages.Add(vm);
                    }
                }

                if (rs.Any())
                {
                    await Client.ConnectAsync();

                    await Client.SubscribeAsync(rs.Select(r => r.Channel));
                }
            }
            catch
            {
            }
        }

        private void UpdateCurrentChannel()
        {
            if (_PublicChannels == null)
            {
                return;
            }
            var cp = _CurrentPage as ChannelPageViewModel;

            foreach (var r in _PublicChannels)
            {
                r.IsOpen = r.Id == cp?.Id;
            }
            foreach (var r in _PrivateChannels)
            {
                r.IsOpen = r.Id == cp?.Id;
            }
            foreach (var r in _DirectMessages)
            {
                r.IsOpen = r.Id == cp?.Id;
            }
        }

        #endregion Channel

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
                    UpdateCurrentChannel();
                }
            }
        }
    }
}