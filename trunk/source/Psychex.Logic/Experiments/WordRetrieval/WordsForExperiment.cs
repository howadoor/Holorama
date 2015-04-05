using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.Serialization;

namespace Psychex.Logic.Experiments.WordRetrieval
{
    [DataContract]
    public class WordsForExperiment
    {
        [DataMember]
        public string[] UsedWords { get; private set; }
        
        [DataMember]
        public string[] NotUsedWords { get; private set; }

        protected WordsForExperiment(string [] usedWords, string [] notUsedWords)
        {
            UsedWords = usedWords;
            NotUsedWords = notUsedWords;
        }

        public static WordsForExperiment Create(WordsByThemes wordsByThemes, int count)
        {
            var random = new Random();
            var used = new List<string>();
            var lists = wordsByThemes.Values.ToArray();
            for (; used.Count < count;)
            {
                var list = lists[random.Next(lists.Length)];
                var word = list[random.Next(list.Count)];
                if (!used.Contains(word)) used.Add(word);
            }
            var notUsed = new List<string>();
            foreach (var word in wordsByThemes.AllWords )
            {
                if (!used.Contains(word) && !notUsed.Contains(word)) notUsed.Add(word);
            }
            return new WordsForExperiment(used.ToArray(), notUsed.ToArray());
        }
    }
}