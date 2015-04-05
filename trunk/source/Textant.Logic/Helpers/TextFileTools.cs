using System;
using System.IO;
using System.Text;

namespace Textant.Logic.Helpers
{
    public class TextFileTools
    {
        public static string ReadTextFile(string filename, Encoding encoding)
        {
            using (var fs = new FileStream(filename, FileMode.Open, FileAccess.Read, FileShare.ReadWrite))
            {
                var reader = new StreamReader(fs, encoding);
                return reader.ReadToEnd();
            }
        }

        /// <summary>
        /// Reads text from file <see cref="filename"/>.
        /// </summary>
        /// <param name="filename"></param>
        /// <returns></returns>
        public static string ReadTextFile(string filename)
        {
            var encoding = GetEncodingOfFile(filename);
            if (encoding == null) throw new InvalidOperationException(String.Format("Cannot detect encoding of text in file {0}", filename));
            return ReadTextFile(filename, encoding);
        }

        /// <summary>
        /// Determines encoding of text in file <see cref="filename"/>.
        /// </summary>
        /// <param name="filename"></param>
        /// <returns></returns>
        public static Encoding GetEncodingOfFile(string filename)
        {
            var sample = GetByteSample(filename);
            var encoding = new EncodingDetector().DetectEncoding(sample);
            return encoding;
        }

        /// <summary>
        /// Returns filename where computed occurency for file <see cref="filename"/> is stored
        /// </summary>
        /// <param name="filename"></param>
        /// <returns></returns>
        public static string GetOccurencyFilename(string filename)
        {
            return filename + ".occx";
        }

        private static byte[] GetByteSample(string fileName)
        {
            using (var file = File.OpenRead(fileName))
            {
                using (var reader = new BinaryReader(file))
                {
                    return reader.ReadBytes(16 * 1024);
                }
            }
        }
    }
}