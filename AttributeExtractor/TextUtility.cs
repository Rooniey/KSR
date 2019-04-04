using System.Text.RegularExpressions;

namespace AttributeExtractor
{
    public static class TextUtility
    {
        public static string ReplaceSpecialCharacters(string text)
        {
            return Regex.Replace(text, @"[\t(\r\n)\u0003]", " ");
        }
    }
}
