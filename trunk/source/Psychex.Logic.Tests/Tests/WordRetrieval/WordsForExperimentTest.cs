using System.Linq;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using Psychex.Logic.Experiments.WordRetrieval;

namespace Psychex.Logic.Tests.Tests.WordRetrieval
{
    [TestClass]
    public class WordsForExperimentTest
    {
        [TestMethod]
        public void CreationTest()
        {
            CreateFromLoadedWords();
        }

        public static WordsForExperiment CreateFromLoadedWords(int wordsForTestCount = 10)
        {
            var wordsByThemes = WordsByThemesTest.LoadWordsByThemes();
            var wordsForExperiment = CreateFrom(wordsByThemes, wordsForTestCount);
            return wordsForExperiment;
        }

        public static WordsForExperiment CreateFrom(WordsByThemes wordsByThemes, int wordsForTestCount = 10)
        {
            Assert.IsNotNull(wordsByThemes);
            Assert.IsTrue(wordsByThemes.Count > 0);
            var wordsForExperiment = WordsForExperiment.Create(wordsByThemes, wordsForTestCount);
            Assert.IsNotNull(wordsForExperiment);
            Assert.IsNotNull(wordsForExperiment.UsedWords);
            Assert.IsNotNull(wordsForExperiment.NotUsedWords);
            Assert.IsTrue(wordsForExperiment.UsedWords.Length == wordsForTestCount);
            Assert.IsTrue(wordsForExperiment.NotUsedWords.Length > 0);
            Assert.IsFalse(wordsForExperiment.UsedWords.Any(w => wordsForExperiment.NotUsedWords.Contains(w)));
            return wordsForExperiment;
        }
    }
}
