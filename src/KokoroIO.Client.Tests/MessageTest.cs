using System.IO;
using System.Runtime.Serialization;
using System.Xml;
using System.Xml.Linq;
using Newtonsoft.Json;
using Newtonsoft.Json.Linq;
using Xunit;

namespace KokoroIO
{
    public class MessageTest
    {
        #region DataContractSerializer Test

        [Fact]
        public void DataContractSerializer_SerializationTest()
        {
            var m = new Message()
            {
                HtmlContent = "hoge"
            };

            using (var sw = new StringWriter())
            {
                using (var xw = XmlWriter.Create(sw, new XmlWriterSettings()
                {
                    CloseOutput = false
                }))
                {
                    new DataContractSerializer(typeof(Message)).WriteObject(xw, m);
                }
                var xml = sw.ToString();

                var xd = XDocument.Parse(xml);
                var NS = xd.Root.Name.Namespace.ToString();
#pragma warning disable 618
                Assert.Null(xd.Root.Element(XName.Get(nameof(m.Content), NS)));
#pragma warning restore 618
                Assert.NotNull(xd.Root.Element(XName.Get(nameof(m.HtmlContent), NS)));
            }
        }

        #endregion DataContractSerializer Test

        #region Json.NET Test

        [Fact]
        public void JsonSerializer_SerialzationTest()
        {
            var m = new Message()
            {
                HtmlContent = "hoge"
            };
            var json = JsonConvert.SerializeObject(m);
            var jo = JObject.Parse(json);
            Assert.Null(jo["content"]?.Value<string>());
            Assert.Equal(m.HtmlContent, jo["html_content"].Value<string>());
        }

        [Fact]
        public void JsonSerializer_DeserializationTest_Content()
        {
            var json = "{\"content\":\"fuga\"}";
            var m = JsonConvert.DeserializeObject<Message>(json);
            Assert.Equal("fuga", m.HtmlContent);
        }

        [Fact]
        public void JsonSerializer_DeserializationTest_HtmlContent()
        {
            var json = "{\"html_content\":\"fuga\"}";
            var m = JsonConvert.DeserializeObject<Message>(json);
            Assert.Equal("fuga", m.HtmlContent);
        }

        #endregion Json.NET Test
    }
}