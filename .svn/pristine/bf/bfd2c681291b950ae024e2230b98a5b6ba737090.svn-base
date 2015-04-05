using System;
using System.IO;
using System.Net.Mime;
using System.Text;
using System.Collections.Generic;
using System.Linq;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using Textant.Logic.Tests.Helpers;

namespace Textant.Logic.Tests.Tests
{
    [TestClass]
    public class TextsTest
    {
        [TestMethod]
        public void TestSourceFiles()
        {
            var wrongFiles = new List<string>();
            foreach (var textFile in Texts.SourceTextFiles)
            {
                try
                {
                    Texts.GetNormalizedTextFile(textFile);
                }
                catch
                {
                    wrongFiles.Add(textFile);
                }
            }
            foreach (var wrongFile in wrongFiles)
            {
                File.Move(wrongFile, wrongFile + ".wrong");
                Console.WriteLine(wrongFile);
            }
        }
    }
}
