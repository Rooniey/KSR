using System;
using System.Collections.Generic;

namespace AttributeExtractor.Processing
{
    public static class WordComparatorProvider
    {
        public static readonly List<string> AVAILABLE_WORD_COMPARATORS = new List<string>() { "LEMMATIZATION", "STEMMING" };

        public static ITokenProcessor GetWordComparator(string name)
        {
            if (name == null || string.IsNullOrEmpty(name))
                throw new ArgumentNullException(nameof(name));

            string parsedName = name.ToUpperInvariant();

            switch (parsedName)
            {
                case "LEMMATIZATION":
                    return new Lemmatizer();
                case "STEMMING":
                    return new Stemmer();
                default:
                    throw new ArgumentException("Unknown word comparator");
            }
        }

    }
}
