using System;
using System.Collections.Generic;
using System.Linq;
using System.Text.RegularExpressions;
using BayesianTest.BayesianClassificator;
using Meta.Numerics;
using Meta.Numerics.Statistics;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using Psychex.Logic.Experiments.WordRetrieval;
using Psychex.Logic.Helpers;

namespace Psychex.Logic.Tests.Tests.WordRetrieval
{
    [TestClass]
    public class ExperimentResultsExportTest
    {
        private EvaluatedResults[] evaluatedResults;

        private EvaluatedResults[] EvaluatedResults
        {
            get
            {
                if (evaluatedResults == null)
                {
                    var results = ResultsParserTest.LoadExperimentResults();
                    var evaluator = new ResultsEvaluator {Modalities = ExperimentResultsTest.LoadModalities()};
                    evaluatedResults = results.Select(evaluator.Evaluate).Where(er => er.RightActiveAnswerDistinct.Count() > 0 && er.RightActiveAnswerDistinct.Count() <= 33).OrderBy(er => er.Results.StartTime).ToArray();
                }
                return evaluatedResults;
            }
        }

        private IEnumerable<EvaluatedResults> MaleEvaluatedResults
        {
            get { return EvaluatedResults.Where(er => er.Results.IsMale); }
        }

        private IEnumerable<EvaluatedResults> FemaleEvaluatedResults
        {
            get { return EvaluatedResults.Where(er => er.Results.IsFemale); }
        }

        protected IEnumerable<ResultsSet> ResultsSets
        {
            get
            {
                yield return new ResultsSet {Name = "Všechny výsledky", Results = EvaluatedResults};
                yield return new ResultsSet {Name = "Rok narození < 1985", Results = WhereBirthDateIn(EvaluatedResults, 1900, 1985)};
                yield return new ResultsSet {Name = "Rok narození >= 1985", Results = WhereBirthDateIn(EvaluatedResults, 1985, 3000)};
                yield return new ResultsSet {Name = "1940-1950", Results = WhereBirthDateIn(EvaluatedResults, 1940, 1950)};
                yield return new ResultsSet {Name = "1950-1960", Results = WhereBirthDateIn(EvaluatedResults, 1950, 1960)};
                yield return new ResultsSet {Name = "1960-1970", Results = WhereBirthDateIn(EvaluatedResults, 1960, 1970)};
                yield return new ResultsSet {Name = "1970-1980", Results = WhereBirthDateIn(EvaluatedResults, 1970, 1980)};
                yield return new ResultsSet {Name = "1980-1990", Results = WhereBirthDateIn(EvaluatedResults, 1980, 1990)};
                yield return new ResultsSet {Name = "1990-2000", Results = WhereBirthDateIn(EvaluatedResults, 1990, 2000)};
                yield return new ResultsSet {Name = "Muži", Results = MaleEvaluatedResults};
                yield return new ResultsSet {Name = "Ženy", Results = FemaleEvaluatedResults};
                yield return new ResultsSet {Name = "Muži 1960-1990", Results = WhereBirthDateIn(MaleEvaluatedResults, 1960, 1980)};
                yield return new ResultsSet {Name = "Ženy 1960-1990", Results = WhereBirthDateIn(FemaleEvaluatedResults, 1960, 1980)};
                yield return new ResultsSet {Name = "Muži < 1985", Results = WhereBirthDateIn(MaleEvaluatedResults, 1900, 1985)};
                yield return new ResultsSet {Name = "Muži >= 1985", Results = WhereBirthDateIn(MaleEvaluatedResults, 1985, 3000)};
                yield return new ResultsSet {Name = "Ženy < 1985", Results = WhereBirthDateIn(FemaleEvaluatedResults, 1900, 1985)};
                yield return new ResultsSet {Name = "Ženy >= 1985", Results = WhereBirthDateIn(FemaleEvaluatedResults, 1985, 3000)};

                yield return new ResultsSet {Name = "Muži 1950-1960", Results = WhereBirthDateIn(MaleEvaluatedResults, 1950, 1960)};
                yield return new ResultsSet {Name = "Ženy 1950-1960", Results = WhereBirthDateIn(FemaleEvaluatedResults, 1950, 1960)};
                yield return new ResultsSet {Name = "Muži 1960-1970", Results = WhereBirthDateIn(MaleEvaluatedResults, 1960, 1970)};
                yield return new ResultsSet {Name = "Ženy 1960-1970", Results = WhereBirthDateIn(FemaleEvaluatedResults, 1960, 1970)};
                yield return new ResultsSet {Name = "Muži 1970-1980", Results = WhereBirthDateIn(MaleEvaluatedResults, 1970, 1980)};
                yield return new ResultsSet {Name = "Ženy 1970-1980", Results = WhereBirthDateIn(FemaleEvaluatedResults, 1970, 1980)};
                yield return new ResultsSet {Name = "Muži 1980-1990", Results = WhereBirthDateIn(MaleEvaluatedResults, 1980, 1990)};
                yield return new ResultsSet {Name = "Ženy 1980-1990", Results = WhereBirthDateIn(FemaleEvaluatedResults, 1980, 1990)};
                yield return new ResultsSet {Name = "Muži 1990-2000", Results = WhereBirthDateIn(MaleEvaluatedResults, 1990, 2000)};
                yield return new ResultsSet {Name = "Ženy 1990-2000", Results = WhereBirthDateIn(FemaleEvaluatedResults, 1990, 2000)};
            }
        }

        protected IEnumerable<Tuple<ResultsSet, ResultsSet>> ComparisonResultsSets
        {
            get
            {
                yield return new Tuple<ResultsSet, ResultsSet>(new ResultsSet {Name = "Rok narození < 1985", Results = WhereBirthDateIn(EvaluatedResults, 1900, 1985)},
                                                               new ResultsSet {Name = "Rok narození >= 1985", Results = WhereBirthDateIn(EvaluatedResults, 1985, 3000)});
                yield return new Tuple<ResultsSet, ResultsSet>(new ResultsSet {Name = "1940-1950", Results = WhereBirthDateIn(EvaluatedResults, 1940, 1950)},
                                                               new ResultsSet {Name = "1950-1960", Results = WhereBirthDateIn(EvaluatedResults, 1950, 1960)});
                yield return new Tuple<ResultsSet, ResultsSet>(new ResultsSet {Name = "1950-1960", Results = WhereBirthDateIn(EvaluatedResults, 1950, 1960)},
                                                               new ResultsSet {Name = "1960-1970", Results = WhereBirthDateIn(EvaluatedResults, 1960, 1970)});
                yield return new Tuple<ResultsSet, ResultsSet>(new ResultsSet {Name = "1960-1970", Results = WhereBirthDateIn(EvaluatedResults, 1960, 1970)},
                                                               new ResultsSet {Name = "1970-1980", Results = WhereBirthDateIn(EvaluatedResults, 1970, 1980)});
                yield return new Tuple<ResultsSet, ResultsSet>(new ResultsSet {Name = "1970-1980", Results = WhereBirthDateIn(EvaluatedResults, 1970, 1980)},
                                                               new ResultsSet {Name = "1980-1990", Results = WhereBirthDateIn(EvaluatedResults, 1980, 1990)});
                yield return new Tuple<ResultsSet, ResultsSet>(new ResultsSet {Name = "1980-1990", Results = WhereBirthDateIn(EvaluatedResults, 1980, 1990)},
                                                               new ResultsSet {Name = "1990-2000", Results = WhereBirthDateIn(EvaluatedResults, 1990, 2000)});
                yield return new Tuple<ResultsSet, ResultsSet>(new ResultsSet {Name = "Muži", Results = MaleEvaluatedResults},
                                                               new ResultsSet {Name = "Ženy", Results = FemaleEvaluatedResults});
                yield return new Tuple<ResultsSet, ResultsSet>(new ResultsSet { Name = "Muži < 1985", Results = WhereBirthDateIn(MaleEvaluatedResults, 1900, 1985) },
                                                               new ResultsSet { Name = "Ženy < 1985", Results = WhereBirthDateIn(FemaleEvaluatedResults, 1900, 1985) });
                yield return new Tuple<ResultsSet, ResultsSet>(new ResultsSet { Name = "Muži >= 1985", Results = WhereBirthDateIn(MaleEvaluatedResults, 1985, 3000) },
                                                               new ResultsSet { Name = "Ženy >= 1985", Results = WhereBirthDateIn(FemaleEvaluatedResults, 1985, 3000) });

                yield return new Tuple<ResultsSet, ResultsSet>(new ResultsSet { Name = "Muži < 1985", Results = WhereBirthDateIn(MaleEvaluatedResults, 1900, 1985) },
                                                               new ResultsSet {Name = "Muži >= 1985", Results = WhereBirthDateIn(MaleEvaluatedResults, 1985, 3000)});
                yield return new Tuple<ResultsSet, ResultsSet>(new ResultsSet {Name = "Ženy < 1985", Results = WhereBirthDateIn(FemaleEvaluatedResults, 1900, 1985)},
                                                               new ResultsSet {Name = "Ženy >= 1985", Results = WhereBirthDateIn(FemaleEvaluatedResults, 1985, 3000)});
                yield return new Tuple<ResultsSet, ResultsSet>(new ResultsSet {Name = "Muži 1960-1990", Results = WhereBirthDateIn(MaleEvaluatedResults, 1960, 1980)},
                                                               new ResultsSet {Name = "Ženy 1960-1990", Results = WhereBirthDateIn(FemaleEvaluatedResults, 1960, 1980)});
                
                yield return new Tuple<ResultsSet, ResultsSet>(new ResultsSet {Name = "Muži 1950-1960", Results = WhereBirthDateIn(MaleEvaluatedResults, 1950, 1960)},
                                                               new ResultsSet {Name = "Muži 1960-1970", Results = WhereBirthDateIn(MaleEvaluatedResults, 1960, 1970)});
                yield return new Tuple<ResultsSet, ResultsSet>(new ResultsSet {Name = "Muži 1960-1970", Results = WhereBirthDateIn(MaleEvaluatedResults, 1960, 1970)},
                                                               new ResultsSet {Name = "Muži 1970-1980", Results = WhereBirthDateIn(MaleEvaluatedResults, 1970, 1980)});
                yield return new Tuple<ResultsSet, ResultsSet>(new ResultsSet {Name = "Muži 1970-1980", Results = WhereBirthDateIn(MaleEvaluatedResults, 1970, 1980)},
                                                               new ResultsSet {Name = "Muži 1980-1990", Results = WhereBirthDateIn(MaleEvaluatedResults, 1980, 1990)});
                yield return new Tuple<ResultsSet, ResultsSet>(new ResultsSet {Name = "Muži 1980-1990", Results = WhereBirthDateIn(MaleEvaluatedResults, 1980, 1990)},
                                                               new ResultsSet {Name = "Muži 1990-2000", Results = WhereBirthDateIn(MaleEvaluatedResults, 1990, 2000)});

                yield return new Tuple<ResultsSet, ResultsSet>(new ResultsSet {Name = "Ženy 1950-1960", Results = WhereBirthDateIn(FemaleEvaluatedResults, 1950, 1960)},
                                                               new ResultsSet {Name = "Ženy 1960-1970", Results = WhereBirthDateIn(FemaleEvaluatedResults, 1960, 1970)});
                yield return new Tuple<ResultsSet, ResultsSet>(new ResultsSet {Name = "Ženy 1960-1970", Results = WhereBirthDateIn(FemaleEvaluatedResults, 1960, 1970)},
                                                               new ResultsSet {Name = "Ženy 1970-1980", Results = WhereBirthDateIn(FemaleEvaluatedResults, 1970, 1980)});
                yield return new Tuple<ResultsSet, ResultsSet>(new ResultsSet {Name = "Ženy 1970-1980", Results = WhereBirthDateIn(FemaleEvaluatedResults, 1970, 1980)},
                                                               new ResultsSet {Name = "Ženy 1980-1990", Results = WhereBirthDateIn(FemaleEvaluatedResults, 1980, 1990)});
                yield return new Tuple<ResultsSet, ResultsSet>(new ResultsSet {Name = "Ženy 1980-1990", Results = WhereBirthDateIn(FemaleEvaluatedResults, 1980, 1990)},
                                                               new ResultsSet {Name = "Ženy 1990-2000", Results = WhereBirthDateIn(FemaleEvaluatedResults, 1990, 2000)});
            }
        }

        private IEnumerable<EvaluatedResults> WhereBirthDateIn(IEnumerable<EvaluatedResults> results, int from, int to)
        {
            return results.Where(er => er.Results.BirthYear.HasValue && er.Results.BirthYear >= from && er.Results.BirthYear < to);
        }

        [TestMethod]
        public void ExportResultsList()
        {
            var table = new VirtualTable<EvaluatedResults>();
            table.Columns.Add(new VirtualTable<EvaluatedResults>.Column {Header = "Id", DataFetcher = er => er.Results.Uuid});
            table.Columns.Add(new VirtualTable<EvaluatedResults>.Column {Header = "IP adresa", DataFetcher = er => er.Results.Client});
            table.Columns.Add(new VirtualTable<EvaluatedResults>.Column {Header = "Varianta", DataFetcher = er => er.Results.Modality});
            table.Columns.Add(new VirtualTable<EvaluatedResults>.Column {Header = "Začátek", DataFetcher = er => er.Results.StartTime});
            table.Columns.Add(new VirtualTable<EvaluatedResults>.Column {Header = "Konec", DataFetcher = er => er.Results.EndTime});
            table.Columns.Add(new VirtualTable<EvaluatedResults>.Column {Header = "Jméno", DataFetcher = er => er.Results.Name});
            table.Columns.Add(new VirtualTable<EvaluatedResults>.Column {Header = "Pohlaví", DataFetcher = er => er.Results.IsMale ? "M" : "Ž"});
            table.Columns.Add(new VirtualTable<EvaluatedResults>.Column {Header = "Rok narození", DataFetcher = er => er.Results.BirthYear});
            table.Columns.Add(new VirtualTable<EvaluatedResults>.Column {Header = "Počet slov v odpovědi", DataFetcher = er => er.Results.ActiveAnswersWords.Length});
            table.Columns.Add(new VirtualTable<EvaluatedResults>.Column {Header = "Počet správných slov v odpovědi", DataFetcher = er => er.RightActiveAnswerDistinct.Count()});
            table.Columns.Add(new VirtualTable<EvaluatedResults>.Column {Header = "Odpověď", DataFetcher = er => er.IdentifiedActiveAnswerDisplayString});
            table.Columns.Add(new VirtualTable<EvaluatedResults>.Column
                {
                    Header = "Průměrný úhel vektoru odpovědi",
                    DataFetcher = er =>
                        {
                            var angleSample = er.GetAngles().Select(a => a - 90.0).ToSample();
                            if (angleSample.Count == 0) return 0;
                            if (angleSample.Count == 1) return angleSample.First();
                            return angleSample.PopulationMean;
                        }
                });
            //table.Columns.Add(new VirtualTable<ResultsSet>.Column { Header = "Test nulového úhlu", DataFetcher = er => er.Results.SelectMany(r => r.GetAngles().Select(a => a - 90.0)).ToSample().StudentTTest(0.0).LeftProbability });
            table.Columns.Add(new VirtualTable<EvaluatedResults>.Column {Header = "Podíl úhlů vektoru v 1. kvadrantu", DataFetcher = er => er.GetAngles().Count(a => a >= 0 && a < 90.0)/(double) er.GetAngles().Count()});
            table.Columns.Add(new VirtualTable<EvaluatedResults>.Column {Header = "Podíl úhlů vektoru v 2. kvadrantu", DataFetcher = er => er.GetAngles().Count(a => a >= 90 && a < 180.0)/(double) er.GetAngles().Count()});
            table.Columns.Add(new VirtualTable<EvaluatedResults>.Column {Header = "Podíl úhlů vektoru v 3. kvadrantu", DataFetcher = er => er.GetAngles().Count(a => a < -90.0 && a >= -180.0)/(double) er.GetAngles().Count()});
            table.Columns.Add(new VirtualTable<EvaluatedResults>.Column {Header = "Podíl úhlů vektoru v 4. kvadrantu", DataFetcher = er => er.GetAngles().Count(a => a < 0.0 && a >= -90.0)/(double) er.GetAngles().Count()});
            table.ExportCsv(EvaluatedResults, @"c:\psychex_results.txt");
        }

        [TestMethod]
        public void ExportResultsOverallCharacteristics()
        {
            var table = new VirtualTable<ResultsSet>();
            table.Columns.Add(new VirtualTable<ResultsSet>.Column {Header = "Výběr", DataFetcher = er => er.Name});
            table.Columns.Add(new VirtualTable<ResultsSet>.Column {Header = "Začátek experimentu", DataFetcher = er => er.Results.Min(r => r.Results.StartTime)});
            table.Columns.Add(new VirtualTable<ResultsSet>.Column {Header = "Konec experimentu", DataFetcher = er => er.Results.Max(r => r.Results.EndTime)});
            table.Columns.Add(new VirtualTable<ResultsSet>.Column {Header = "Celkový počet výsledků", DataFetcher = er => er.Results.Count()});
            table.Columns.Add(new VirtualTable<ResultsSet>.Column {Header = "Počet různých IP adres respondentů", DataFetcher = er => er.Results.Select(r => r.Results.Client).Distinct().Count()});
            table.Columns.Add(new VirtualTable<ResultsSet>.Column {Header = "Počet použitých variant experimentu", DataFetcher = er => er.Results.Select(r => r.Modality).Distinct().Count()});
            table.Columns.Add(new VirtualTable<ResultsSet>.Column {Header = "Počet odpovědí s uvedeným rokem narození", DataFetcher = er => er.Results.Count(r => r.Results.BirthYear != null)});
            table.Columns.Add(new VirtualTable<ResultsSet>.Column {Header = "Rok narození nejstaršího respondenta", DataFetcher = er => er.Results.Where(r => r.Results.BirthYear != null).Min(r => r.Results.BirthYear.Value)});
            table.Columns.Add(new VirtualTable<ResultsSet>.Column {Header = "Rok narození nejmladšího respondenta", DataFetcher = er => er.Results.Where(r => r.Results.BirthYear != null).Max(r => r.Results.BirthYear.Value)});
            table.Columns.Add(new VirtualTable<ResultsSet>.Column {Header = "Průměr roku narození", DataFetcher = er => new Sample(er.Results.Where(r => r.Results.BirthYear != null).Select(r => (double) r.Results.BirthYear.Value)).PopulationMean});
            table.Columns.Add(new VirtualTable<ResultsSet>.Column {Header = "Medián roku narození", DataFetcher = er => new Sample(er.Results.Where(r => r.Results.BirthYear != null).Select(r => (double) r.Results.BirthYear.Value)).Median});
            table.Columns.Add(new VirtualTable<ResultsSet>.Column {Header = "Počet mužů", DataFetcher = er => er.Results.Count(r => r.Results.IsMale)});
            table.Columns.Add(new VirtualTable<ResultsSet>.Column {Header = "Počet žen", DataFetcher = er => er.Results.Count(r => r.Results.IsFemale)});
            table.Columns.Add(new VirtualTable<ResultsSet>.Column {Header = "Podíl mužů na celkovém počtu respondentů", DataFetcher = er => er.Results.Count(r => r.Results.IsMale)/(double) er.Results.Count()});
            /*
            table.Columns.Add(new VirtualTable<ResultsSet>.Column {Header = "Nejkratší doba experimentu [s]", DataFetcher = er => new Sample(er.Results.Select(r => r.Results.TotalTime.TotalSeconds)).Minimum});
            table.Columns.Add(new VirtualTable<ResultsSet>.Column {Header = "Nejdelší doba experimentu [s]", DataFetcher = er => new Sample(er.Results.Select(r => r.Results.TotalTime.TotalSeconds)).Maximum});
            table.Columns.Add(new VirtualTable<ResultsSet>.Column {Header = "Průměrná doba experimentu [s]", DataFetcher = er => new Sample(er.Results.Select(r => r.Results.TotalTime.TotalSeconds)).PopulationMean});
            */
            table.Columns.Add(new VirtualTable<ResultsSet>.Column {Header = "Nejmenší počet slov v odpovědi", DataFetcher = er => new Sample(er.Results.Select(r => (double) r.Results.ActiveAnswersWords.Count())).Minimum});
            table.Columns.Add(new VirtualTable<ResultsSet>.Column {Header = "Největší počet slov v odpovědi", DataFetcher = er => new Sample(er.Results.Select(r => (double) r.Results.ActiveAnswersWords.Count())).Maximum});
            table.Columns.Add(new VirtualTable<ResultsSet>.Column {Header = "Průměrný počet slov v odpovědi", DataFetcher = er => new Sample(er.Results.Select(r => (double) r.Results.ActiveAnswersWords.Count())).PopulationMean});
            table.Columns.Add(new VirtualTable<ResultsSet>.Column {Header = "Medián počtu slov v odpovědi", DataFetcher = er => new Sample(er.Results.Select(r => (double) r.Results.ActiveAnswersWords.Count())).Median});

            table.Columns.Add(new VirtualTable<ResultsSet>.Column {Header = "Nejmenší počet správných slov v odpovědi", DataFetcher = er => new Sample(er.Results.Select(r => (double) r.RightActiveAnswerDistinct.Count())).Minimum});
            table.Columns.Add(new VirtualTable<ResultsSet>.Column {Header = "Největší počet správných slov v odpovědi", DataFetcher = er => new Sample(er.Results.Select(r => (double) r.RightActiveAnswerDistinct.Count())).Maximum});
            table.Columns.Add(new VirtualTable<ResultsSet>.Column {Header = "Průměrný počet správných slov v odpovědi", DataFetcher = er => new Sample(er.Results.Select(r => (double) r.RightActiveAnswerDistinct.Count())).PopulationMean});
            table.Columns.Add(new VirtualTable<ResultsSet>.Column {Header = "Medián počtu správných slov v odpovědi", DataFetcher = er => new Sample(er.Results.Select(r => (double) r.RightActiveAnswerDistinct.Count())).Median});

            table.Columns.Add(new VirtualTable<ResultsSet>.Column {Header = "Průměrný úhel vektoru odpovědi", DataFetcher = er => er.Results.SelectMany(r => r.GetAngles()).ToSample().PopulationMean});
            table.Columns.Add(new VirtualTable<ResultsSet>.Column {Header = "Test nulového úhlu", DataFetcher = er => er.Results.SelectMany(r => r.GetAngles()).ToSample().StudentTTest(0.0).LeftProbability});
            table.Columns.Add(new VirtualTable<ResultsSet>.Column
                {
                    Header = "Podíl úhlů vektoru v 1. kvadrantu",
                    DataFetcher = er => er.Results.SelectMany(r => r.GetAngles().Where(a => a >= 0 && a < 90.0)).Count()/(double) er.Results.SelectMany(r => r.GetAngles()).Count()
                });
            table.Columns.Add(new VirtualTable<ResultsSet>.Column
                {
                    Header = "Podíl úhlů vektoru v 2. kvadrantu",
                    DataFetcher = er => er.Results.SelectMany(r => r.GetAngles().Where(a => a >= 90 && a < 180.0)).Count()/(double) er.Results.SelectMany(r => r.GetAngles()).Count()
                });
            table.Columns.Add(new VirtualTable<ResultsSet>.Column
                {
                    Header = "Podíl úhlů vektoru v 3. kvadrantu",
                    DataFetcher = er => er.Results.SelectMany(r => r.GetAngles().Where(a => a < -90.0 && a >= -180.0)).Count()/(double) er.Results.SelectMany(r => r.GetAngles()).Count()
                });
            table.Columns.Add(new VirtualTable<ResultsSet>.Column
                {
                    Header = "Podíl úhlů vektoru v 4. kvadrantu",
                    DataFetcher = er => er.Results.SelectMany(r => r.GetAngles().Where(a => a < 0.0 && a >= -90.0)).Count()/(double) er.Results.SelectMany(r => r.GetAngles()).Count()
                });
            for (var i = 0; i < 3; i++)
            {
                var skip = 0;
                var take = i + 1;
                table.Columns.Add(new VirtualTable<ResultsSet>.Column
                    {
                        Header = string.Format("Průměrný úhel vektoru {0}. až {1}. slova v odpovědi", skip + 1, skip + take + 1),
                        DataFetcher = er => er.Results.SelectMany(r => r.GetAngles().Skip(skip).Take(take)).ToSample().PopulationMean
                    });
                // table.Columns.Add(new VirtualTable<ResultsSet>.Column {Header = "Test nulového úhlu", DataFetcher = er => er.Results.SelectMany(r => r.GetAngles().Skip(skip).Take(take)).ToSample().StudentTTest(0.0).LeftProbability});
                table.Columns.Add(new VirtualTable<ResultsSet>.Column
                    {
                        Header = "Podíl úhlů vektoru v 1. kvadrantu",
                        DataFetcher = er => er.Results.SelectMany(r => r.GetAngles().Skip(skip).Take(take).Where(a => a >= 0 && a < 90.0)).Count()/(double) er.Results.SelectMany(r => r.GetAngles().Skip(skip).Take(take)).Count()
                    });
                table.Columns.Add(new VirtualTable<ResultsSet>.Column
                    {
                        Header = "Podíl úhlů vektoru v 2. kvadrantu",
                        DataFetcher = er => er.Results.SelectMany(r => r.GetAngles().Skip(skip).Take(take).Where(a => a >= 90 && a < 180.0)).Count()/(double) er.Results.SelectMany(r => r.GetAngles().Skip(skip).Take(take)).Count()
                    });
                table.Columns.Add(new VirtualTable<ResultsSet>.Column
                    {
                        Header = "Podíl úhlů vektoru v 3. kvadrantu",
                        DataFetcher = er => er.Results.SelectMany(r => r.GetAngles().Skip(skip).Take(take).Where(a => a < -90.0 && a >= -180.0)).Count()/(double) er.Results.SelectMany(r => r.GetAngles().Skip(skip).Take(take)).Count()
                    });
                table.Columns.Add(new VirtualTable<ResultsSet>.Column
                    {
                        Header = "Podíl úhlů vektoru v 4. kvadrantu",
                        DataFetcher = er => er.Results.SelectMany(r => r.GetAngles().Skip(skip).Take(take).Where(a => a < 0.0 && a >= -90.0)).Count()/(double) er.Results.SelectMany(r => r.GetAngles().Skip(skip).Take(take)).Count()
                    });
            }
            var allSet = new ResultsSet {Results = EvaluatedResults};
            foreach (var theme in allSet.Themes.OrderByDescending(t => t.Value.Value))
            {
                var themeName = theme.Key;
                table.Columns.Add(new VirtualTable<ResultsSet>.Column
                {
                    Header = theme.Key,
                    DataFetcher = er => er.Themes.First(t => t.Key.Equals(themeName)).Value.Value
                });
            }
            
            table.ExportCsvInverted(ResultsSets, @"c:\psychex_results_overall.txt");
        }

        [TestMethod]
        public void ExportWordResults()
        {
            var wordTable = new VirtualTable<WordOccurency>();
            wordTable.Columns.Add(new VirtualTable<WordOccurency>.Column { Header = "Slovo", DataFetcher = wr => wr.Word });
            wordTable.Columns.Add(new VirtualTable<WordOccurency>.Column { Header = "Počet zobrazení", DataFetcher = wr => wr.ShowCount });
            wordTable.Columns.Add(new VirtualTable<WordOccurency>.Column { Header = "Počet odpovědí", DataFetcher = wr => wr.FoundCount });
            wordTable.Columns.Add(new VirtualTable<WordOccurency>.Column { Header = "Pravděpodobnost odpovědi", DataFetcher = wr => wr.Probability });
            wordTable.Columns.Add(new VirtualTable<WordOccurency>.Column { Header = "Pravděpodobnost odpovědi", DataFetcher = wr =>
                {
                    var sample = wr.Sample;
                    if (sample.Count > 1) return sample.PopulationMean;
                    return string.Empty;
                } });
            wordTable.Columns.Add(new VirtualTable<WordOccurency>.Column { Header = "Délka slova ve znacích", DataFetcher = wr => wr.Word.Length });
            wordTable.Columns.Add(new VirtualTable<WordOccurency>.Column { Header = "Plocha slova na obrazovce", DataFetcher = wr => GetWordArea(wr.Word) });
            
            var themeTable = new VirtualTable<KeyValuePair<string, UncertainValue>>();
            themeTable.Columns.Add(new VirtualTable<KeyValuePair<string, UncertainValue>>.Column {Header = "Téma", DataFetcher = kv => kv.Key});
            themeTable.Columns.Add(new VirtualTable<KeyValuePair<string, UncertainValue>>.Column { Header = "Pravděpodobnost odpovědi", DataFetcher = kv => kv.Value.Value });
            themeTable.Columns.Add(new VirtualTable<KeyValuePair<string, UncertainValue>>.Column { Header = "Pravděpodobnost odpovědi", DataFetcher = kv => kv.Value });
            
            foreach (var resultsSet in ResultsSets)
            {
                var filename = string.Format("c:\\psychex_words_{0}.txt", resultsSet.Name).Replace('=', '_').Replace("<", "lwr").Replace(">", "grt");
                wordTable.ExportCsv(resultsSet.Occurencies, filename);
                filename = string.Format("c:\\psychex_themes_{0}.txt", resultsSet.Name).Replace('=', '_').Replace("<", "lwr").Replace(">", "grt");
                themeTable.ExportCsv(resultsSet.Themes.OrderByDescending(t => t.Value.Value), filename);
            }
        }

        private double GetWordArea(string word)
        {
            var result = EvaluatedResults.First(er => er.Modality.Words.UsedWords.Contains(word));
            var position = result.Modality.Positions[word];
            return position.Width*position.Height;
        }

        [TestMethod]
        public void ExportResultsComparison()
        {
            var table = new VirtualTable<Tuple<ResultsSet, ResultsSet>>();
            table.Columns.Add(new VirtualTable<Tuple<ResultsSet, ResultsSet>>.Column {Header = "Výběr 1", DataFetcher = er => er.Item1.Name});
            table.Columns.Add(new VirtualTable<Tuple<ResultsSet, ResultsSet>>.Column {Header = "Výběr 2", DataFetcher = er => er.Item2.Name});
            table.Columns.Add(new VirtualTable<Tuple<ResultsSet, ResultsSet>>.Column {Header = "Počet správných odpovědí V1 < V2", DataFetcher = er => GetRightActiveAnswerDistinctComparison(er).RightProbability});
            table.Columns.Add(new VirtualTable<Tuple<ResultsSet, ResultsSet>>.Column {Header = "Počet správných odpovědí V1 > V2", DataFetcher = er => GetRightActiveAnswerDistinctComparison(er).LeftProbability});
            table.ExportCsv(ComparisonResultsSets, @"c:\psychex_results_comparison.txt");
        }

        private TestResult GetRightActiveAnswerDistinctComparison(Tuple<ResultsSet, ResultsSet> sets)
        {
            var sample1 = sets.Item1.Results.Select(r => (double) r.RightActiveAnswerDistinct.Count()).ToSample();
            var sample2 = sets.Item2.Results.Select(r => (double) r.RightActiveAnswerDistinct.Count()).ToSample();
            return Sample.StudentTTest(sample1, sample2);
        }

        public static string[] GetShownWords(IEnumerable<EvaluatedResults> results)
        {
            return results.SelectMany(r => r.Modality.Words.UsedWords).Distinct().ToArray();
        }

        public static IEnumerable<WordOccurency> ComputeOccurencies(IEnumerable<EvaluatedResults> results)
        {
            foreach (var shownWord in GetShownWords(results))
            {
                var showCount = results.Count(r => r.Modality.Words.UsedWords.Contains(shownWord));
                var foundCount = results.Count(r => r.RightActiveAnswerDistinct.Contains(shownWord));
                yield return new WordOccurency { Word = shownWord, FoundCount = foundCount, ShowCount = showCount };
            }
        }

        public class ResultsSet
        {
            private WordOccurency[] occurencies;
            private Dictionary<string, UncertainValue> themes;
            public string Name { get; set; }
            public IEnumerable<EvaluatedResults> Results { get; set; }

            public WordOccurency[] Occurencies
            {
                get { return occurencies ?? (occurencies = ComputeOccurencies(Results).OrderByDescending(wocc => wocc.Probability).ToArray()); }
            }

            public Dictionary<string, UncertainValue> Themes
            {
                get
                {
                    return themes ?? (themes = ComputeThemes());
                }
            }

            private Dictionary<string, UncertainValue> ComputeThemes()
            {
                var wordByThemes = WordsByThemesTest.LoadWordsByThemes();
                var themes = new Dictionary<string, UncertainValue>();
                foreach (var theme in wordByThemes)
                {
                    UncertainValue? themeMean = null;
                    var count = 0;
                    foreach (var word in theme.Value)
                    {
                        var occurency = Occurencies.FirstOrDefault(occ => occ.Word.Equals(word));
                        if (occurency == null) continue;
                        if (occurency.ShowCount <= 1) continue;
                        if (count == 0) themeMean = occurency.Sample.PopulationMean;
                        else
                        {
                            themeMean = themeMean + occurency.Sample.PopulationMean;
                        }
                        count++;
                    }
                    if (count > 0) themes[theme.Key] = themeMean.Value / count;
                }
                return themes;
            }
        }

        public class WordOccurency
        {
            public string Word { get; set; }
            public long ShowCount { get; set; }
            public long FoundCount { get; set; }

            public double Probability {get { return (FoundCount == 0 ? 1e-9 : FoundCount)/(double) ShowCount; }}
            
            public Sample Sample { get
                {
                    var sample = new Sample();
                    for (int i = 0; i < ShowCount - FoundCount; i++) sample.Add(0.0);
                    for (int i = 0; i < FoundCount; i++) sample.Add(1.0);
                    return sample;
                } }
        }

        [TestMethod]
        public void GetMails()
        {
            var mails = EvaluatedResults.SelectMany(er => er.Results.Mails).Distinct();
            foreach (var mail in mails)
            {
                Console.WriteLine(mail);
            }
        }


        [TestMethod]
        public void ComputeScreenSegments()
        {
            ComputeScreenSegments(4, 4);
        }

        private void ComputeScreenSegments(int countX, int countY)
        {
            var segments = new Sample[countX,countY];
            foreach (var result in EvaluatedResults)
            {
               foreach (var wordPos in result.Modality.Positions/*.Where(p => p.Key.Equals("sex") || p.Key.Equals("vagína") || p.Key.Equals("kunda") || p.Key.Equals("piča") || p.Key.Equals("kokot") || p.Key.Equals("čurák"))*/)
               {
                   var center = wordPos.Value.GetCenter();
                   var ix = (int) (center.X/1024.0*countX);
                   var iy = (int)(center.Y / 720.0 * countY);
                   if (segments[ix, iy] == null) segments[ix, iy] = new Sample();
                   segments[ix, iy]. Add(result.RightActiveAnswerDistinct.Contains(wordPos.Key) ? 1.0 : 0.0);
                   
               }
            }
            for (int iy = 0; iy < countY; iy++ )
            {
                for (int ix = 0; ix < countX; ix++)
                {
                    var sample = segments[ix, iy];
                    Console.Write("{0}\t", VirtualTableEx.ConvertToString(sample.PopulationMean));
                }
                Console.WriteLine();
            }
        }

        [TestMethod]
        public void SexBayesianClassificationTest()
        {
            const int testCount = 12;
            var malesTrainee = MaleEvaluatedResults.OrderBy(er => er.Results.Uuid).Skip(testCount);
            var femalesTrainee = FemaleEvaluatedResults.OrderBy(er => er.Results.Uuid).Skip(testCount);
            var test = MaleEvaluatedResults.OrderBy(er => er.Results.Uuid).Take(testCount).Concat(FemaleEvaluatedResults.OrderBy(er => er.Results.Uuid).Take(testCount));
            var maleIndex = Index.CreateMemoryIndex();
            maleIndex.Add(malesTrainee.Select(er => Entry.FromTokens(er.RightActiveAnswerDistinct)));
            var femaleIndex = Index.CreateMemoryIndex();
            femaleIndex.Add(femalesTrainee.Select(er => Entry.FromTokens(er.RightActiveAnswerDistinct)));
            var sample = new Sample();
            foreach (var testResult in test)
            {
                var testEntry = Entry.FromTokens(testResult.RightActiveAnswerDistinct);
                var classification = new Analyzer().Categorize(testEntry, maleIndex, femaleIndex);
                var success = (testResult.Results.IsMale && classification == CategorizationResult.First) || (testResult.Results.IsFemale && classification == CategorizationResult.Second);
                sample.Add(success ? 1.0 : 0.0);
                Console.WriteLine("{3}\t{0}\t{1}\t{2}", testResult.Results.Sex, classification, success, testResult.Results.Name);
            }
            Console.WriteLine(VirtualTableEx.ConvertToString(sample.PopulationMean));
        }

        [TestMethod]
        public void AgeBayesianClassificationTest()
        {
            const int testCount = 16;
            const int limitYear = 1985;
            var malesTrainee = EvaluatedResults.OrderBy(er => er.Results.Uuid).Where(er => er.Results.BirthYear.HasValue && er.Results.BirthYear < limitYear).Skip(testCount);
            var femalesTrainee = EvaluatedResults.OrderBy(er => er.Results.Uuid).Where(er => er.Results.BirthYear.HasValue && er.Results.BirthYear >= limitYear).Skip(testCount);
            var test = EvaluatedResults.OrderBy(er => er.Results.Uuid).Where(er => er.Results.BirthYear.HasValue && er.Results.BirthYear < limitYear).Take(testCount).Concat(EvaluatedResults.OrderBy(er => er.Results.Uuid).Where(er => er.Results.BirthYear.HasValue && er.Results.BirthYear >= limitYear).Take(testCount));
            var maleIndex = Index.CreateMemoryIndex();
            maleIndex.Add(malesTrainee.Select(er => Entry.FromTokens(er.RightActiveAnswerDistinct)));
            var femaleIndex = Index.CreateMemoryIndex();
            femaleIndex.Add(femalesTrainee.Select(er => Entry.FromTokens(er.RightActiveAnswerDistinct)));
            var sample = new Sample();
            foreach (var testResult in test)
            {
                var testEntry = Entry.FromTokens(testResult.RightActiveAnswerDistinct);
                var classification = new Analyzer().Categorize(testEntry, maleIndex, femaleIndex);
                var success = (testResult.Results.BirthYear < limitYear && classification == CategorizationResult.First) || (testResult.Results.BirthYear >= limitYear && classification == CategorizationResult.Second);
                sample.Add(success ? 1.0 : 0.0);
                Console.WriteLine("{3}\t{0}\t{1}\t{2}", testResult.Results.BirthYear, classification, success, testResult.Results.Name);
            }
            Console.WriteLine(VirtualTableEx.ConvertToString(sample.PopulationMean));
        }
    }
}