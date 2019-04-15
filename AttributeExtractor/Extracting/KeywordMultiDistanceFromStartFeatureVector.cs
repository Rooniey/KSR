﻿using System.Collections.Generic;
using System.Linq;

namespace AttributeExtractor.Extracting
{
    public class KeywordMultiDistanceFromStartFeatureExtractor : IFeatureExtractor
    {
        public Dictionary<string, double> ExtractFeatures(List<string> article, IEnumerable<string> keywords)
        {
            Dictionary<string, double> dictionary = new Dictionary<string, double>();
            foreach (var kw in keywords)
            {
                var dist = article.IndexOf(kw);
                dictionary[kw] = dist == -1 ? 1000000 : (dist / (double)article.Count(t => t == kw));
            }

            return dictionary;
        }
    }
}