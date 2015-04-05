using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using Psychex.Logic.Experiments.WordRetrieval;
using Psychex.Logic.Helpers;

namespace Psychex.Logic.Tests.Tests.WordRetrieval
{
    [TestClass]
    public class ResultsParserTest
    {
        [TestMethod] 
        public void ParsingTest()
        {
            long valid = 0, invalid = 0;
            foreach (var file in Directory.GetFiles(Path.Combine(WordsByThemesTest.PsychexPath, "results"), "*.txt"))
            {
                if (ParsingTest(file))
                {
                    valid++;
                    var results = new ExperimentResults(ResultsParser.Load(file).ToArray());
                    var activeCount = results.ActiveAnswers.Split(' ', ',', '\t', '\r', '\n');
                }
                else invalid++;
            }
            Console.WriteLine("From {0} files is {1} valid, {2} invalid", invalid + valid, valid, invalid);
         }

        [TestMethod]
        public void ResultsTest()
        {
            var experimentResults = LoadExperimentResults();
            Console.WriteLine("{0} validated experiment results from {1} clients", experimentResults.Count(), experimentResults.Select(er => er.Client).Distinct().Count());
            int activeMax = experimentResults.Max(er => er.ActiveAnswersWords.Count());
            Console.WriteLine("{0} average active answers, {1} min, {2} max", experimentResults.Average(er => er.ActiveAnswersWords.Count()), experimentResults.Min(er => er.ActiveAnswersWords.Count()), activeMax);
            int passiveMax = experimentResults.Max(er => er.PassiveAnswersWords.Count());
            Console.WriteLine("{0} average passive answers, {1} min, {2} max", experimentResults.Average(er => er.PassiveAnswersWords.Count()), experimentResults.Min(er => er.PassiveAnswersWords.Count()), passiveMax);
            // Console.WriteLine(experimentResults.First(er => er.ActiveAnswersWords.Count() == activeMax).ActiveAnswers);
            Console.WriteLine("{0} with name, {1} with contact, {2} with birth, {4} with birth year, {3} males", experimentResults.Count(er => !string.IsNullOrWhiteSpace(er.Name)), experimentResults.Count(er => !string.IsNullOrWhiteSpace(er.Contact)), experimentResults.Count(er => !string.IsNullOrWhiteSpace(er.Birth)), experimentResults.Count(er => er.Sex.Equals("male")), experimentResults.Count(er => er.BirthYear.HasValue));
            Console.WriteLine("{0} average total seconds, {1} min, {2} max", experimentResults.Average(er => er.TotalTime.TotalSeconds), experimentResults.Min(er => er.TotalTime.TotalSeconds), experimentResults.Max(er => er.TotalTime.TotalSeconds));
            var birthYears = experimentResults.Where(er => er.BirthYear.HasValue).Select(er => er.BirthYear.Value).ToArray();
            Console.WriteLine("{0} average birth year, {1} min, {2} max", birthYears.Average(), birthYears.Min(), birthYears.Max());
            foreach (var er in experimentResults.Where(er => er.BirthYear.HasValue))
            {
                Console.WriteLine("{0}\t{1}", er.BirthYear, er.ActiveAnswersWords.Length);
            }
            return;
            var birthYearsOccurency = new OccurencyCounter<int> { birthYears };
            foreach (var year in birthYearsOccurency.Keys.OrderBy(k => k))
            {
                Console.WriteLine("{0}\t{1}\t{2:##.00}", year, birthYearsOccurency[year], experimentResults.Where(er => er.BirthYear.HasValue && er.BirthYear == year).Average(er => (double) er.ActiveAnswersWords.Length));
            }
            var wordsUsage = new OccurencyCounter<string>();
            var wordsOccurency = new OccurencyCounter<string>();
            var modalities = ExperimentResultsTest.LoadModalities();
            foreach (var result in experimentResults)
            {
                wordsUsage.Add (modalities[result.Modality].Words.UsedWords);
                var identificator = new WordIdentificator(modalities[result.Modality].Words.UsedWords);
                foreach (var word in result.ActiveAnswersWords)
                {
                    string identified;
                    if (identificator.TryIdentify(word, out identified)) wordsOccurency.Add(identified);
                }
            }
            var wordsStrength = wordsOccurency.Keys.Select(w => new KeyValuePair<string, double> (w, (double) wordsOccurency[w]/wordsUsage[w]));
            var index = 1;
            foreach (var wordOcc in wordsStrength.OrderByDescending(wc => wc.Value))
            {
                Console.WriteLine("{4}\t{0}\t{1:##.00}\t{2}\t{3}", wordOcc.Key, wordOcc.Value * 100.0, wordsOccurency[wordOcc.Key], wordsUsage[wordOcc.Key], index);
                index++;
            }
        }

        
        public static ExperimentResults[] LoadExperimentResults()
        {
            var files = Directory.GetFiles(Path.Combine(WordsByThemesTest.PsychexPath, "results"), "*.txt");
            var allResults = files.Select(file => ResultsParser.Load(file).ToArray());
            var experimentResults = allResults.Where(r => ResultsValidator.Validate(r).IsValid).Select(r => new ExperimentResults(r)).ToArray();
            return experimentResults;
        }

        private bool ParsingTest(string filename)
        {
            var parsed = ResultsParser.Load(filename).ToList();
            Assert.IsNotNull(parsed);
            Assert.IsTrue(parsed.Count > 0);
            var validationResults = ResultsValidator.Validate(parsed);
            Console.WriteLine("File {0} is {1}", filename, validationResults.IsValid ? "valid" : "invalid");
            foreach (var error in validationResults.Errors) Console.WriteLine(error);
            return validationResults.IsValid;
        }
    }
}