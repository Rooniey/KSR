using System;
using System.Collections.Generic;
using System.Linq;
using Common.Model;

namespace AttributeExtractor.Extracting
{
    public class FirstKeywordDistanceFromStartFeatureExtractor : IFeatureExtractor
    {
        public Dictionary<string, double> ExtractFeatures(Article article, IEnumerable<string> keywords)
        {

            string firstKeyword = article.Tokens.FirstOrDefault(keywords.Contains);
            int firstKeywordLength = firstKeyword != null
                ? article.Body.IndexOf(firstKeyword, StringComparison.InvariantCulture)
                : article.Body.Length;

            return new Dictionary<string, double>()
            {
                {"firstKeywordLengthFromBeginning", firstKeywordLength}
            };
        }
    }
}
