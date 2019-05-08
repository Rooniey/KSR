using System;
using System.Collections.Generic;
using System.Linq;
using System.Text.RegularExpressions;
using Common.Model;

namespace AttributeExtractor.Extracting
{
    public class AverageSentenceLengthFeatureExtractor : IFeatureExtractor
    {
        public Dictionary<string, double> ExtractFeatures(Article article, IEnumerable<string> keywords)
        {
            var sentences = Regex.Split(article.Body, "[.?!]\\s");
            return new Dictionary<string, double>()
            {

                {"averageSentenceLength", sentences.Average(s => s.Length)}
            };
        }
    }
}
