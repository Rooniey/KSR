using System.Collections.Generic;
using System.Linq;

namespace AttributeExtractor.Extracting
{
    public class KeywordFrequencyExtractor : IFeatureExtractor
    {
        public Dictionary<string, double> ExtractFeatures(List<string> article, IEnumerable<string> keywords)
        {
            Dictionary<string, double> dictionary = new Dictionary<string, double>();
            foreach (var kw in keywords)
            {
                dictionary[kw] = (double)article.Count(t => t == kw) / article.Count;
            }

            return dictionary;
        }
    }
}
