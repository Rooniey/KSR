﻿using System;
using System.Linq;
using System.Text.RegularExpressions;

namespace AttributeExtractor
{
    public static class Tokenizer
    {

        public static Token[] TokenizeWords(string text)
        {
            string[] separators = new string[] { ",", ".", "!", "\'", " ", "\'s" };
            return text.Split(separators, StringSplitOptions.RemoveEmptyEntries)
                .Select(s => new Token(s))
                .ToArray();
        }

        public static Token[] TokenizeSentences(string text)
        {
            return Regex.Split(text, @"(?<=[\.!\?])\s+")
                .Select(s => new Token(s))
                .ToArray();
        }
    }
}
