using System.IO;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using Psychex.Logic.Experiments.WordRetrieval;

namespace Psychex.Logic.Tests.Tests.WordRetrieval
{
    [TestClass]
    public class ModalityTest
    {
        [TestMethod] 
        public void TestModalityLoading()
        {
            LoadModality(0);
        }

        public static Modality LoadModality(int modalityIndex)
        {
            var modality = ModalityEx.Load(Path.Combine(WordsByThemesTest.PsychexPath, "sources"), modalityIndex);
            Assert.IsNotNull(modality);
            Assert.IsNotNull(modality.Words);
            Assert.IsNotNull(modality.Positions);
            Assert.IsNotNull(modality.Questions);
            Assert.IsTrue(modality.Words.UsedWords.Length == 128);
            Assert.IsTrue(modality.Positions.Count == 128);
            Assert.IsTrue(modality.Questions.Length == 20);
            return modality;
        }
    }
}