using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Text.RegularExpressions;

namespace Textant.Logic
{
    public class TextParser
    {
        private Regex restOfWord = new Regex(@"-[\r\n]+\s*(\w+)");
        
        /// <summary>
        /// Parses <see cref="text"/> to sequence of words
        /// </summary>
        /// <param name="text"></param>
        /// <returns></returns>
        public IEnumerable<string> ParseWords(string text)
        {
            for (int charIndex = 0; charIndex < text.Length; charIndex++)
            {
                var character = text[charIndex];
                if (char.IsLetter(character))
                {
                    var startIndex = charIndex;
                    for (; charIndex < text.Length && char.IsLetter(text[charIndex]); charIndex++);
                    var @word = text.Substring(startIndex, charIndex - startIndex);
                    if (charIndex < text.Length - 1 && text[charIndex] == '-' && (text[charIndex + 1] == '\r' || text[charIndex + 1] == '\n'))
                    {
                        var match = restOfWord.Match(text, charIndex);
                        if (match.Success)
                        {
                            @word += match.Groups[1].Value;
                            charIndex += match.Length;
                        }
                    }
                    yield return @word.ToLower();
                    charIndex--;
                }
            }
        }

        /// <summary>
        /// Parses <see cref="text"/> to normalized sequence of characters
        /// </summary>
        /// <param name="text"></param>
        /// <returns></returns>
        public IEnumerable<char> ParseCharacters(string text)
        {
            for (int charIndex = 0; charIndex < text.Length; charIndex++)
            {
                var character = text[charIndex];
                if (char.IsLetter(character))
                {
                    var startIndex = charIndex;
                    for (; charIndex < text.Length && char.IsLetter(text[charIndex]); charIndex++) ;
                    var @word = text.Substring(startIndex, charIndex - startIndex);
                    if (charIndex < text.Length - 1 && text[charIndex] == '-' && (text[charIndex + 1] == '\r' || text[charIndex + 1] == '\n'))
                    {
                        var match = restOfWord.Match(text, charIndex);
                        if (match.Success)
                        {
                            @word += match.Groups[1].Value;
                            charIndex += match.Length;
                        }
                    }
                    foreach (var @char in @word) yield return @char;
                    charIndex--;
                    continue;
                }
                if (char.IsWhiteSpace(character))
                {
                    var startIndex = charIndex;
                    for (; charIndex < text.Length && char.IsWhiteSpace(text[charIndex]); charIndex++) ;
                    var whiteSpaces = text.Substring(startIndex, charIndex - startIndex);
                    yield return whiteSpaces.Contains('\n') ? '\n' : ' ';
                    charIndex--;
                    continue;
                }
                yield return character;
            }
        }
    }
}
