using System;
using System.Linq;
using System.Text;
using HtmlAgilityPack;

namespace DarkSky.Messaging.Extensions {
    public static class StringExtensions {
        /// <summary>
        /// Returns the trimmed version of the specified string. If the specified string is null, an empty string is returned
        /// </summary>
        public static string TrimSafe(this string text) {
            return text != null ? text.Trim() : string.Empty;
        }

        /// <summary>
        /// Strips the specified html string from its tags and returns a plain text string
        /// </summary>
        public static string StripHtml(this string html) {
            var doc = new HtmlDocument();
            doc.LoadHtml(html);
            return doc.DocumentNode.InnerText;
        }

        public static int? AsInt32(this string text) {
            int value;
            if (string.IsNullOrWhiteSpace(text))
                return null;
            if (!int.TryParse(text, out value))
                return null;
            return value;
        }

        /// <summary>
        /// Returns the first non-null or whitespace string
        /// </summary>
        public static string PrioritySelect(params string[] values) {
            return values.FirstOrDefault(x => !string.IsNullOrWhiteSpace(x));
        }

        public static string SanitizeXmlString(this string xml) {
            if(xml == null)
                throw new ArgumentNullException("xml");

            var buffer = new StringBuilder(xml.Length);

            foreach (var c in xml.Where(IsLegalXmlChar))
                buffer.Append(c);

            return buffer.ToString();
        }

        private static bool IsLegalXmlChar(char character) {
            return
                (!(0x0 <= character && character <= 0x8) &&
                 !new[] {0xB, 0xC}.Contains(character) &&
                 !(0xE <= character && character <= 0x1F) &&
                 !(0x7F <= character && character <= 0x84) &&
                 !(0x86 <= character && character <= 0x9F) &&
                 !(0xD800 <= character && character <= 0xDFFF) &&
                 !new[] {0xFFFE, 0xFFFF}.Contains(character));
        }
    }
}