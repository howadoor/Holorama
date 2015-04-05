using System;
using System.IO;
using System.Linq;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using Psychex.Logic.Experiments.WordRetrieval;

namespace Psychex.Logic.Tests.Tests.WordRetrieval
{
    [TestClass]
    public class WordsByThemesTest
    {
        private static WordsByThemes wordsByThemes;
        public const string PsychexPath = "c:\\projects\\psychex\\";
        public const string SourceFilename = "psychex.txt";

        public static string SourceFile { get { return Path.Combine(PsychexPath, SourceFilename); } }
        
        [TestMethod]
        public void LoadingTest()
        {
            LoadWordsByThemes();
        }

        public static WordsByThemes LoadWordsByThemes()
        {
            if (wordsByThemes == null)
            {
                wordsByThemes = WordsByThemes.Load(SourceFile);
                Assert.IsNotNull(wordsByThemes);
                Assert.IsTrue(wordsByThemes.Count > 0);
                Assert.IsTrue(wordsByThemes.TotalCount > 0);
                Assert.IsTrue(wordsByThemes.Values.All(themeList => themeList.Count > 0));
                Console.WriteLine("{0} words loaded", wordsByThemes.TotalCount);
            }
            return wordsByThemes;
        }
    }
}
