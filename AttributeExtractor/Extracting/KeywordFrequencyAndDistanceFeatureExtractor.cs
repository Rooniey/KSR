using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace AttributeExtractor.Extracting
{
    public class KeywordFrequencyAndDistanceFeatureExtractor : IFeatureExtractor
    {
        public Dictionary<string, double> ExtractFeatures(List<string> article, IEnumerable<string> keywords)
        {
            Dictionary<string, double> dictionary = new Dictionary<string, double>();
            foreach (var kw in keywords)
            {
                var dist = article.IndexOf(kw);
                dictionary[kw] = dist == -1 ? 0 : 10000 - dist;
                dictionary[$"{kw}-f"] = article.Count(t => t == kw);
            }

            return dictionary;
        }
    }
}
