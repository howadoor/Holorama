using System.Collections.Generic;

namespace BayesianTest.BayesianClassificator
{
    public abstract class Index
    {
        public abstract int EntryCount { get; }

        public virtual void Add(IEnumerable<Entry> documents)
        {
            foreach (var entry in documents) Add(entry);
        }

        public abstract void Add(Entry document);

        public abstract int GetTokenCount(string token);

        public static Index CreateMemoryIndex()
        {
            return new MemoryIndex();
        }
    }
}