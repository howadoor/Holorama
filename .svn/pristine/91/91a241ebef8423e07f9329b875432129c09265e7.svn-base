using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Text.RegularExpressions;

namespace Psychex.Logic.Experiments.WordRetrieval
{
    public class ResultsParser
    {
         public static IEnumerable<KeyValuePair<string, string>> Load(string filename)
         {
             using (var fs = new FileStream(filename, FileMode.Open, FileAccess.Read, FileShare.ReadWrite))
             {
                 var reader = new StreamReader(fs, Encoding.UTF8);
                 return Load(reader);
             }
         }

        public static IEnumerable<KeyValuePair<string, string>> Load(StreamReader reader)
        {
            var regex = new Regex(@"<(\w+)>\r\n(.*)(?!\r\n<\w+>\r\n)");
            var fileContent = reader.ReadToEnd();
            bool wasSex = false;
            return regex.Matches(fileContent).Cast<Match>().Select(match => new KeyValuePair<string, string>(match.Groups[1].Value, match.Groups[2].Value.Trim())).TakeWhile(kv => { if (wasSex) return false;
                                                                                                                                                                                       wasSex = kv.Key.Equals("sex");
                                                                                                                                                                                       return true;
            });
        }
    }
}