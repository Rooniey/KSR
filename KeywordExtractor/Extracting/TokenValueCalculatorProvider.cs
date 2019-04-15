using System;
using System.Collections.Generic;

namespace KeywordExtractor.Extracting
{
    public static class TokenValueCalculatorProvider
    {
        public static readonly List<string> AVAILABLE_TOKEN_VALUE_CALCULATORS = new List<string>() { "COUNT", "TFIDF" };

        public static ITokenValueCalculator GetFeatureExtractorExtractor(string name)
        {
            if (name == null || string.IsNullOrEmpty(name))
                throw new ArgumentNullException(nameof(name));

            string parsedName = name.ToUpperInvariant();

            switch (parsedName)
            {
                case "COUNT":
                    return new TokenCountCalculator();
                case "TFIDF":
                    return new TfIdfTokenCalculator();
                default:
                    throw new ArgumentException("Unknown token value calculator");
            }
        }
    }
}
