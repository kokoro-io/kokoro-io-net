using System;
using System.Collections.Generic;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Documents;
using KokoroIO.SampleApp.ViewModels;

namespace KokoroIO.SampleApp.Views
{
    public sealed class MessageSpanControl : TextBlock
    {
        static MessageSpanControl()
        {
            DefaultStyleKeyProperty.OverrideMetadata(typeof(MessageSpanControl), new FrameworkPropertyMetadata(typeof(MessageSpanControl)));
        }

        public MessageSpanControl()
        {
            Loaded += MessageSpanControl_Loaded;
            DataContextChanged += MessageSpanControl_DataContextChanged;
        }

        private void MessageSpanControl_DataContextChanged(object sender, System.Windows.DependencyPropertyChangedEventArgs e)
        {
            BuildTextBlock();
        }

        private void MessageSpanControl_Loaded(object sender, System.Windows.RoutedEventArgs e)
        {
            BuildTextBlock();
        }

        private void BuildTextBlock()
        {
            Inlines.Clear();

            var spans = DataContext as IEnumerable<MessageSpan>;

            if (spans != null)
            {
                foreach (var sp in spans)
                {
                    if (sp.Text == Environment.NewLine)
                    {
                        Inlines.Add(new LineBreak());
                    }
                    else
                    {
                        Inlines.Add(new Run()
                        {
                            Text = sp.Text
                        });
                    }
                }
            }
        }
    }
}