using System.Collections.Generic;
using System.Drawing;
using System.IO;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using Psychex.Logic.Experiments.WordRetrieval;
using Psychex.Logic.Helpers;

namespace Psychex.Logic.Tests.Tests.WordRetrieval
{
    [TestClass]
    public class ImageCreatorTest
    {
        private const int wordsCount = 128;

        [TestMethod]
        public void ImageCreationTest()
        {
            return;
            var imageCreator = new ImageCreator();
            var words = CreateWords(wordsCount);
            WordPositions wordPositions;
            var bitmap = imageCreator.CreateImage(words, new Size(960, 640), new Size(4, 4), out wordPositions);
            Assert.IsNotNull(bitmap);
            Assert.IsNotNull(wordPositions);
            Assert.IsTrue(wordPositions.Count == wordsCount);
            bitmap.Save(Path.Combine(WordsByThemesTest.PsychexPath, "psychex.png"));
            XmlSerialization.SaveToXml(wordPositions, Path.Combine(WordsByThemesTest.PsychexPath, "psychex.xml"));
            var deserialized = XmlSerialization.LoadFromXml<WordPositions>(Path.Combine(WordsByThemesTest.PsychexPath, "psychex.xml"));
            Assert.IsNotNull(deserialized);
            Assert.IsTrue(deserialized.GetType() == typeof(WordPositions));
            Assert.IsTrue(deserialized.Count == wordsCount);
        }

        [TestMethod]
        public void ImageCreationTest2()
        {
            return;
            var imageCreator = new ImageCreator();
            var targetFolder = Path.Combine(WordsByThemesTest.PsychexPath, "sources");
            if (!Directory.Exists(targetFolder)) Directory.CreateDirectory(targetFolder);
            foreach (var modalityImage in Directory.GetFiles(targetFolder, "modality_*"))
            {
                File.Delete(modalityImage);
            }
            var wordsByThemes = WordsByThemesTest.LoadWordsByThemes();
            for (int i = 0; i < 1024; i++)
            {
                var words = WordsForExperimentTest.CreateFrom(wordsByThemes, wordsCount);
                words.StoreToPhp(Path.Combine(targetFolder, string.Format("modality_words_{0}.php", i)));
                XmlSerialization.SaveToXml(words, Path.Combine(targetFolder, string.Format("modality_words_{0}.xml", i)));
                WordPositions wordPositions;
                using (var bitmap = imageCreator.CreateImage(words.UsedWords, new Size(1024, 720), new Size(4, 4), out wordPositions))
                {
                    Assert.IsNotNull(bitmap);
                    Assert.IsNotNull(wordPositions);
                    Assert.IsTrue(wordPositions.Count == wordsCount);
                    bitmap.Save(Path.Combine(targetFolder, string.Format("modality_{0}.png", i)));
                }
                XmlSerialization.SaveToXml(wordPositions, Path.Combine(targetFolder, string.Format("modality_{0}.xml", i)));
                var deserialized = XmlSerialization.LoadFromXml<WordPositions>(Path.Combine(targetFolder, string.Format("modality_{0}.xml", i)));
                Assert.IsNotNull(deserialized);
                Assert.IsTrue(deserialized.GetType() == typeof(WordPositions));
                Assert.IsTrue(deserialized.Count == wordsCount);
                words.StoreQuestionsToPhp(Path.Combine(targetFolder, string.Format("modality_{0}.php", i)), 20);
            }
        }

        private IEnumerable<string> CreateWords(int count)
        {
            for (int i = 0; i < count; i++)
            {
                yield return string.Format("word{0}", i);
            }
        }
    }
}
