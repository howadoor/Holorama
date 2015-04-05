using System;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;
using System.Text.RegularExpressions;

namespace Psychex.Logic.Experiments.WordRetrieval
{
    public class ExperimentResults
    {
        private string uuid;
        private int modality = -1;
        private string client;
        private string activeAnswers;
        private string passiveAnswers;
        private string name;
        private string birth;
        private string sex;
        private string contact;
        private string[] activeAnswersWords;
        private string[] passiveAnswersWords;
        private DateTime? startTime;
        private DateTime? endTime;

        public ExperimentResults(KeyValuePair<string, string>[] results)
        {
            Results = results;
        }

        public KeyValuePair<string, string>[] Results { get; private set; }

        public string Uuid
        {
            get
            {
                return uuid ?? (uuid = ValuesOf("uuid").First());
            }
        }

        public string Client
        {
            get
            {
                return client ?? (client = ValuesOf("client").First());
            }
        }

        public int Modality
        {
            get
            {
                return modality < 0 ? (modality = int.Parse(ValuesOf("modality").First())) : modality;
            }
        }

        public string ActiveAnswers
        {
            get
            {
                return activeAnswers ?? (activeAnswers = ValuesOf("1").Single());
            }
        }

        public string PassiveAnswers
        {
            get
            {
                return passiveAnswers ?? (passiveAnswers = ValuesOf("answers").Single());
            }
        }

        public string Name
        {
            get
            {
                return name ?? (name = ValuesOf("name").Single());
            }
        }

        public string Birth
        {
            get
            {
                return birth ?? (birth = ValuesOf("birth").Single());
            }
        }

        public int? BirthYear
        {
            get
            {
                int year;
                if (!int.TryParse(Birth, out year) || year < 1900 || year > 2013) return null;
                return year;
            }
        }

        public string Sex
        {
            get
            {
                return sex ?? (sex = ValuesOf("sex").Single());
            }
        }

        public string Contact
        {
            get
            {
                return contact ?? (contact = ValuesOf("contact").Single());
            }
        }

        public string[] ActiveAnswersWords
        {
            get { return activeAnswersWords ?? (activeAnswersWords = ToWords(ActiveAnswers)); }
        }

        public string[] PassiveAnswersWords
        {
            get { return passiveAnswersWords ?? (passiveAnswersWords = ToWords(PassiveAnswers)); }
        }

        public DateTime StartTime
        {
            get { return (startTime ?? (startTime = ToTime(ValuesOf("time").First()))).Value; }
        }

        public DateTime EndTime
        {
            get { return (endTime ?? (endTime = ToTime(ValuesOf("time").Last()))).Value; }
        }

        public TimeSpan TotalTime
        {
            get { return EndTime - StartTime; }
        }

        public bool IsMale
        {
            get { return "male".Equals(Sex); }
        }

        public bool IsFemale
        {
            get { return "female".Equals(Sex); }
        }

        public IEnumerable<string> Mails
        {
            get { return GetMails(Contact).Concat(GetMails(Name)); }
        }

        private IEnumerable<string> GetMails(string @string)
        {
            if (!string.IsNullOrEmpty(@string))
            {
                var mailRegex = new Regex(@"\b[A-Z0-9._%+-]+@[A-Z0-9.-]+\.[A-Z]{2,4}\b", RegexOptions.IgnoreCase);
                return mailRegex.Matches(@string).Cast<Match>().Select(match => match.Value);
            }
            return new string[]{};
        }

        private DateTime ToTime(string time)
        {
            DateTime dateValue;
            if (!DateTime.TryParseExact(time, "yyyy-MM-dd HH:mm:ss", CultureInfo.InvariantCulture, DateTimeStyles.AssumeLocal, out dateValue)) throw new InvalidOperationException("Cannot parse time");
            return dateValue;
        }

        private string[] ToWords(string @string)
        {
            return @string.Split(new char[] {' ', ',', '\t', '\r', '\n'}, StringSplitOptions.RemoveEmptyEntries);
        }

        private IEnumerable<string> ValuesOf(string key)
        {
            return Results.Where(kv => kv.Key.Equals(key)).Select(kv => kv.Value);
        }
    }
}