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


    public sealed class MessageSpan
    {
        public string Text { get; set; }

        public static IEnumerable<MessageSpan> EnumerateSpans(XElement root)
        {
            foreach (var d in root.DescendantNodes())
            {
                if (d.NodeType == XmlNodeType.Text)
                {
                    var t = ((XText)d).Value;

                    yield return new MessageSpan()
                    {
                        Text = t,
                    };
                }
                else if (d.NodeType == XmlNodeType.Element)
                {
                    if ("br".Equals((d as XElement)?.Name.LocalName))
                    {
                        yield return new MessageSpan()
                        {
                            Text = Environment.NewLine,
                        };
                    }
                }
            }
        }
    }

}