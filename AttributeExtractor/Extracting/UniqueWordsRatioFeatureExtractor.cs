using System.Collections.Generic;
using System.Linq;
using Common.Model;

namespace AttributeExtractor.Extracting
{
    public class UniqueWordsRatioFeatureExtractor : IFeatureExtractor
    {
        public Dictionary<string, double> ExtractFeatures(Article article, IEnumerable<string> keywords)
        {
            var uniqueCount = article.Tokens
                .GroupBy(x => x, (key, g) => new { Id = key, Count = g.Count() })
                .Count(x => x.Count == 1);


            return new Dictionary<string, double>()
            {

                {"uniqueWordsRatio", (1.0 * uniqueCount)/article.Tokens.Count}
            };
        }
    }

}
