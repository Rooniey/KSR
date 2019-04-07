using System;
using System.Collections.Generic;
using System.Linq;

namespace AttributeExtractor
{
    public static class TfIdfCalculator
    {
        public static double CalculateTfIdf(string term, List<string> article, List<List<string>> documents, Dictionary<string, double> cachedIdfDictionary)
        {
            double tf = CalculateTermFrequency(term, article);

            if (cachedIdfDictionary.ContainsKey(term))
            {
                return tf / cachedIdfDictionary[term];
            }

            int aggregateCount = 0;

            foreach (var document in documents)
            {
                aggregateCount += document.Exists(t => t == term) ? 1 : 0;
            }

            double idf = Math.Log10(documents.Count / (1.0 + aggregateCount));

            cachedIdfDictionary[term] = idf;

            return tf * idf;
        }

        public static double CalculateTermFrequency(string term, List<string> articleTokens)
        {
            int termsCount = articleTokens.Count(t => t == term);
            return (1.0 * termsCount) / articleTokens.Count;
        }
    }
}
