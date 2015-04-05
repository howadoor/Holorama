using System.Collections.Generic;
using System.Linq;

namespace Psychex.Logic.Experiments.WordRetrieval
{
    public class ResultsValidator
    {
        public static ValidationResult Validate(IEnumerable<KeyValuePair<string, string>> results)
        {
            return new ValidationResult(GetValidationErrors(results));
        }

        private static IEnumerable<string> GetValidationErrors(IEnumerable<KeyValuePair<string, string>> results)
        {
            if (!ContainsExactlyOne(results, "uuid")) yield return "There is not exactly one uuid in the results";
            if (!ContainsExactlyOne(results, "modality")) yield return "There is not exactly one modality in the results";
            if (!ContainsExactlyOne(results, "client")) yield return "There is not exactly one client IP in the results";
            if (!ContainsExactlyOne(results, "0") || !ContainsExactlyOne(results, "1")) yield return "There is not exactly one active answer in the results";
            if (!ContainsExactlyOne(results, "answers")) yield return "There is not exactly one passive answer in the results";
            if (!ContainsExactlyOne(results, "name")) yield return "There is not exactly one name in the results";
            if (!ContainsExactlyOne(results, "contact")) yield return "There is not exactly one contact in the results";
            if (!ContainsExactlyOne(results, "sex")) yield return "There is not exactly one sex in the results";
        }

        private static bool ContainsExactlyOne(IEnumerable<KeyValuePair<string, string>> results, string key)
        {
            return Values(results, key).Distinct().Count() == 1;
        }

        private static IEnumerable<string> Values(IEnumerable<KeyValuePair<string, string>> results, string key)
        {
            return results.Where(kv => kv.Key.Equals(key)).Select(kv => kv.Value);
        }
    }
}