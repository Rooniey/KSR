using System.Collections.Generic;

namespace AttributeExtractor.Extracting
{
    public class TokenCountCalculator : ITokenValueCalculator
    {
        public double Calculate(string token, List<string> articleWords, List<List<string>> allArticleWords)
        {
            return 1;
        }
    }
}
