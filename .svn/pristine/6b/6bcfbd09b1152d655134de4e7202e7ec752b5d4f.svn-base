using System;
using System.IO;
using System.Text;
using System.Collections.Generic;
using System.Linq;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using Textant.Logic.Classification;
using Textant.Logic.Helpers;
using Textant.Logic.Tests.Helpers;

namespace Textant.Logic.Tests.Tests
{
    [TestClass]
    public class EncodingDetectorTest
    {
        [TestMethod]
        public void BasicTests()
        {
            var detector = new EncodingDetector();
            var wordCounter = new OccurencyCounter<string>();
            var alphabetCounter = new OccurencyCounter<char>();
            var filesCount = 0;
            var wordCountingTools = new WordOccurencyTools();
            foreach (var wordsInFile in Texts.SourceTextFiles.Where(wordCountingTools.CanLoadTextFromFile).Select(wordCountingTools.GetWordOccurencyInFile))
            {
                wordCounter.Add(wordsInFile);
                filesCount++;
            }
            wordCounter.WriteToXml(OverallOccurencyFilename);
            Console.WriteLine("Analyzed {0:### ### ###} documents with {1:### ### ###} words, found {2:### ### ###} unique words.", filesCount, wordCounter.Total, wordCounter.Count);
            WriteOccurenciesMagnitudes(wordCounter);
            WriteToConsole(wordCounter);
            return;
            /*
            foreach (var file in files)
            {
                var sample = GetByteSample(file);
                var encoding = detector.DetectEncoding(sample);
                // Console.WriteLine("{0} => {1}", file, encoding);
                if (encoding != null)
                {
                    var text = ReadText(file, encoding);
                    var textWordCounter = new OccurencyCounter<string>();
                    textWordCounter.Add(new TextParser().ParseWords(text));
                    var thematic = GetThematicWords(textWordCounter, wordCounter).Take(16).ToArray();
                    var stringBuilder = new StringBuilder();
                    stringBuilder.AppendFormat("\"{0}\"\t", Path.GetFileName(file));
                    foreach (var word in thematic) stringBuilder.AppendFormat("{0} ", word);
                    Console.WriteLine(stringBuilder.ToString());

                }
            }
            */


            // WriteToConsole(wordCounter);
            /*
            foreach (var word in wordCounter.Where(f => f.Value > 5).Select(f => f.Key))
            {
                alphabetCounter.Add(word);
            }
            var ordinarinessies = wordCounter.Where(f => f.Value > 5).Select(f => f.Key).Select(word => new Tuple<string, double>(word, alphabetCounter.GetOrdinariness(word))).OrderByDescending(o => o.Item2).ToArray();
            foreach (var ordinarinessy in ordinarinessies.Take(64))
            {
                Console.WriteLine("{0}\t{1}", ordinarinessy.Item1, ordinarinessy.Item2);
            }*/
        }

        [TestMethod]
        public void ScramblingTest()
        {
            var wordCounter = OccurencyCounterTools.ReadFromXml<string>(OverallOccurencyFilename);
            var canonicalizedWords = new Dictionary<string, List<string>>();
            foreach (var word in wordCounter.Keys)
            {
                var canonicalizedWord = ScrambleAndCanonicalize(word);
                List<string> list;
                if (!canonicalizedWords.TryGetValue(canonicalizedWord, out list))
                {
                    canonicalizedWords[canonicalizedWord] = (list = new List<string>());
                }
                list.Add(word);
            }
            Console.WriteLine("{0} words scrambled and canonicalized to {1} variants ({2:#0.00} %)", wordCounter.Count, canonicalizedWords.Count, 100.0 * canonicalizedWords.Count / wordCounter.Count);
            File.WriteAllLines(@"c:\canonicalized_scrambling_CZ.txt", GetCanonicalizedScramblingLines(canonicalizedWords), Encoding.UTF8);
        }

        private IEnumerable<string> GetCanonicalizedScramblingLines(Dictionary<string, List<string>> canonicalizedWords)
        {
            return canonicalizedWords.Keys.OrderByDescending(w => w).Select(cv => string.Format("{0}\t{2}\t{1}", cv, Joint(canonicalizedWords[cv]), canonicalizedWords[cv].Count));
        }

        private string Joint(List<string> canonicalizedWord)
        {
            canonicalizedWord.Sort();
            var builder = new StringBuilder();
            foreach (var cv in canonicalizedWord)
            {
                if (builder.Length > 0) builder.Append(" ");
                builder.Append(cv);
            }
            return builder.ToString();
        }

        private string ScrambleAndCanonicalize(string word)
        {
            return new string(GetScrambledAndCanonicalizedCharacters(word).ToArray());
        }

        private IEnumerable<char> GetScrambledAndCanonicalizedCharacters(string word)
        {
            var length = word.Length;
            if (length > 0) yield return word[0];
            if (length > 2)
            {
                foreach (var @char in word.Substring(1, length - 2).OrderBy(c => c))
                {
                    yield return @char;
                }
            }
            if (length > 1) yield return word[length - 1];
        }

        [TestMethod]
        public void AuthorSimilarityTest()
        {
            const int minPpm = 1000;
            using (var target = new StreamWriter(Path.Combine(Texts.SourceTextsDirectory, string.Format("similarity [minPpm {0}].txt", minPpm)), false, Encoding.GetEncoding(1250)))
            {
                var overallOccurency = OccurencyCounterTools.ReadFromXml<string>(OverallOccurencyFilename);
                var overallTestVector = overallOccurency.OrderByDescending(occ => occ.Value).Where(occ => overallOccurency.GetPpm(occ.Key) >= minPpm).Select(occ => new KeyValuePair<string, double>(occ.Key, overallOccurency.GetPpm(occ.Key))).ToArray();
                foreach (var occ in overallTestVector)
                {
                    target.Write("[{0}, {1}] ", occ.Key, (int) occ.Value);
                }
                target.WriteLine();
                var testVectors = new Dictionary<string, double[]>();
                var wordCountingTools = new WordOccurencyTools();
                foreach (var textFile in Texts.SourceTextFiles.Where(wordCountingTools.ExistsStoredOccurencyForFile))
                {
                    var textFileOccurency = wordCountingTools.GetWordOccurencyInFile(textFile);
                    testVectors[Path.GetFileNameWithoutExtension(textFile)] = overallTestVector.Select(occ => textFileOccurency.GetPpm(occ.Key)).ToArray();
                }
                foreach (var testVector in testVectors)
                {
                    var mostSimilar = GetVectorSimilarities(testVector.Value, testVectors).OrderBy(s => s.Value).Take(10).ToArray();
                    target.WriteLine("* {0}", testVector.Key);
                    foreach (var similar in mostSimilar)
                    {
                        target.WriteLine("\t{0}\t{1}", (int) similar.Value, similar.Key);
                    };
                }
            }
        }

        [TestMethod]
        public void KohonenTest()
        {
            var overallOccurency = OccurencyCounterTools.ReadFromXml<string>(OverallOccurencyFilename);
            var overallTestVector = overallOccurency.OrderByDescending(occ => occ.Value).Where(occ => overallOccurency.GetPpm(occ.Key) >= 1000.0).Select(occ => new KeyValuePair<string, double>(occ.Key, overallOccurency.GetPpm(occ.Key))).ToArray();
            var testVectors = new Dictionary<string, double[]>();
            var wordCountingTools = new WordOccurencyTools();
            foreach (var textFile in Texts.SourceTextFiles.Where(wordCountingTools.ExistsStoredOccurencyForFile).OrderBy(f => f.GetHashCode()).Take(2048))
            {
                var textFileOccurency = wordCountingTools.GetWordOccurencyInFile(textFile);
                testVectors[Path.GetFileNameWithoutExtension(textFile)] = overallTestVector.Select(occ => textFileOccurency.GetPpm(occ.Key)).ToArray();
            }
            var kohonenMap = new KohonenMap();
            kohonenMap.CalculateMap(testVectors);
        }

        [TestMethod]
        public void DiwordsTest()
        {
            var overallOccurency = OccurencyCounterTools.ReadFromXml<string>(OverallOccurencyFilename);
            var overallTestVector = overallOccurency.OrderByDescending(occ => occ.Value).Where(occ => overallOccurency.GetPpm(occ.Key) >= 1000.0).Select(occ => occ.Key).ToLookup(key => key);
            var wordCountingTools = new WordOccurencyTools();
            var diwordOccurency = new OccurencyCounter<string>();
            foreach (var textFileWords in Texts.SourceTextFiles.Where(wordCountingTools.CanLoadTextFromFile).Select(file => new TextParser().ParseWords(TextFileTools.ReadTextFile(file))))
            {
                string lastWord = null;
                foreach (var word in textFileWords)
                {
                    bool isTestedWord = overallTestVector.Contains(word);
                    if (lastWord != null && isTestedWord)
                    {
                        diwordOccurency.Add(string.Format("{0} {1}", lastWord, word));
                    }
                    lastWord = isTestedWord ? word : null;
                }
            }
            diwordOccurency.WriteToXml(DiwordOccurencyFilename);
        }

        private IEnumerable<KeyValuePair<string, double>> GetVectorSimilarities(double[] testVector, Dictionary<string, double[]> testVectors)
        {
            foreach (var candidate in testVectors.Where(v => v.Value != testVector))
            {
                var vector = candidate.Value;
                yield return new KeyValuePair<string, double>(candidate.Key, testVector.Select((t, i) => Math.Abs(t - vector[i])).Sum());
            }
        }

        protected string OverallOccurencyFilename
        {
            get { return Path.Combine(Texts.SourceTextsDirectory, @".all_texts.occx"); }
        }

        protected string DiwordOccurencyFilename
        {
            get { return Path.Combine(Texts.SourceTextsDirectory, @"diwords.all_texts.occx"); }
        }

        private IEnumerable<string> GetThematicWords(OccurencyCounter<string> textWordCounter, OccurencyCounter<string> wordCounter)
        {
            return textWordCounter.OrderByDescending(twc => ((double)twc.Value * (double)twc.Value / textWordCounter.Total - (double)wordCounter.GetOccurency(twc.Key) * (double)wordCounter.GetOccurency(twc.Key) / wordCounter.Total) * wordCounter.GetOccurency(twc.Key) * wordCounter.GetOccurency(twc.Key)).Select(twc => twc.Key);
        }

        private void WriteOccurenciesMagnitudes<TItem>(OccurencyCounter<TItem> wordCounter)
        {
            for (int minOccurency = 1;; minOccurency *= 2)
            {
                var occurency = wordCounter.Count(f => f.Value >= minOccurency);
                if (occurency == 0) break;
                var words = wordCounter.Where(f => f.Value >= minOccurency && f.Value < minOccurency*2).OrderBy(w => w.GetHashCode()).Select(f => f.Key).Take(10).OrderBy(w => w).ToArray();
                var stringBuilder = new StringBuilder();
                stringBuilder.AppendFormat("{0}\t{1}\t{2:0.00} %\t", minOccurency, occurency, 100.0 * occurency / wordCounter.Count);
                foreach (var word in words) stringBuilder.AppendFormat("{0} ", word);
                Console.WriteLine(stringBuilder.ToString());
            }
        }

        private void WriteToConsole<TItem>(OccurencyCounter<TItem> frequencyCounter)
        {
            var freqs = frequencyCounter.ToArray().OrderByDescending(f => f.Value).Take(2048).ToArray();
            foreach (var freq in freqs)
            {
                Console.WriteLine("{0}\t{1}\t{2:0} ppm", freq.Key, freq.Value, freq.Value *1000000.0 / (double)frequencyCounter.Total);
            }
            Console.WriteLine("Total {0}, items {1}", frequencyCounter.Total, frequencyCounter.Count);
        }

        private string ReadText(string file, Encoding encoding)
        {
            using (StreamReader streamReader = new StreamReader(file, encoding))
            {
                return streamReader.ReadToEnd();
            }
        }

        private byte[] GetByteSample(string fileName)
        {
            using (var file = File.OpenRead(fileName))
            {
                using (var reader = new BinaryReader(file))
                {
                    return reader.ReadBytes(16*1024);
                }
            }
        }
    }
}
