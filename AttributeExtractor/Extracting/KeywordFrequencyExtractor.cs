﻿using System.Collections.Generic;
using System.Linq;
using Common.Model;

namespace AttributeExtractor.Extracting
{
    public class KeywordFrequencyExtractor : IFeatureExtractor
    {
        public Dictionary<string, double> ExtractFeatures(Article article, IEnumerable<string> keywords)
        {
            return new Dictionary<string, double>()
            {
                {"keywordFrequency",  (1.0 * article.Tokens.Count(keywords.Contains) ) / article.Tokens.Count}
            };
        }
    }
}
