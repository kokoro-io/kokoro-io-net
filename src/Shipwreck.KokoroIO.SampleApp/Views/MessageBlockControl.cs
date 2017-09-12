using System.Windows;
using System.Windows.Controls;

namespace Shipwreck.KokoroIO.SampleApp.Views
{
    public sealed class MessageBlockControl : Control
    {
        static MessageBlockControl()
        {
            DefaultStyleKeyProperty.OverrideMetadata(typeof(MessageBlockControl), new FrameworkPropertyMetadata(typeof(MessageBlockControl)));
        }
    }
}