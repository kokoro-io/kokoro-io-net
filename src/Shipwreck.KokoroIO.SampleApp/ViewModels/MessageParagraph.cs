using System.Collections.Generic;
using System.Xml.Linq;

namespace Shipwreck.KokoroIO.SampleApp.ViewModels
{
    public sealed class MessageParagraph : MessageBlock
    {
        internal MessageParagraph(MessageViewModel message)
            : base(message)
        {
        }

        internal MessageParagraph(MessageViewModel message, XElement element)
            : base(message)
        {
            _Spans.AddRange(MessageSpan.EnumerateSpans(element));
        }

        private readonly List<MessageSpan> _Spans = new List<MessageSpan>();

        public List<MessageSpan> Spans => _Spans;
    }
}