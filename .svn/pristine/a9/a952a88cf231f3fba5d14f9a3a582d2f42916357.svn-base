using System.Collections.Generic;
using System.Linq;

namespace BayesianTest.BayesianClassificator
{
    internal class MemoryIndex : Index
    {
        internal IndexTable<string, int> table = new IndexTable<string, int>();

        public override int EntryCount
        {
            get { return (table.Values).Sum(); }
        }

        public override void Add(Entry document)
        {
            foreach (string key in document)
            {
                if (table.ContainsKey(key))
                {
                    Dictionary<string, int> dictionary;
                    string index;
                    (dictionary = table)[index = key] = dictionary[index] + 1;
                }
                else
                    table.Add(key, 1);
            }
        }

        public override int GetTokenCount(string token)
        {
            if (!table.ContainsKey(token))
                return 0;
            else
                return table[token];
        }
    }
}