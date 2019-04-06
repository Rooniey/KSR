using System;
using System.Collections.Generic;
using System.Linq;
using System.Text.RegularExpressions;

namespace AttributeExtractor
{
    public static class Tokenizer
    {

        public static List<Token> TokenizeWords(string text)
        {
            string[] separators = new string[] { ",", ".", "!", "\'", " ", "\'s", "\"" };
            return text.Split(separators, StringSplitOptions.RemoveEmptyEntries)
                .Select(s => new Token(s.ToLowerInvariant()))
                .ToList();
        }

        public static List<Token> TokenizeSentences(string text)
        {
            return Regex.Split(text, @"(?<=[\.!\?])\s+")
                .Select(s => new Token(s.ToLowerInvariant()))
                .ToList();
        }
    }
}
