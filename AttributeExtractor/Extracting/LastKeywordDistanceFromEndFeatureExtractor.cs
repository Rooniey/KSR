using System;
using System.Collections.Generic;
using System.Linq;
using Common.Model;

namespace AttributeExtractor.Extracting
{
    public class LastKeywordDistanceFromEndFeatureExtractor : IFeatureExtractor
    {
        public Dictionary<string, double> ExtractFeatures(Article article, IEnumerable<string> keywords)
        {
            string lastKeyword = article.Tokens.LastOrDefault(keywords.Contains);
            int lastKeywordLength = lastKeyword != null
                ? (article.Body.Length - lastKeyword.Length) -
                  article.Body.LastIndexOf(lastKeyword, StringComparison.InvariantCulture)
                : article.Body.Length;

            return new Dictionary<string, double>()
            {
                {"lastKeywordLengthFromEnd", lastKeywordLength}
            };
        }
    }
}
