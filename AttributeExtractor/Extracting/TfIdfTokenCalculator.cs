using System.Collections.Generic;

namespace AttributeExtractor.Extracting
{
    public class TfIdfTokenCalculator : ITokenValueCalculator
    {
        private readonly Dictionary<string, double> _cache;

        public TfIdfTokenCalculator()
        {
            _cache = new Dictionary<string, double>();
        }

        public double Calculate(string token, List<string> articleWords, List<List<string>> allArticleWords)
        {
            return TfIdfCalculator.CalculateTfIdf(token, articleWords, allArticleWords, _cache);
        }
    }
}
