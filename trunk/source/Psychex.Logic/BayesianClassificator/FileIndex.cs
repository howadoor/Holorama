using System;
using System.IO;
using System.Xml.Serialization;

namespace BayesianTest.BayesianClassificator
{
    public class FileIndex : Index
    {
        private readonly string filePath;
        private readonly MemoryIndex index = new MemoryIndex();

        public FileIndex(string filePath)
        {
            if (string.IsNullOrEmpty(filePath))
                throw new ArgumentNullException("filePath");
            this.filePath = filePath;
        }

        public override int EntryCount
        {
            get { return index.EntryCount; }
        }

        public void Open()
        {
            if (!File.Exists(filePath))
                return;
            using (Stream stream = File.OpenRead(filePath))
                index.table = new XmlSerializer(typeof (IndexTable<string, int>)).Deserialize(stream) as IndexTable<string, int>;
        }

        public override void Add(Entry document)
        {
            index.Add(document);
        }

        public void Save()
        {
            using (Stream stream = File.Open(filePath, FileMode.OpenOrCreate, FileAccess.Write))
                new XmlSerializer(typeof (IndexTable<string, int>)).Serialize(stream, index.table);
        }

        public override int GetTokenCount(string token)
        {
            return index.GetTokenCount(token);
        }
    }
}