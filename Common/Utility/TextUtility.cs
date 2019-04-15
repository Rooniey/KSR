using System.Text.RegularExpressions;

namespace Common.Utility
{
    public static class TextUtility
    {
        public static string ReplaceSpecialCharacters(this string text)
        {
            return Regex.Replace(text, @"[\t(\r\n)\u0003]", " ");
        }
    }
}
