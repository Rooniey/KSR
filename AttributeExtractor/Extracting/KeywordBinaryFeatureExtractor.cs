using System.Collections.Generic;
using System.Linq;

namespace AttributeExtractor.Extracting
{
    public class KeywordBinaryFeatureExtractor : IFeatureExtractor
    {

        public Dictionary<string, double> ExtractFeatures(List<string> article, IEnumerable<string> keywords)
        {
            Dictionary<string, double> dictionary = new Dictionary<string, double>();
            foreach (var kw in keywords)
            {
                dictionary[kw] = article.Any(t => t == kw) ? 1d : 0d;
            }

            return dictionary;
        }
    }
}
