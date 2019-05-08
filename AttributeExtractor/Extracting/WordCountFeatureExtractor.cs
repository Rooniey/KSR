using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Common.Model;

namespace AttributeExtractor.Extracting
{
    public class WordCountFeatureExtractor : IFeatureExtractor
    {
        public Dictionary<string, double> ExtractFeatures(Article article, IEnumerable<string> keywords)
        {
            return new Dictionary<string, double>()
            {
                {"allWordsCount",  article.Tokens.Count }
            };
        }
    }
}
