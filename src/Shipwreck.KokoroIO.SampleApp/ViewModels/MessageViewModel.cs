using System;

namespace Shipwreck.KokoroIO.SampleApp.ViewModels
{
    public sealed class MessageViewModel : ViewModelBase
    {
        private readonly RoomPageViewModel _Page;
        private readonly Message _Model;

        internal MessageViewModel(RoomPageViewModel page, Message model)
        {
            _Page = page;
            _Model = model;
        }

        public string Avatar => _Model.Avatar;

        public string DisplayName => _Model.DisplayName;

        public DateTime PublishedAt => _Model.PublishedAt;

        public string Content => _Model.Content;
    }
}