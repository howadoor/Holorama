using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Globalization;
using System.Linq;
using System.Runtime.Serialization;
using System.Text;
using System.Xml;
using System.Xml.Serialization;

namespace Textant.Logic
{
    /// <summary>
    /// Methods related to <see cref="OccurencyCounter{T}"/>
    /// </summary>
    public static class OccurencyCounterTools
    {
        public static double GetOrdinariness<TItem>(this OccurencyCounter<TItem> occurencies, IEnumerable<TItem> sequency)
        {
            var counter = 0;
            long summ = 0;
            foreach (var item in sequency)
            {
                counter++;
                summ += occurencies.GetOccurency(item);
            }
            return (double) summ/counter;
        }

        #region XML serialization

        public static void WriteToXml<TItem>(this OccurencyCounter<TItem> occurencies, XmlWriter target)
        {
            var serializer = new DataContractSerializer(typeof(TItem));
            target.WriteAttributeString("count", occurencies.Count.ToString(CultureInfo.InvariantCulture));
            target.WriteAttributeString("total", occurencies.Total.ToString(CultureInfo.InvariantCulture));
            int i = 0;
            foreach (var item in occurencies.OrderByDescending(item => item.Value))
            {
                i++;
                target.WriteStartElement("i");
                target.WriteAttributeString("i", i.ToString(CultureInfo.InvariantCulture));
                target.WriteAttributeString("count", item.Value.ToString(CultureInfo.InvariantCulture));
                target.WriteAttributeString("ppm", ((int) (1000000.0 * item.Value / occurencies.Total + 0.5)).ToString(CultureInfo.InvariantCulture));
                serializer.WriteObjectContent(target, item.Key);
                target.WriteEndElement();
            }
        }

        public static void WriteToXml<TItem>(this OccurencyCounter<TItem> occurencies, string filename)
        {
            using (var target = XmlWriter.Create(filename, new XmlWriterSettings{Encoding = Encoding.UTF8, Indent = true}))
            {
                target.WriteStartDocument();
                target.WriteStartElement("occurencies");
                occurencies.WriteToXml(target);
                target.WriteEndElement();
                target.WriteEndDocument();
            }
        }

        public static void ReadFromXml<TItem>(this OccurencyCounter<TItem> occurencies, XmlReader source)
        {
            var serializer = new DataContractSerializer(typeof(TItem));
            for (; source.ReadToFollowing("i"); )
            {
                int count = int.Parse(source.GetAttribute("count"));
                var key = (TItem) serializer.ReadObject(source, false);
                occurencies.Add(key, count);
            }
        }


        public static OccurencyCounter<TItem> ReadFromXml<TItem>(string filename)
        {
            using (var source = XmlReader.Create(filename))
            {
                if (!source.ReadToFollowing("occurencies")) throw new InvalidOperationException(string.Format("Cannot find <occurencies> root tag in file {0}", filename));
                var occurencies = new OccurencyCounter<TItem>();
                occurencies.ReadFromXml(source);
                return occurencies;
            }
        }

        #endregion
    }
}