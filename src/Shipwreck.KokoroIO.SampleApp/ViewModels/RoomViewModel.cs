namespace Shipwreck.KokoroIO.SampleApp.ViewModels
{
    public sealed class RoomViewModel : ViewModelBase
    {
        private readonly Room _Model;

        public RoomViewModel(Room model)
        {
            _Model = model;
        }

        public string ChannelName => _Model.ChannelName;
    }
}