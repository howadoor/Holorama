using System;
using System.IO;
using System.Text;
using System.Collections.Generic;
using System.Linq;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using Textant.Logic.Tests.Helpers;

namespace Textant.Logic.Tests.Tests
{
    [TestClass]
    public class CharactersOccurencyTest
    {
        private static OccurencyCounter<char> characterOccurency;
        private Dictionary<int, OccurencyCounter<string>> ngramOccurency = new Dictionary<int, OccurencyCounter<string>>();

        [TestMethod]
        public void CharacterOccurencyTest()
        {
            var occurency = CharacterOccurency;
            foreach (var occ in occurency)
            {
                string character = occ.Key.ToString();
                if (occ.Key == ' ') character = "' '";
                if (occ.Key == '\n') character = @"\n";
                Console.WriteLine("{0}\t{3}\t{1}\t{2:0.##}", character, occ.Value, occurency.GetPpm(occ.Key), (int) occ.Key);
            }
        }

        public static OccurencyCounter<char> CharacterOccurency
        {
            get
            {
                if (characterOccurency == null)
                {
                    var filename = CharacterOccurencyFile;
                    if (File.Exists(filename)) characterOccurency = OccurencyCounterTools.ReadFromXml<char>(filename);
                    else
                    {
                        characterOccurency = new OccurencyCounter<char>();
                        foreach (var text in Texts.NormalizedTexts)
                        {
                            characterOccurency.Add(text);
                        }
                        characterOccurency.WriteToXml(filename);
                    }
                }
                return characterOccurency;
            }
        }
        
        private static string CharacterOccurencyFile
        {
            get { return Path.Combine(Texts.SourceTextsDirectory, "overall_characters.occx"); }
        }

        [TestMethod]
        public void NgramsOccurencyTest()
        {
            for (int n = 2; n < 8; n++)
            {
                Console.WriteLine("{0}-gram occurency", n);
                var occurency = GetNgramOccurency(n);
                foreach (var occ in occurency.Take(512))
                {
                    var key = occ.Key.Replace("' '", "' '").Replace("\n", @"\n");
                    Console.WriteLine("{0}\t{1}\t{2:0.##}", key, occ.Value, occurency.GetPpm(occ.Key));
                }
                Console.WriteLine("-----------------------------------------------");
            }
        }

        private OccurencyCounter<string> GetNgramOccurency(int n)
        {
            OccurencyCounter<string> occurency;
            if (!ngramOccurency.TryGetValue(n, out occurency))
            {
                var filename = GetNgramOccurencyFile(n);
                if (File.Exists(filename)) characterOccurency = OccurencyCounterTools.ReadFromXml<char>(filename);
                else
                {
                    occurency = new OccurencyCounter<string>();
                    foreach (var text in Texts.NormalizedTexts)
                    {
                        occurency.Add(GetNGrams(n, text));
                    }
                    occurency.WriteToXml(filename);
                    ngramOccurency[n] = occurency;
                }
            }
            return occurency;
        }

        private IEnumerable<string> GetNGrams(int n, string text)
        {
            for (int i = 0; i < text.Length - n; i++)
            {
                var ngram = text.Substring(i, n);
                if (!ngram.Contains('\x0') && !ngram.Contains('\x1a') && !ngram.Contains('\x1b') && !ngram.Contains('\x01')) yield return ngram;
            }
        }

        private string GetNgramOccurencyFile(int n)
        {
            return Path.Combine(Texts.SourceTextsDirectory, string.Format("{0}-gram_overall.occx", n)); ;
        }
    }
}
