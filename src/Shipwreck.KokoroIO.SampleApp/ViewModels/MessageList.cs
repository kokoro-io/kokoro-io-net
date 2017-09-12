using System;
using System.Linq;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.IO;
using System.Text.RegularExpressions;
using System.Xml;
using System.Xml.Linq;

namespace Shipwreck.KokoroIO.SampleApp.ViewModels
{


    public sealed class MessageList : MessageBlock
    {
        internal MessageList(MessageViewModel message, XElement element)
            : base(message)
        {
            _Items = new List<MessageListItem>();

            foreach (var li in element.Elements("li"))
            {
                var ivm = new MessageListItem();

                ivm.Spans.AddRange(MessageSpan.EnumerateSpans(li));

                _Items.Add(ivm);
            }
        }

        private readonly List<MessageListItem> _Items;

        public List<MessageListItem> Items => _Items;
    }

}