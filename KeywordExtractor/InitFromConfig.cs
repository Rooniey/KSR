using System;
using System.Configuration;
using KeywordExtractor.Extracting;

namespace KeywordExtractor
{
    public static class InitFromConfig
    {
        public static KeywordsExtractor GetKeywordExtractor()
        {
            string keywordExtractor = ConfigurationManager.AppSettings["keywordExtractor"];
            string keywordsToCut = ConfigurationManager.AppSettings["keywordsToCut"];
            ITokenValueCalculator tokenValueCalculator = TokenValueCalculatorProvider.GetFeatureExtractorExtractor(keywordExtractor);
            int numberToCut = Int32.Parse(keywordsToCut);
            return new KeywordsExtractor(tokenValueCalculator, numberToCut);
        }
    }
}
