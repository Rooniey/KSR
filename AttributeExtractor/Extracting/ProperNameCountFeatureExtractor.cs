using System.Collections.Generic;
using System.Linq;
using System.Text.RegularExpressions;
using Common.Model;

namespace AttributeExtractor.Extracting
{
    public class ProperNameCountFeatureExtractor : IFeatureExtractor
    {   
        public Dictionary<string, double> ExtractFeatures(Article article, IEnumerable<string> keywords)
        {

            var sentencesCount = Regex.Split(article.Body, @"[.?!](\s|$)").Length;

            var properNames = Regex.Matches(article.Body, @"(?:\s*\b([A-Z][a-z]+)\b)+");


            List<string> ms = new List<string>();
            foreach (Match properName in properNames)
            {
                foreach (Group properNameGroup in properName.Groups)
                {
                    ms.Add(properNameGroup.Value);
                }
            }

            var toRet = ms.Count - sentencesCount;

            return new Dictionary<string, double>()
            {
                {"properNameCount", toRet < 0 ? 0 : toRet }
            };
        }
    }
}
