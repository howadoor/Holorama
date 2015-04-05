using System.Collections.Generic;
using System.Xml;
using System.Xml.Serialization;
using System.Xml.Schema;

namespace BayesianTest.BayesianClassificator
{
    [XmlRoot("index")]
    public class IndexTable<TKey, TValue> : Dictionary<TKey, TValue>, IXmlSerializable
    {
        #region IXmlSerializable Members

        public XmlSchema GetSchema()
        {
            return null;
        }

        public void ReadXml(XmlReader reader)
        {
            var xmlSerializer1 = new XmlSerializer(typeof (TKey));
            var xmlSerializer2 = new XmlSerializer(typeof (TValue));
            bool isEmptyElement = reader.IsEmptyElement;
            reader.Read();
            if (isEmptyElement)
                return;
            while (reader.NodeType != XmlNodeType.EndElement)
            {
                reader.ReadStartElement("entry");
                reader.ReadStartElement("word");
                var key = (TKey) xmlSerializer1.Deserialize(reader);
                reader.ReadEndElement();
                reader.ReadStartElement("count");
                var obj = (TValue) xmlSerializer2.Deserialize(reader);
                reader.ReadEndElement();
                Add(key, obj);
                reader.ReadEndElement();
                var num = (int) reader.MoveToContent();
            }
            reader.ReadEndElement();
        }

        public void WriteXml(XmlWriter writer)
        {
            var xmlSerializer1 = new XmlSerializer(typeof (TKey));
            var xmlSerializer2 = new XmlSerializer(typeof (TValue));
            foreach (TKey index in Keys)
            {
                writer.WriteStartElement("entry");
                writer.WriteStartElement("word");
                xmlSerializer1.Serialize(writer, index);
                writer.WriteEndElement();
                writer.WriteStartElement("count");
                TValue obj = this[index];
                xmlSerializer2.Serialize(writer, obj);
                writer.WriteEndElement();
                writer.WriteEndElement();
            }
        }

        #endregion
    }
}