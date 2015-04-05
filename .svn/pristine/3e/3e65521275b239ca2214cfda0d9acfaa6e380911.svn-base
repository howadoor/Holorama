using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;

namespace Psychex.Logic.Experiments.WordRetrieval
{
    public class WordsByThemes : Dictionary<string, List<string>>
    {
        public long TotalCount
        {
            get { return AllWords.Count(); }
        }

        public IEnumerable<string> AllWords
        {
            get { return this.Values.SelectMany(l => l).Distinct(); }
        }

        public static WordsByThemes Load(string filename)
         {
             var wordsByThemes = new WordsByThemes();
             var currentTheme = "Not specified";
             using (var fs = new FileStream(filename, FileMode.Open, FileAccess.Read, FileShare.ReadWrite))
             {
                 var reader = new StreamReader(fs, Encoding.UTF8);
                 string line;
                 while ((line = reader.ReadLine()) != null)
                 {
                     if (line.StartsWith("-"))
                     {
                         currentTheme = line.Trim(' ', '\r', '\n', '-');
                         continue;
                     }
                     var words = line.Split(' ', ',', '\t').Select(s => s.Trim()).Where(s => !string.IsNullOrWhiteSpace(s)).ToList();
                     if (words.Count > 0)
                     {
                         if (wordsByThemes.ContainsKey(currentTheme)) throw new InvalidDataException("Theme is repeated");
                         wordsByThemes[currentTheme] = words;
                     }
                 }
             }
             return wordsByThemes;
         }

        public string GetThemeOfWord(string word)
        {
            return this.First(t => t.Value.Contains(word)).Key;
        }
    }
}