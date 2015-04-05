using System.Linq;
using System.Text;

namespace Textant.Logic.Helpers
{
    /// <summary>
    /// Detects encoding of text
    /// </summary>
    public class EncodingDetector
    {
        /// <summary>
        /// Detects encoding of <see cref="encodedText"/>
        /// </summary>
        /// <param name="encodedText"></param>
        /// <returns></returns>
        public Encoding DetectEncoding(byte[] encodedText)
        {
            Encoding[] testedEncodings = new Encoding[] {Encoding.UTF8, Encoding.GetEncoding(1250)};
            return testedEncodings.SingleOrDefault(testedEncoding => IsEncodedIn(encodedText, testedEncoding));
        }

        /// <summary>
        /// Checks if <see cref="encodedText"/> is encoded in <see cref="testedEncoding"/>
        /// </summary>
        /// <param name="encodedText"></param>
        /// <param name="testedEncoding"></param>
        /// <returns></returns>
        public bool IsEncodedIn(byte[] encodedText, Encoding testedEncoding)
        {
            var text = testedEncoding.GetString(encodedText);
            const string necessaryCharacters = "ěščřžýáíé";
            return necessaryCharacters.All(text.Contains);
        }
    }
}
