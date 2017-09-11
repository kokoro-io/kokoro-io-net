namespace Shipwreck.KokoroIO.SampleApp.ViewModels
{
    public abstract class PageViewModelBase : ViewModelBase
    {
        internal PageViewModelBase(MainViewModel main)
        {
            Main = main;
        }

        public MainViewModel Main { get; }

        public string TypeName => GetType().Name;
    }
}