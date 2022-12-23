using System.Text.RegularExpressions;
namespace SortGame
{
    /// <summary>
    /// Extension methods for strings.
    /// </summary>
    public static class StringExtensions
    {
        /// <summary>
        /// Remove white spaces, excluding newlines, from the given string.
        /// </summary>
        /// <returns>A copy with no white space.</returns>
        public static string StripWhiteSpaces(this string text)
        {
            return Regex.Replace(text, @"[^\S\r\n]", string.Empty);
        }
    }
}
