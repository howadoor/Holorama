using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Psychex.Logic.Experiments.WordRetrieval
{
    public class EvaluatedResults
    {
        private static readonly Dictionary<int, double[]> SelectionProbabilities = new Dictionary<int, double[]>();

        public EvaluatedResults(ExperimentResults results, Modality modality, IdentifiedWord[] identifiedActiveAnswer)
        {
            IdentifiedActiveAnswer = identifiedActiveAnswer;
            Modality = modality;
            Results = results;
            ActiveAnswerWordSelectionProbability = GetSelectionProbability(modality.Words.UsedWords.Length)[RightActiveAnswerDistinct.Count()];
        }

        public ExperimentResults Results { get; private set; }
        public Modality Modality { get; private set; }
        public IdentifiedWord[] IdentifiedActiveAnswer { get; private set; }
        public double ActiveAnswerWordSelectionProbability { get; private set; }

        public IEnumerable<string> RightActiveAnswerDistinct
        {
            get { return IdentifiedActiveAnswer.Where(iw => iw.IsIdentified).Select(iw => iw.Identified).Distinct(); }
        }

        public string IdentifiedActiveAnswerDisplayString
        {
            get
            {
                var builder = new StringBuilder();
                foreach (var word in IdentifiedActiveAnswer)
                {
                    if (builder.Length > 0) builder.Append(" ");
                    if (word.IsIdentified)
                        builder.Append(word.Identified);
                    else
                        builder.AppendFormat("~{0}", word.Answered);
                }
                return builder.ToString();
            }
        }

        private double[] GetSelectionProbability(int totalCount)
        {
            double[] probabilities;
            if (!SelectionProbabilities.TryGetValue(totalCount, out probabilities))
            {
                probabilities = new double[totalCount + 1];
                probabilities[0] = 1.0;
                for (var i = 0; i < totalCount; i++)
                {
                    probabilities[i + 1] = probabilities[i]*(totalCount - i - 1)/(totalCount - i);
                }
                SelectionProbabilities[totalCount] = probabilities;
            }
            return probabilities;
        }
    }
}