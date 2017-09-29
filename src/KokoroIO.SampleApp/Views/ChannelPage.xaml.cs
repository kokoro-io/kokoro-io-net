using System;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Media;
using KokoroIO.SampleApp.ViewModels;

namespace KokoroIO.SampleApp.Views
{
    /// <summary>
    /// ChannelPage.xaml の相互作用ロジック
    /// </summary>
    public partial class ChannelPage : UserControl
    {
        public ChannelPage()
        {
            InitializeComponent();
        }

        private void messageViewer_ScrollChanged(object sender, ScrollChangedEventArgs e)
        {
            var vm = DataContext as ChannelPageViewModel;

            if (vm?.IsLoading != false)
            {
                return;
            }

            var hb = header.TranslatePoint(new Point(0, header.ActualHeight), messageViewer).Y;
            var ft = footer.TranslatePoint(new Point(0, 0), messageViewer).Y;

            if (header.IsVisible && hb > 4)
            {
                vm?.BeginPrepend();
            }
        }

        private void Border_IsVisibleChanged(object sender, DependencyPropertyChangedEventArgs e)
        {
            if (!(sender as UIElement).IsVisible)
            {
                if (messageViewer.ScrollableHeight > messageViewer.ActualHeight)
                {
                    var hb = header.TranslatePoint(new Point(0, header.ActualHeight), messageViewer).Y;

                    if (header.IsVisible && hb > 0)
                    {
                        messageViewer.ScrollToVerticalOffset(header.TranslatePoint(new Point(0, header.ActualHeight), messagesPanel).Y);
                    }
                }
            }
        }
    }
}