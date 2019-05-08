using System;
using System.Collections.Generic;
using System.Linq;
using Common.Model;

namespace AttributeExtractor.Extracting
{
    public class AverageWordLengthFeatureExtractor : IFeatureExtractor
    {
        public Dictionary<string, double> ExtractFeatures(Article article, IEnumerable<string> keywords)
        {

            return new Dictionary<string, double>()
            {

                {"averageWordLength", article.Tokens.Average(t => t.Length)}
            };
        }
    }
}
