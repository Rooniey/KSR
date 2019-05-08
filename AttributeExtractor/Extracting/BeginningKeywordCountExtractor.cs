using System.Collections.Generic;
using System.Linq;
using Common.Model;

namespace AttributeExtractor.Extracting
{
    public class BeginningKeywordCountFeatureExtractor : IFeatureExtractor
    {   
        public Dictionary<string, double> ExtractFeatures(Article article, IEnumerable<string> keywords)
        {
            return new Dictionary<string, double>()
            {
                {"beginningCount",  keywords.Count(article.Tokens.Take(article.Tokens.Count/3).Contains)}
            };
        }
    }
}
