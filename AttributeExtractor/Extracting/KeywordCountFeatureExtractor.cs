using System.Collections.Generic;
using System.Linq;
using Common.Model;

namespace AttributeExtractor.Extracting
{
    public class KeywordCountFeatureExtractor : IFeatureExtractor
    {   
        public Dictionary<string, double> ExtractFeatures(Article article, IEnumerable<string> keywords)
        {
            return new Dictionary<string, double>()
            {
                {"count",  article.Tokens.Count(keywords.Contains) }
            };
        }
    }
}
