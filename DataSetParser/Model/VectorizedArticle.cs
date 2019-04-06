using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DataSetParser.Model
{
    public class VectorizedArticle
    {
        public TokenizedArticle Article { get; set; }

        public Dictionary<string, double> FeatureVector { get; set; }

        public string Prediction { get; set; }

        public VectorizedArticle(TokenizedArticle article, Dictionary<string, double> featureVector)
        {
            Article = article;
            FeatureVector = featureVector;
        }
    }
}
