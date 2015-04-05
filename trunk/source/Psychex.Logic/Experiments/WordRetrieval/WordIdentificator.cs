using System;
using System.Collections.Generic;
using System.Linq;
using Psychex.Logic.Helpers;

namespace Psychex.Logic.Experiments.WordRetrieval
{
    public class WordIdentificator
    {
        private HashSet<string> candidates;
        private Dictionary<string, string> candidatesWithoutDiacritics;

        public WordIdentificator(IEnumerable<string> candidates)
        {
            this.candidates = new HashSet<string>(candidates);
            this.candidatesWithoutDiacritics = new Dictionary<string, string>(candidates.ToDictionary(c => c.RemoveDiacritics().ToLower()));
        }

        public bool TryIdentify(string word, out string identified)
        {
            if (candidates.Contains(word))
            {
                identified = word;
                return true;
            }
            identified = candidates.FirstOrDefault(c => c.Equals(word, StringComparison.InvariantCultureIgnoreCase));
            if (identified != null) return true;
            if (candidatesWithoutDiacritics.TryGetValue(word.RemoveDiacritics().ToLower(), out identified)) return true;
            // var similarity = candidates.Select(c => new KeyValuePair<string, double>(c, c.GetSimilarity(word))).OrderByDescending(kv => kv.Value).ToArray();
            return false;
        }

    }
}