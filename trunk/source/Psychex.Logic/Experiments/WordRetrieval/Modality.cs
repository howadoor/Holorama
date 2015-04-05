using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Psychex.Logic.Experiments.WordRetrieval
{
    public class Modality
    {
        public Modality(WordsForExperiment words, WordPositions positions, string[] questions)
        {
            Words = words;
            Positions = positions;
            Questions = questions;
        }

        public WordsForExperiment Words { get; private set; }
        public WordPositions Positions { get; private set; }
        public string[] Questions { get; private set; }
    }
}
