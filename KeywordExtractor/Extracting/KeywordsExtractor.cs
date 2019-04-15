using System;
using System.Collections.Generic;
using System.Linq;
using AttributeExtractor.Processing;
using Common.Model;
using Common.Utility;

namespace KeywordExtractor.Extracting
{
    public class KeywordsExtractor
    {
        private readonly ITokenValueCalculator _tokenValueCalculator;

        private readonly int _mostFrequentTermsToCutCount;

        public KeywordsExtractor(ITokenValueCalculator tokenValueCalculator, int mostFrequentTermsToCutCount = 0)
        {
            _tokenValueCalculator =
                tokenValueCalculator ?? throw new ArgumentNullException(nameof(tokenValueCalculator));

            _mostFrequentTermsToCutCount = mostFrequentTermsToCutCount;
        }

        public List<string> ExtractKeywords(List<Article> trainingSet, List<string> labels, int keywordCount = 20)
        {
            return labels
                .Select(label => GetMostFrequentTermsForLabel(trainingSet, label, keywordCount))
                .Aggregate((sum, next) =>
                {
                    foreach (var kw in next.Keys)
                    {
                        sum.AddOrCreate(kw, next[kw]);
                    }

                    return sum;
                })
                .OrderByDescending(pair => pair.Value)
                .Skip(_mostFrequentTermsToCutCount)
                .Select(pair => pair.Key)
                .Distinct()
                .ToList();
        }

        private Dictionary<string, double> GetMostFrequentTermsForLabel(List<Article> articles, string label, int termCount = 20)
        {
            List<Article> tokenizedArticles = articles.Where(a => a.Label == label).ToList();

            Console.WriteLine($"Label: {label}");

            Dictionary<string, double> countDictionary = new Dictionary<string, double>();
            int i = 0;
            foreach (var tokenizedArticle in tokenizedArticles)
            {
                Console.WriteLine(++i);
                foreach (var token in tokenizedArticle.Tokens)
                {
                    countDictionary.AddOrCreate(token, _tokenValueCalculator.Calculate(token, tokenizedArticle.Tokens, articles.Select(a => a.Tokens).ToList()));
                }
            }

            return countDictionary
                .OrderByDescending(pair => pair.Value)
                .Take(termCount)
                .ToDictionary(pair => pair.Key, pair => pair.Value);
        }


        public List<string> GetMostFrequentTerms(List<Article> tokenizedArticles)
        {
            Dictionary<string, int> countDictionary = new Dictionary<string, int>();

            if (_mostFrequentTermsToCutCount > 0)
            {
                RemoveMostFrequentKeywords(tokenizedArticles);
            }


            foreach (var tokenizedArticle in tokenizedArticles)
            {
                foreach (var token in tokenizedArticle.Tokens)
                {
                    countDictionary.AddOrCreate(token, 1);
                }
            }

            return countDictionary
                .OrderByDescending(pair => pair.Value)
                .Take(_mostFrequentTermsToCutCount)
                .Select(pair => pair.Key)
                .ToList();
        }

        private void RemoveMostFrequentKeywords(List<Article> tokenizedArticles)
        {
            var mostFrequentWords = GetMostFrequentTerms(tokenizedArticles);

            var postProcessor = new StopWordsFilterProcessor(mostFrequentWords);

            foreach (var tokenizedArticle in tokenizedArticles)
            {
                tokenizedArticle.Tokens = postProcessor.Process(tokenizedArticle.Tokens);
            }
        }
    }
}
