using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.Text.RegularExpressions;
namespace SortGame
{
    public static class StringExtensions
    {
        public static string StripWhiteSpaces(this string text)
        {
            return Regex.Replace(text, @"[^\S\r\n]", string.Empty);
        }
    }
}
