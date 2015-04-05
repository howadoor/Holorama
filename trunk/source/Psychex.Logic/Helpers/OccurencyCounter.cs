using System.Collections.Generic;

namespace Psychex.Logic.Helpers
{
    public class OccurencyCounter<TItem> : Dictionary<TItem, int>
    {
        private int total = 0;

        public int Total
        {
            get { return total; }
        }

        #region Items adding

        public void Add(TItem item)
        {
            int count;
            if (!TryGetValue(item, out count))
            {
                this[item] = 1;
                total = Total + 1;
                return;
            }
            this[item] = count + 1;
            total = Total + 1;
        }

        public void Add(TItem item, int countAddition)
        {
            int count;
            if (!TryGetValue(item, out count))
            {
                this[item] = countAddition;
                total = Total + countAddition;
                return;
            }
            this[item] = count + countAddition;
            total = Total + countAddition;
        }

        public void Add(IEnumerable<TItem> items)
        {
            foreach (var item in items) Add(item);
        }

        public void Add(IEnumerable<KeyValuePair<TItem, int>> occurencies)
        {
            foreach (var occurency in occurencies) Add(occurency.Key, occurency.Value);
        }

        #endregion

        public long GetOccurency(TItem item)
        {
            int occurency;
            return TryGetValue(item, out occurency) ? occurency : 0;
        }

        public double GetPpm(TItem item)
        {
            return 1000000.0 * GetOccurency(item) / Total;
        }
    }
}