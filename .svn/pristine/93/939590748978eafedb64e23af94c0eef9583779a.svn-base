using System;
using System.IO;
using System.Text;
using Textant.Logic.Helpers;

namespace Textant.Logic
{
    /// <summary>
    /// Static methods related to counting of word occurency
    /// </summary>
    public class WordOccurencyTools
    {
        /// <summary>
        /// Returns word occurency for text stored in file <see cref="filename"/>. If stored occurency exists, loads it from file,
        /// otherwise loads content of text file, computes occurency and stores it to occurency file.
        /// </summary>
        /// <param name="filename"></param>
        /// <returns></returns>
        public OccurencyCounter<string> GetWordOccurencyInFile(string filename)
        {
            var occurencyFilename = TextFileTools.GetOccurencyFilename(filename);
            if (File.Exists(occurencyFilename)) return OccurencyCounterTools.ReadFromXml<string>(occurencyFilename);
            var occurency = ComputeWordOccurencyInFile(filename);
            occurency.WriteToXml(occurencyFilename);
            return occurency;
        }

        /// <summary>
        /// Computes word occurency from text stored in file <see cref="filename"/>. Always loads text from file and computes new occurency statistics.
        /// </summary>
        /// <param name="filename"></param>
        /// <returns></returns>
        public OccurencyCounter<string> ComputeWordOccurencyInFile(string filename)
        {
            var text = TextFileTools.ReadTextFile(filename);
            return ComputeWordOccurency(text);
        }

        /// <summary>
        /// Computes word occurency in <see cref="text"/>.
        /// </summary>
        /// <param name="text"></param>
        /// <returns></returns>
        public OccurencyCounter<string> ComputeWordOccurency(string text)
        {
            var wordCounter = new OccurencyCounter<string> {new TextParser().ParseWords(text)};
            return wordCounter;
        }

        /// <summary>
        /// Checks if stored word occurency statistics exists for text file <see cref="filename"/>.
        /// </summary>
        /// <param name="filename"></param>
        /// <returns></returns>
        public bool ExistsStoredOccurencyForFile(string filename)
        {
            return File.Exists(TextFileTools.GetOccurencyFilename(filename));
        }

        private string ReadText(string file, Encoding encoding)
        {
            using (StreamReader streamReader = new StreamReader(file, encoding))
            {
                return streamReader.ReadToEnd();
            }
        }

        public bool CanLoadTextFromFile(string filename)
        {
            return TextFileTools.GetEncodingOfFile(filename) != null;
        }
    }
}