namespace Psychex.Logic.Experiments.WordRetrieval
{
    public class IdentifiedWord
    {
        public IdentifiedWord(string answered, string identified)
        {
            Identified = identified;
            Answered = answered;
        }

        public string Answered { get; private set; }
        public string Identified { get; private set; }
        public bool IsIdentified { get { return !string.IsNullOrEmpty(Identified); } }
    }
}