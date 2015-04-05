using System;
using System.Collections.Generic;
using System.Drawing;
using System.Drawing.Drawing2D;
using System.Drawing.Text;
using System.IO;
using System.Linq;
using Meta.Numerics.Statistics;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using Psychex.Logic.Experiments.WordRetrieval;
using Psychex.Logic.Helpers;

namespace Psychex.Logic.Tests.Tests.WordRetrieval
{
    [TestClass]
    public class ExperimentResultsTest
    {
        [TestMethod]
        public void TestLoadModalities()
        {
            LoadModalities();
        }

        public static Dictionary<int, Modality> LoadModalities()
        {
            var modalities = new Dictionary<int, Modality>(1024);
            for (var i = 0; i < 1024; i++)
            {
                modalities[i] = ModalityTest.LoadModality(i);
            }
            Assert.IsTrue(modalities.Count == 1024);
            return modalities;
        }

        [TestMethod]
        public void TestReconstructModalityImage()
        {
            var results = ResultsParserTest.LoadExperimentResults();
            var targetPath = Path.Combine(WordsByThemesTest.PsychexPath, "evaluation");
            if (!Directory.Exists(targetPath)) Directory.CreateDirectory(targetPath);
            else
            {
                foreach (var file in Directory.GetFiles(targetPath, "*")) File.Delete(file);
            }
            foreach (var result in results)
            {
                var modality = ModalityTest.LoadModality(result.Modality);
                var imageFile = Path.Combine(targetPath, string.Format("{0}.png", result.Uuid));
                CreateModalityImage(modality, result, imageFile);
            }
        }

        [TestMethod]
        public void WordsIdentificationTest()
        {
            var allUsedWords = LoadModalities().SelectMany(m => m.Value.Words.UsedWords).Distinct().ToArray();
            var identificator = new WordIdentificator(allUsedWords);
            var allActiveAnswers = ResultsParserTest.LoadExperimentResults().SelectMany(r => r.ActiveAnswersWords).Distinct().ToArray();
            string identifiedWord;
            var notIdentifiedWords = allActiveAnswers.Where(w => !identificator.TryIdentify(w, out identifiedWord)).ToArray();
            foreach (var notIdentified in notIdentifiedWords) Console.Write("{0}, ", notIdentified);
        }

        [TestMethod]
        public void ResultsEvaluationTest()
        {
            var results = ResultsParserTest.LoadExperimentResults();
            var evaluator = new ResultsEvaluator {Modalities = LoadModalities()};
            var evaluatedResults = results.Select(evaluator.Evaluate).ToArray();
            EvaluateResults(evaluatedResults);
        }

        [TestMethod]
        public void MaleResultsEvaluationTest()
        {
            var results = ResultsParserTest.LoadExperimentResults();
            var evaluator = new ResultsEvaluator { Modalities = LoadModalities() };
            var evaluatedResults = results.Where(r => r.IsMale).Select(evaluator.Evaluate).ToArray();
            EvaluateResults(evaluatedResults);
        }

        [TestMethod]
        public void FemaleResultsEvaluationTest()
        {
            var results = ResultsParserTest.LoadExperimentResults();
            var evaluator = new ResultsEvaluator { Modalities = LoadModalities() };
            var evaluatedResults = results.Where(r => r.IsFemale).Select(evaluator.Evaluate).ToArray();
            EvaluateResults(evaluatedResults);
        }

        [TestMethod]
        public void MaleFemaleResultsEvaluationTest()
        {
            var results = ResultsParserTest.LoadExperimentResults();
            var evaluator = new ResultsEvaluator { Modalities = LoadModalities() };
            var evaluatedMaleResults = results.Where(r => r.IsMale).Select(evaluator.Evaluate).ToArray();
            var evaluatedFemaleResults = results.Where(r => r.IsFemale).Select(evaluator.Evaluate).ToArray();
            var maleStrengths = EvaluateResults(evaluatedMaleResults);
            var femaleStrengths = EvaluateResults(evaluatedFemaleResults);
            var diffs = maleStrengths.Select(ms => new KeyValuePair<string, double>(ms.Key, ms.Value.Mean - femaleStrengths.First(fs => fs.Key.Equals(ms.Key)).Value.Mean)).ToArray();
            // Console.Clear();
            Console.WriteLine("-------------------------------------------------------");
            int index = 1;
            foreach (var diff in diffs.OrderByDescending(d => d.Value))
            {
                Console.WriteLine("{0}\t{4}\t{1:#0.00}\t{2:#0.00}\t{3:#0.00}", index, diff.Value * 100.0, maleStrengths.First(s => s.Key.Equals(diff.Key)).Value.Mean * 100.0, femaleStrengths.First(s => s.Key.Equals(diff.Key)).Value.Mean * 100.0, diff.Key);
                index++;
            }
        }

        [TestMethod]
        public void RandomResultsEvaluationTest()
        {
            var results = ResultsParserTest.LoadExperimentResults();
            var evaluator = new ResultsEvaluator {Modalities = LoadModalities()};
            var randomizedResults = GetRandomizedResults(results, evaluator);
            EvaluateResults(randomizedResults);
        }

        [TestMethod]
        public void KohonenMapTest()
        {
            var results = ResultsParserTest.LoadExperimentResults();
            var evaluator = new ResultsEvaluator { Modalities = LoadModalities() };
            var evaluatedResults = results.Select(evaluator.Evaluate).ToArray();
            var kohonenMapSource = new Dictionary<string, double[]>();
            foreach (var evaluatedResult in evaluatedResults)
            {
                var name = string.Format("{0} {1} {2} {3} {4}", evaluatedResult.Results.Name, evaluatedResult.Results.Contact, evaluatedResult.Results.BirthYear, evaluatedResult.Results.Sex, evaluatedResult.Results.Uuid);
                var value = EvaluateResults(new[] {evaluatedResult}).OrderBy(t => t.Key).Select(t => t.Value.Mean).ToArray();
                kohonenMapSource[name] = value;
            }
            var map = new KohonenMap();
            map.CalculateMap(kohonenMapSource);
        }

        [TestMethod]
        public void WordsAssociationTest()
        {
            var results = ResultsParserTest.LoadExperimentResults();
            var evaluator = new ResultsEvaluator { Modalities = LoadModalities() };
            var evaluatedResults = results.Select(evaluator.Evaluate).ToArray();
            // var evaluatedResults = results.Select(er => Randomize(evaluator.Evaluate(er))).ToArray();
            var allWords = evaluatedResults.SelectMany(er => er.Modality.Words.UsedWords).Distinct().OrderBy(w => w).ToArray();
            var associations = new List<KeyValuePair<string, double>>();
            foreach (var word1 in allWords)
            {
                foreach (var word2 in allWords)
                {
                    if (word1.CompareTo(word2) <= 0) continue;
                    var relevantResults = evaluatedResults.Where(er => er.Modality.Words.UsedWords.Contains(word1) && er.Modality.Words.UsedWords.Contains(word2));
                    int a = 0, b = 0, c = 0, d = 0;
                    foreach (var relevantResult in relevantResults)
                    {
                        if (relevantResult.RightActiveAnswerDistinct.Contains(word1))
                        {
                            if (relevantResult.RightActiveAnswerDistinct.Contains(word2)) a++;
                            else b++;
                        }
                        else
                        {
                            if (relevantResult.RightActiveAnswerDistinct.Contains(word2)) c++;
                            else d++;
                        }
                    }
                    var table = new BinaryContingencyTable(new int[2,2] {{a, b}, {c, d}});
                    var fischerTest = table.FisherExactTest();
                    var prob = table.ProbabilityOfRowConditionalOnColumn(0, 0).Value;
                    associations.Add(new KeyValuePair<string, double>(string.Format("{0}~{1}", word1, word2), fischerTest.Statistic));
                }
            }
            foreach (var association in associations.OrderBy(a => a.Value).Take(64))
            {
                Console.WriteLine("{0}\t{1:#0.0000} %", association.Key, association.Value * 100.0);
            }
        }

        [TestMethod]
        public void SexAssociationTest()
        {
            var results = ResultsParserTest.LoadExperimentResults();
            var evaluator = new ResultsEvaluator { Modalities = LoadModalities() };
            var evaluatedResults = results.Select(evaluator.Evaluate).ToArray();
            // var evaluatedResults = results.Select(er => Randomize(evaluator.Evaluate(er))).ToArray();
            var allWords = evaluatedResults.SelectMany(er => er.Modality.Words.UsedWords).Distinct().OrderBy(w => w).ToArray();
            var associations = new List<KeyValuePair<string, double>>();
            foreach (var word in allWords)
            {
                    var relevantResults = evaluatedResults.Where(er => er.Modality.Words.UsedWords.Contains(word));
                    int a = 0, b = 0, c = 0, d = 0;
                    foreach (var relevantResult in relevantResults)
                    {
                        if (relevantResult.Results.IsMale)
                        {
                            if (relevantResult.RightActiveAnswerDistinct.Contains(word)) a++;
                            else b++;
                        }
                        else
                        {
                            if (relevantResult.RightActiveAnswerDistinct.Contains(word)) c++;
                            else d++;
                        }
                    }
                    var table = new BinaryContingencyTable(new int[2, 2] { { a, b }, { c, d } });
                    var fischerTest = table.FisherExactTest();
                    associations.Add(new KeyValuePair<string, double>(word, fischerTest.Statistic));
            }
            foreach (var association in associations.OrderBy(a => a.Value).Take(64))
            {
                Console.WriteLine("{0}\t{1:#0.0000} %", association.Key, association.Value * 100.0);
            }
        }

        [TestMethod]
        public void AgeCorrellationTest()
        {
            var results = ResultsParserTest.LoadExperimentResults();
            var evaluator = new ResultsEvaluator { Modalities = LoadModalities() };
            var evaluatedResults = results.Select(evaluator.Evaluate).ToArray();
            var bivariateSample = new BivariateSample();
            foreach (var evaluatedResult in evaluatedResults.Where(er => er.Results.BirthYear.HasValue))
            {
                bivariateSample.Add(evaluatedResult.Results.BirthYear.Value, evaluatedResult.RightActiveAnswerDistinct.Count());
            }
            Console.WriteLine("Count {0}", bivariateSample.Count);
            Console.WriteLine("Covariance {0}", bivariateSample.Covariance);
            Console.WriteLine("Correlation coefficient {0}", bivariateSample.CorrelationCoefficient);
            Console.WriteLine("Population covariance {0}", bivariateSample.PopulationCovariance);
            Console.WriteLine("Kendal tau {0}", bivariateSample.KendallTauTest().Statistic);
        }

        private EvaluatedResults[] GetRandomizedResults(ExperimentResults[] results, ResultsEvaluator evaluator)
        {
            var evaluatedResults = results.SelectMany(er =>
                {
                    var q = new List<EvaluatedResults>(256);
                    for (var i = 0; i < 1; i++)
                    {
                        q.Add(Randomize(evaluator.Evaluate(er)));
                    }
                    return q;
                }).ToArray();
            return evaluatedResults;
        }

        private EvaluatedResults Randomize(EvaluatedResults result)
        {
            var randomActiveAnswer = result.Modality.Words.UsedWords.Shuffle().Take(result.IdentifiedActiveAnswer.Length).Select(w => new IdentifiedWord(w, w)).ToArray();
            return new EvaluatedResults(result.Results, result.Modality, randomActiveAnswer);
        }

        private static KeyValuePair<string, Sample>[] EvaluateResults(EvaluatedResults[] evaluatedResults)
        {
            Console.WriteLine("{0} experimental results used in analysis", evaluatedResults.Length);
            /*
            foreach (var evaluatedResult in evaluatedResults.Where(er => !string.IsNullOrEmpty(er.Results.Contact)))
            {
                Console.WriteLine("{0}\t{1}", evaluatedResult.Results.Uuid, evaluatedResult.Results.Contact);
            }*/
            /*
            var strengths = new Dictionary<string, double>();
            foreach (var usedWord in evaluator.Modalities.SelectMany(m => m.Value.Words.UsedWords).Distinct())
            {
                double strengthSum = 0;
                int usageCounter = 0;
                foreach (var evaluatedResult in evaluatedResults.Where(er => er.Modality.Words.UsedWords.Contains(usedWord)))
                {
                    usageCounter++;
                    var wasAnswered = evaluatedResult.RightActiveAnswerDistinct.Contains(usedWord);
                    if (wasAnswered) strengthSum += evaluatedResult.ActiveAnswerWordSelectionProbability;
                    else strengthSum -= (1.0 - evaluatedResult.ActiveAnswerWordSelectionProbability);
                }
                double strength = strengthSum/usageCounter;
                strengths[usedWord] = strength;
            }
            var i = 1;
            foreach (var strength in strengths.OrderByDescending(s => s.Value))
            {
                Console.WriteLine("{0}\t{1}\t{2:0.00}", i, strength.Key, strength.Value * 100.0);
                i++;
            }*/
            int femalesCount = 0, malesCount = 0;
            foreach (var er in evaluatedResults.Where(e => !string.IsNullOrEmpty(e.Results.Sex)))
            {
                if (er.Results.Sex.Equals("female")) femalesCount++;
                else malesCount++;
            }
            Console.WriteLine("{0} males, {1} females ({2:#0.00} %)", malesCount, femalesCount, 100.0 * femalesCount / (malesCount + femalesCount));
            var wordsUsed = new OccurencyCounter<string>();
            var wordsAnswered = new OccurencyCounter<string>();
            foreach (var result in evaluatedResults)
            {
                wordsUsed.Add(result.Modality.Words.UsedWords);
                wordsAnswered.Add(result.RightActiveAnswerDistinct);
            }
            var allWords = wordsUsed.Keys;
            var wordArea = new Dictionary<string, double>();
            foreach (var word in allWords)
            {
                var wordPosition = evaluatedResults.First(er => er.Modality.Positions.ContainsKey(word)).Modality.Positions[word];
                wordArea[word] = wordPosition.Height*wordPosition.Width;
            }
            var wordsStrengths = allWords.Select(w => new KeyValuePair<string, Sample>(w, GetWordOccurencySample(w, evaluatedResults))).ToArray();
            var index = 1;
            foreach (var wordStrength in wordsStrengths.OrderByDescending(wc => wc.Value.Mean))
            {
                Console.WriteLine("{5}\t{0}\t{1:#0.00}\t{3}\t{4}\t{6}", wordStrength.Key, wordStrength.Value.Mean * 100.0, null, wordsAnswered.ContainsKey(wordStrength.Key) ? wordsAnswered[wordStrength.Key] : 0.0, wordsUsed[wordStrength.Key], index, wordArea[wordStrength.Key]);
                index++;
            }
            var wordsByThemes = WordsByThemesTest.LoadWordsByThemes();
            var themesStrength = wordsByThemes.Select(themeAndWords => new KeyValuePair<string, Sample>(themeAndWords.Key, GetThemeStrength(wordsStrengths, themeAndWords.Value))).ToArray();
            /*
            Console.WriteLine("--- Themes strength ---");
            index = 1;
            foreach (var themeStrength in themesStrength.OrderByDescending(wc => wc.Value.Mean))
            {
                Console.WriteLine("{4}\t{0}\t{1:#0.00}\t{2:#0.00}\t{3:#0.00}", themeStrength.Key, themeStrength.Value.Mean, themeStrength.Value.PopulationMean.ConfidenceInterval(0.95), themeStrength.Value.Median * 100.0, index);
                index++;
            }*/
            return wordsStrengths;
        }

        private static Sample GetWordOccurencySample(string word, EvaluatedResults[] evaluatedResults)
        {
            return new Sample(evaluatedResults.Where(er => er.Modality.Words.UsedWords.Contains(word)).Select(er => er.RightActiveAnswerDistinct.Contains(word) ? 1.0 : 1e-9));
        }

        private static Sample GetThemeStrength(IEnumerable<KeyValuePair<string, Sample>> wordsStrengths, List<string> themeWords)
        {
            return new Sample(themeWords.Where(tw => wordsStrengths.Any(ws => ws.Key.Equals(tw))).Select(tw => wordsStrengths.First(ws => ws.Key.Equals(tw)).Value.Mean * 100.0));
        }

        private void CreateModalityImage(Modality modality, ExperimentResults result, string imageFile)
        {
            using (var bitmap = DrawResult(modality, result, new Size(1024, 720)))
            {
                bitmap.Save(imageFile);
            }
        }

        private Bitmap DrawResult(Modality modality, ExperimentResults result, Size bitmapSize)
        {
            var bitmap = new Bitmap(bitmapSize.Width, bitmapSize.Height);
            using (var graphics = Graphics.FromImage(bitmap))
            {
                graphics.SmoothingMode = SmoothingMode.AntiAlias;
                graphics.TextRenderingHint = TextRenderingHint.AntiAlias;
                DrawResult(bitmap, modality, result);
                new ImageCreator().CreateImage(graphics, modality.Positions);
            }
            return bitmap;
        }

        private void DrawResult(Bitmap bitmap, Modality modality, ExperimentResults result)
        {
            var identificator = new WordIdentificator(modality.Words.UsedWords);
            var font = new Font("Cambrium", 11.0f);
            using (var graphics = Graphics.FromImage(bitmap))
            {
                graphics.SmoothingMode = SmoothingMode.AntiAlias;
                graphics.TextRenderingHint = TextRenderingHint.AntiAlias;
                PointF? lastPoint = null;
                var penOk = new Pen(Brushes.LightGreen, 2.0f);
                var penWrong = new Pen(Brushes.OrangeRed, 2.0f);
                var stringFormat = (StringFormat)StringFormat.GenericTypographic.Clone();
                stringFormat.Alignment = StringAlignment.Center;
                stringFormat.LineAlignment = StringAlignment.Center;
                string lastIdentified = null;
                double? lastAngle = null;
                foreach (var activeWord in result.ActiveAnswersWords)
                {
                    string identified;
                    if (identificator.TryIdentify(activeWord, out identified))
                    {
                        var position = modality.Positions[identified];
                        var newPoint = position.GetCenter();
                        if (lastPoint.HasValue)
                        {
                            graphics.DrawLine(lastIdentified != null ? penOk : penWrong, lastPoint.Value, newPoint);
                        }
                        else
                        {
                            graphics.DrawCircle(newPoint, 10.0f, null, Brushes.LightGreen);
                        }
                        lastPoint = newPoint;
                        if (lastIdentified != null)
                        {
                            var previsousPosition = modality.Positions[lastIdentified].GetCenter();
                            var currentPosition = modality.Positions[identified].GetCenter();
                            var vector = new PointF(currentPosition.X - previsousPosition.X, currentPosition.Y - previsousPosition.Y);
                            var angle = Math.Atan2(vector.Y, vector.X) * 180.0 / Math.PI;
                            // if (angle > 180.0) angle -= 180.0;
                            var angleString = lastAngle.HasValue ? string.Format("{0:0°} {1:0°}", angle, lastAngle - angle) : string.Format("{0:0°}", angle);
                            var anglePosition = previsousPosition.GetMidPoint(currentPosition);
                            graphics.DrawString(angleString, font, Brushes.DarkGreen, anglePosition.X, anglePosition.Y, stringFormat);
                            lastAngle = angle;
                        }
                    }
                    else
                    {
                        lastAngle = null;
                    }
                    lastIdentified = identified;
                }
                if (lastPoint.HasValue)
                {
                    graphics.DrawCircle(lastPoint.Value, 10.0f, penOk, Brushes.White);
                }
            }
        }
    }
}