using System;
using System.Collections.Generic;
using System.Configuration;
using System.Linq;
using AttributeExtractor.Processing;
using Common.Model;
using DataSetParser;
using KeywordExtractor.Extracting;

namespace KeywordExtractor
{
    class Program
    {
        public static List<string> PossibleLabels = new List<string>() { "TOPICS", "PLACES" };

        static void Main(string[] args)
        {
            bool isCustomDataSet = Boolean.Parse(ConfigurationManager.AppSettings["isCustomDataSet"]);
            int keywordsPerLabel = Int32.Parse(ConfigurationManager.AppSettings["keywordsPerLabel"]);
            int keywordsToCut = Int32.Parse(ConfigurationManager.AppSettings["keywordsToCut"]);
            string wordComparator = ConfigurationManager.AppSettings["wordComparator"];
            string label = ConfigurationManager.AppSettings["labels"];
            string keywordsFilePath = ConfigurationManager.AppSettings["keywordsFilePath"];
            string keywordsFileName = $"{keywordsFilePath}{(isCustomDataSet ? "custom" : label)}Keywords(L={keywordsPerLabel}C={keywordsToCut}WC={wordComparator}).txt";


            if(!PossibleLabels.Contains(label))
                throw new ArgumentException("Unknown label");

            string dataSetDirectory = isCustomDataSet ? "../../../Data/custom" : "../../../Data/reuters";
            Console.WriteLine("start...");
            KeywordsExtractor keywordsExtractor = InitFromConfig.GetKeywordExtractor();
            var (allArticles, labels) = DataSetReader.GetArticles(dataSetDirectory, label);
            Console.WriteLine(" tokenizing..");

            foreach (var article in allArticles)
            {
                article.Tokens = Tokenizer.TokenizeWords(article.Body);
            }

            List<ITokenProcessor> preProcessors = new List<ITokenProcessor>()
            {
                new StopWordsFilterProcessor(),
                WordComparatorProvider.GetWordComparator(wordComparator)
            };
            Console.WriteLine("processing...");
            foreach (var article in allArticles)
            {
                foreach (var tokenProcessor in preProcessors)
                {
                    article.Tokens = tokenProcessor.Process(article.Tokens);
                }
            }

//            var (trainingSet, testSet) = Utility.DivideDataSet(allArticles, TrainingSetFraction);


            //X
            Console.WriteLine("Extracting keywords...");
            var keywords = keywordsExtractor.ExtractKeywords(allArticles, labels, keywordsPerLabel);

            KeywordsAccessObject.SaveKeywords(keywords, keywordsFileName);

            
        }
    }
}
