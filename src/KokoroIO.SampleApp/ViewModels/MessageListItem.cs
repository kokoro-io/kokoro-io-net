using System;
using System.Linq;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.IO;
using System.Text.RegularExpressions;
using System.Xml;
using System.Xml.Linq;

namespace KokoroIO.SampleApp.ViewModels
{

    public sealed class MessageListItem : ViewModelBase
    {
        private readonly List<MessageSpan> _Spans = new List<MessageSpan>();

        public List<MessageSpan> Spans => _Spans;
    }

}