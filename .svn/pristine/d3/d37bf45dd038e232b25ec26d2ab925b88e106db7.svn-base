using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using Textant.Logic.Helpers;

namespace Textant.Logic.Tests.Helpers
{
    public class Texts
    {
        public const string SourceTextsDirectory = @"d:\projects\textant\documents\texts";

        public static string[] SourceTextFiles
        {
            get { return Directory.GetFiles(SourceTextsDirectory, "*.txt", SearchOption.AllDirectories); }
        }

        public static IEnumerable<string> NormalizedTextFiles
        {
            get { return SourceTextFiles.Select(GetNormalizedTextFile); }
        }

        public static IEnumerable<string> NormalizedTexts
        {
            get { return NormalizedTextFiles.Select(filename => TextFileTools.ReadTextFile(filename, Encoding.UTF8)); }
        }

        public static string GetNormalizedTextFile(string sourceTextFile)
        {
            var normalizedFile = sourceTextFile + ".normalized";
            if (!File.Exists(normalizedFile)) NormalizeTextFile(sourceTextFile, normalizedFile);
            return normalizedFile;
        }

        private static void NormalizeTextFile(string sourceFilename, string targetFilename)
        {
            var parser = new TextParser();
            var normalizedContent = parser.ParseCharacters(TextFileTools.ReadTextFile(sourceFilename)).ToArray();
            using (var fs = new FileStream(targetFilename, FileMode.Create, FileAccess.Write, FileShare.None))
            {
                using (var writer = new StreamWriter(fs, Encoding.UTF8))
                {
                    writer.Write(normalizedContent);                    
                }
            }
        }
    }
}