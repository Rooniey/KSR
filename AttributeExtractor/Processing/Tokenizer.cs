using System;
using System.Collections.Generic;
using System.Linq;

namespace AttributeExtractor.Processing
{
    public static class Tokenizer
    {
        public static readonly string[] _separators = new[] { ",", ".", "!", "\'", " ", "\'s", "\"", "\r\n", "\u0003", "\t", "\n", "\r" };

        public static List<string> TokenizeWords(string text)
        {
            
            return text.Split(_separators, StringSplitOptions.RemoveEmptyEntries)
                .Select(s => s.ToLowerInvariant())
                .ToList();
        }
    }
}
