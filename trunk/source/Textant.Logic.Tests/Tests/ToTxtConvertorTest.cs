using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Threading;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using Microsoft.Office.Interop.Word;
using Textant.Logic.Tests.Helpers;

namespace Textant.Logic.Tests.Tests
{
    [TestClass]
    public class ToTxtConvertorTest
    {
        [TestMethod]
        public void ConvertDocumentsTest()
        {
            //ConvertFiles(sourceFiles, @"c:\projects\textant\documents\texts");
            ConvertFiles(GetConvertibleFiles(), Texts.SourceTextsDirectory);
        }

        private IEnumerable<string> GetConvertibleFiles()
        {
            return GetConvertibleFiles(@"g:\d\texty\", SearchOption.AllDirectories);
        }

        private  IEnumerable<string> GetConvertibleFiles(string directory, SearchOption searchOption)
        {
            return Directory.GetFiles(directory, "*.doc", searchOption).Concat(Directory.GetFiles(directory, "*.rtf", searchOption)).Concat(Directory.GetFiles(directory, "*.htm", searchOption)).Concat(Directory.GetFiles(directory, "*.html", searchOption)).Concat(Directory.GetFiles(directory, "*.txt", searchOption));
        }

        private void ConvertFiles(IEnumerable<string> sourceFiles, string targetPath)
        {
            var wordCounter = new WordOccurencyTools();
            var wordApp = new Microsoft.Office.Interop.Word.Application();
            foreach (var sourceFile in sourceFiles)
            {
                var targetFile = Path.Combine(targetPath, Path.GetFileNameWithoutExtension(sourceFile) + ".txt");
                if (!File.Exists(targetFile))
                {
                    if (Path.GetExtension(sourceFile) == ".txt")
                    {
                        File.Copy(sourceFile, targetFile);
                    }
                    else
                    {
                        ConvertFile(sourceFile, targetFile, wordApp);
                    }
                }
                if (File.Exists(targetFile) && !wordCounter.CanLoadTextFromFile(targetFile)) File.Delete(targetFile);
            }
        }

        private void ConvertFile(string sourceFile, string targetFile, Application wordApp)
        {
            Document document = null;
            try
            {
                document = wordApp.Documents.Open(sourceFile);
                document.SaveAs(targetFile, WdSaveFormat.wdFormatText);
                document.Close();
                document = null;
                Console.WriteLine("{0} => {1}", sourceFile, targetFile);
                for (; wordApp.Documents.Count > 0; Thread.Sleep(1000));
            }
            catch (Exception exception)
            {
                Console.WriteLine("Exception when converting {0}: {1}", sourceFile, exception);
            }
            finally 
            {
                if (document != null) document.Close();
            }
        }
    }
}