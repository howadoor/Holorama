using System.Collections.Generic;
using System.Linq;

namespace Psychex.Logic.Experiments.WordRetrieval
{
    public class ValidationResult
    {
        public ValidationResult(IEnumerable<string> errors)
        {
            Errors = errors.ToArray();
        }

        public string[] Errors
        {
            get; private set;
        }

        public bool IsValid { get { return Errors.Length == 0;  } }
    }
}