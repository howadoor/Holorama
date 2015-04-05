using System;
using System.Collections.Generic;
using System.Drawing;
using System.Linq;
using Psychex.Logic.Helpers;

namespace Psychex.Logic.Experiments.WordRetrieval
{
    public static class EvaluatedResultsVectorAnalysis
    {
        public static IEnumerable<double> GetAngles(this EvaluatedResults results)
        {
            return GetRunsOfIdentifiedWords(results).SelectMany(wr => GetAnglesOfIdentifiedWordRun(results, wr));
        }
        
        public static IEnumerable<double> GetAnglesDifferencesOfIdentifiedWordRun(this EvaluatedResults results, IdentifiedWord[] words)
        {
            var angles = GetAnglesOfIdentifiedWordRun(results, words).ToArray();
            for (int i = 1; i < angles.Length; i++)
            {
                yield return angles [i] - angles [i-1];
            }
        }

        public static IEnumerable<double> GetAnglesOfIdentifiedWordRun(this EvaluatedResults results, IdentifiedWord[] words)
        {
            for (int i = 1; i < words.Length; i++)
            {
                var prevWord = words[i - 1];
                var currWord = words[i];
                yield return GetAngleOfWords(results, prevWord, currWord);
            }
        }

        public static double GetAngleOfWords(EvaluatedResults results, IdentifiedWord prevWord, IdentifiedWord currWord)
        {
            var previsousPosition = GetPositionOfWord(results, prevWord);
            var currentPosition = GetPositionOfWord(results, currWord);
            var vector = new PointF(currentPosition.X - previsousPosition.X, currentPosition.Y - previsousPosition.Y);
            var angle = Math.Atan2(vector.Y, vector.X) * 180.0 / Math.PI;
            // if (angle > 180.0) angle -= 180.0;
            return angle;
        }

        public static PointF GetPositionOfWord(this EvaluatedResults results, IdentifiedWord word)
        {
            if (!word.IsIdentified) throw new InvalidOperationException("Word is not identified");
            var thePosition = results.Modality.Positions[word.Identified].GetCenter();
            return thePosition;
        }
        
        public static IEnumerable<IdentifiedWord[]> GetRunsOfIdentifiedWords(this EvaluatedResults results)
        {
            var firstIdentifiedIndex = -1;
            for (int i = 0; i < results.IdentifiedActiveAnswer.Length; i++)
            {
                var currWord = results.IdentifiedActiveAnswer[i];
                if (currWord.IsIdentified)
                {
                    if (firstIdentifiedIndex < 0) firstIdentifiedIndex = i;
                }
                else
                {
                    if (firstIdentifiedIndex >= 0)
                    {
                        yield return GetRunsOfIdentifiedWords(results, firstIdentifiedIndex, i);
                    }
                    firstIdentifiedIndex = -1;
                }
            }
            if (firstIdentifiedIndex >= 0)
                yield return GetRunsOfIdentifiedWords(results, firstIdentifiedIndex, results.IdentifiedActiveAnswer.Length);
        }

        private static IdentifiedWord[] GetRunsOfIdentifiedWords(EvaluatedResults results, int from, int to)
        {
            return results.IdentifiedActiveAnswer.Skip(from).Take(to - from).ToArray();
        }
    }
}