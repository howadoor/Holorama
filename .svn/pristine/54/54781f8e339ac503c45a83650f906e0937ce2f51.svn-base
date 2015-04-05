using System.Collections.Generic;
using System.Linq;

namespace Psychex.Logic.Experiments.WordRetrieval
{
    public class ResultsEvaluator
    {
        public Dictionary<int, Modality> Modalities { get; set; }
        private readonly Dictionary<int, WordIdentificator> Identificators = new Dictionary<int, WordIdentificator>();

        public EvaluatedResults Evaluate(ExperimentResults results)
        {
            var identificator = GetIdentificator(results);
            var identifiedActiveAnswer = results.ActiveAnswersWords.Select(w =>
                {
                    string identified;
                    identificator.TryIdentify(w, out identified);
                    return new IdentifiedWord(w, identified);
                }).ToArray();
            return new EvaluatedResults(results, Modalities[results.Modality], identifiedActiveAnswer);
        }

        private WordIdentificator GetIdentificator(ExperimentResults results)
        {
            WordIdentificator identificator;
            if (!Identificators.TryGetValue(results.Modality, out identificator))
            {
                return (Identificators[results.Modality] = new WordIdentificator(Modalities[results.Modality].Words.UsedWords));
            }
            return identificator;
        }
    }
}