using System.Windows;
using System.Windows.Controls;

namespace KokoroIO.SampleApp.Views
{
    public sealed class MessageBlockControl : Control
    {
        static MessageBlockControl()
        {
            DefaultStyleKeyProperty.OverrideMetadata(typeof(MessageBlockControl), new FrameworkPropertyMetadata(typeof(MessageBlockControl)));
        }
    }
}