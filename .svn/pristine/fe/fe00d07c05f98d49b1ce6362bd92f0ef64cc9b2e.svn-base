using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Text.RegularExpressions;

namespace BayesianTest.BayesianClassificator
{
    public abstract class Entry : IEnumerable<string>, IEnumerable
    {
        #region IEnumerable<string> Members

        public abstract IEnumerator<string> GetEnumerator();

        IEnumerator IEnumerable.GetEnumerator()
        {
            return GetEnumerator();
        }

        #endregion

        public static Entry FromString(string content)
        {
            return new StringEntry(content);
        }

        public static Entry FromTokens(IEnumerable<string> tokens)
        {
            return new TokensEntry(tokens);
        }

        public class TokensEntry : Entry
        {
            private readonly IEnumerable<string> tokens;

            public TokensEntry(IEnumerable<string> tokens)
            {
                this.tokens = tokens;
            }

            public override IEnumerator<string> GetEnumerator()
            {
                return tokens.GetEnumerator();
            }
        }

        #region Nested type: StringEntry

        private class StringEntry : Entry
        {
            private readonly IEnumerable<string> tokens;

            public StringEntry(string stringcontent)
            {
                tokens = Parse(stringcontent);
            }

            public override IEnumerator<string> GetEnumerator()
            {
                return tokens.GetEnumerator();
            }

            private static IEnumerable<string> Parse(string source)
            {
                return (CleanInput(source).Split(new char[1]
                                                     {
                                                         ' '
                                                     })).Where((t => !t.Equals(" ", StringComparison.InvariantCultureIgnoreCase))).Select((Func<string, string>) (t => t.ToLowerInvariant())).Distinct();
            }

            private static string CleanInput(string strIn)
            {
                return Regex.Replace(strIn, "[^\\w\\'@-]", " ");
            }
        }

        #endregion
    }
}