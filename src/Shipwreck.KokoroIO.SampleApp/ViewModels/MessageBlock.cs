namespace Shipwreck.KokoroIO.SampleApp.ViewModels
{
    public abstract class MessageBlock : ViewModelBase
    {
        internal MessageBlock(MessageViewModel message)
        {
            Message = message;
        }

        public string TypeName => GetType().Name;

        protected MessageViewModel Message { get; }
    }
}