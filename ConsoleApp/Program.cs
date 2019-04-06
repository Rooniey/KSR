using System;
using System.Collections.Generic;
using System.Linq;
using AttributeExtractor.Extracting;
using AttributeExtractor.Processing;
using Classification;
using Classification.distance;
using DataSetParser;
using DataSetParser.Model;
using Statistics;
using Statistics.model;


namespace ConsoleApp
{
    class Program
    {
        //TODO custom data set
        static void Main(string[] args)
        {
            string dirPath = "../../../Data/reuters";
            
            string labelName = "PLACES";
            List<string> labels = Constants.PLACES.ToList();
            var labelsCollection = Enumerable.Range(0, labels.Count).ToDictionary(i => labels[i], i => i);

            var wordComparator = new Stemmer();

            double trainingSetFraction = 0.6;
            int mostFrequentTermsToCutCount = 5;

            IMetric metric = new EuclideanMetric();
            int K = 3;

            KeywordsExtractor keywordsExtractor = new KeywordsExtractor(new TokenCountCalculator(), mostFrequentTermsToCutCount);

            var allArticles = SgmDataReader.GetArticles(dirPath, labelName);
            foreach (var article in allArticles)
            {
                article.Tokens = Tokenizer.TokenizeWords(article.Body);
            }
            

            List<ITokenProcessor> preProcessors = new List<ITokenProcessor>()
            {
                new StopWordsFilterProcessor(),
                wordComparator
            };

            foreach (var article in allArticles)
            {
                foreach (var tokenProcessor in preProcessors)
                {
                    article.Tokens = tokenProcessor.Process(article.Tokens);
                }
            }

            var (trainingSet, testSet) = Utility.DivideDataSet(allArticles, trainingSetFraction);

            var keywords = keywordsExtractor.ExtractKeywords(trainingSet, labels);


            IFeatureExtractor featureExtractor = new KeywordCountFeatureExtractor();


            foreach (var article in testSet)
            {
                article.FeatureVector = featureExtractor.ExtractFeatures(article.Tokens, allArticles.Select(a => a.Tokens).ToList(), keywords);
            }

            foreach (var article in trainingSet)
            {
                article.FeatureVector = featureExtractor.ExtractFeatures(article.Tokens, allArticles.Select(a => a.Tokens).ToList(), keywords);
            }

            KNNApi.UseKNN(trainingSet, metric, K, testSet);

            PerformanceMeasures measures = PerformanceCalculator.CalculatePerformanceMeasures(testSet, labelsCollection);

            Console.WriteLine("asdasdas");

        }
    }
}

