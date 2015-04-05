using System.IO;
using System.Linq;
using System.Text;
using System.Text.RegularExpressions;
using Psychex.Logic.Helpers;

namespace Psychex.Logic.Experiments.WordRetrieval
{
    public static class ModalityEx
    {
        public static Modality Load(string directory, int modality)
        {
            var words = XmlSerialization.LoadFromXml<WordsForExperiment>(Path.Combine(directory, string.Format("modality_words_{0}.xml", modality)));
            var positions = XmlSerialization.LoadFromXml<WordPositions>(Path.Combine(directory, string.Format("modality_{0}.xml", modality)));
            string[] questions;
            using (var fs = new FileStream(Path.Combine(directory, string.Format("modality_{0}.php", modality)), FileMode.Open, FileAccess.Read, FileShare.ReadWrite))
            {
                var reader = new StreamReader(fs, Encoding.UTF8);
                var questionString = reader.ReadToEnd();
                questions = Regex.Matches(questionString, @"'(\w+)',").Cast<Match>().Select(match => match.Groups[1].Value).ToArray();
            }
            return new Modality(words, positions, questions);
        }
    }
}