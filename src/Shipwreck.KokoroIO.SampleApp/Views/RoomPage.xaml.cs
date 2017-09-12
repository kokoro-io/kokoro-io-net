using System;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Media;
using Shipwreck.KokoroIO.SampleApp.ViewModels;

namespace Shipwreck.KokoroIO.SampleApp.Views
{
    /// <summary>
    /// RoomPage.xaml の相互作用ロジック
    /// </summary>
    public partial class RoomPage : UserControl
    {
        public RoomPage()
        {
            InitializeComponent();
        }

        private void messageViewer_ScrollChanged(object sender, ScrollChangedEventArgs e)
        {
            var vm = DataContext as RoomPageViewModel;

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