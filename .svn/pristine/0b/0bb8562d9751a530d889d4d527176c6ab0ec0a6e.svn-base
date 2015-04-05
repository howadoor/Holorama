using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using Psychex.Logic.Helpers;

namespace Psychex.Logic.Experiments.WordRetrieval
{
    public static class WordsForExperimentEx
    {
        public static void StoreToPhp(this WordsForExperiment words, string filename)
        {
             using (var fs = new FileStream(filename, FileMode.Create, FileAccess.Write, FileShare.None))
             {
                 using (var writer = new StreamWriter(fs, Encoding.UTF8))
                 {
                     StoreToPhp(words, writer);
                 }
             }
        }

        public static void StoreQuestionsToPhp(this WordsForExperiment words, string filename, int count)
        {
            using (var fs = new FileStream(filename, FileMode.Create, FileAccess.Write, FileShare.None))
            {
                using (var writer = new StreamWriter(fs, Encoding.UTF8))
                {
                    StoreQuestionsToPhp(words, writer, count);
                }
            }
        }

        private static void StoreToPhp(WordsForExperiment words, StreamWriter target)
        {
            target.WriteLine("<?php");
            target.WriteLine("$used_words = array(");
            foreach (var word in words.UsedWords) target.WriteLine("'{0}',", word);
            target.WriteLine(");");
            target.WriteLine("$not_used_words = array(");
            foreach (var word in words.NotUsedWords) target.WriteLine("'{0}',", word);
            target.WriteLine(");");
            target.WriteLine("?>");
        }

        private static void StoreQuestionsToPhp(WordsForExperiment words, StreamWriter target, int count)
        {
            var questions = words.UsedWords.Shuffle().Take(count).Concat(words.NotUsedWords.Shuffle().Take(count)).Shuffle().Take(count);
            target.WriteLine("<?php");
            target.WriteLine("$questions = array(");
            foreach (var question in questions) target.WriteLine("'{0}',", question);
            target.WriteLine(");");
            target.WriteLine("?>");
        }
    }
}