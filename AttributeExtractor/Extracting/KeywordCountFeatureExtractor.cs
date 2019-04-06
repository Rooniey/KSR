using System.Collections.Generic;
using System.Linq;

namespace AttributeExtractor.Extracting
{
    public class KeywordCountFeatureExtractor : IFeatureExtractor
    {
        public Dictionary<string, double> ExtractFeatures(List<string> article, List<List<string>> allArticles, List<string> keywords)
        {
            Dictionary<string, double> dictionary = new Dictionary<string, double>();
            foreach (var kw in keywords)
            {
                dictionary[kw] = article.Count(t => t == kw);
            }

            return dictionary;
        }
    }
}
