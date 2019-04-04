using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using AttributeExtractor;
using AttributeExtractor.Processing;
using DataSetParser;
using DataSetParser.Model;


namespace ConsoleApp
{
    class Program
    {
        static void Main(string[] args)
        {
            string dirPath = "../../../Data/reuters";
            var listOfFiles = Directory.GetFiles(dirPath).Where(p => Path.GetExtension(p) == ".sgm").ToList();
            string labelName = "PLACES";
//            Console.WriteLine(listOfFiles);
//            foreach (var file in listOfFiles)
//            {
//                var text = File.ReadAllLines(file);
//                using (var writer = new StreamWriter(new FileStream(file, FileMode.OpenOrCreate)))
//                {
//                    writer.WriteLine(text[0]);
//                    writer.WriteLine("<root>");
//                    for (int i = 1; i < text.Length; i++)
//                    {
//                        writer.WriteLine(text[i]);
//                    }
//                    writer.WriteLine("</root>");
//                }

            var articles = new List<LabeledArticle>();
            foreach (var file in listOfFiles)
            {
                articles.AddRange(SgmDataReader.ReadAllSamples(file, labelName));
            }




            SavedMain(articles);
//            var usaArticles = articles.Where(a => a.Label == "usa").ToList();
//
//            List<TokenizedArticle> tokenizedArticles = new List<TokenizedArticle>();
//            List<Token> oneBigUsaArticleTokens = new List<Token>();
//            foreach (var usaArticle in usaArticles)
//            {
//                var art = TextUtility.ReplaceSpecialCharacters(usaArticle.Article.Body);
//                var tokenizeWords = StopWordsFilterProcessor.Process(Tokenizer.TokenizeWords(art));
//                tokenizedArticles.Add(new TokenizedArticle(usaArticle, tokenizeWords));
//
//                oneBigUsaArticleTokens.AddRange(tokenizeWords);
//            }
//            TokenizedArticle BigUsaArticle = new TokenizedArticle(new LabeledArticle(new Article(), ""), oneBigUsaArticleTokens);
//
//
//
//            List<TokenizedArticle> allTokenizedArticles = new List<TokenizedArticle>();
//            foreach (var article in articles)
//            {
//                var art = TextUtility.ReplaceSpecialCharacters(article.Article.Body);
//                var tokenizeWords = StopWordsFilterProcessor.Process(Tokenizer.TokenizeWords(art));
//                allTokenizedArticles.Add(new TokenizedArticle(article, tokenizeWords));
//            }
//
//            //            Dictionary<string, int> countDictionary = new Dictionary<string, int>();
//            //            foreach (var tokenizedArticle in tokenizedArticles)
//            //            {
//            //                foreach (var token in tokenizedArticle.Tokens)
//            //                {
//            //                    TfIdfCalculator.CalculateTfIdf(token, tokenizedArticle)
//            //                }
//            //            }
//
//            Dictionary<string, double> tfIdfDictionary = new Dictionary<string, double>();
//            foreach (var token in oneBigUsaArticleTokens)
//            {
//                if (!tfIdfDictionary.ContainsKey(token.Word))
//                {
//                    var val = TfIdfCalculator.CalculateTfIdf(token, BigUsaArticle, allTokenizedArticles);
//                    tfIdfDictionary[token.Word] = val;
//                    Console.WriteLine($"{token.Word} -- {val}");
//                }
//            }
//
//            var sorted = tfIdfDictionary.OrderByDescending(pair => pair.Value).ToList();


            
        }

        private static Dictionary<string, int> GetMostFrequentTermsForLabel(List<LabeledArticle> articles, string label, int termCount = 20, string[] stopList = null)
        {
            List<TokenizedArticle> tokenizedArticles = new List<TokenizedArticle>();
//            List<TokenizedArticle> allTokenizedArticles = new List<TokenizedArticle>();

            foreach (var article in articles)
            {
//                allTokenizedArticles.Add(tokenized);
                if (article.Label == label)
                {
                    var art = TextUtility.ReplaceSpecialCharacters(article.Article.Body);
                    var processedWords = StopWordsFilterProcessor.Process(Tokenizer.TokenizeWords(art), stopList);
                    processedWords = Lemmatizer.Process(processedWords);
                    var tokenized = new TokenizedArticle(article, processedWords);
                    tokenizedArticles.Add(tokenized);
                }
                    
            }


            Dictionary<string, int> countDictionary = new Dictionary<string, int>();
            foreach (var tokenizedArticle in tokenizedArticles)
            {
                foreach (var token in tokenizedArticle.Tokens)
                {
                    if (countDictionary.ContainsKey(token.Word))
                    {
                        countDictionary[token.Word]++;
                    }
                    else
                    {
                        countDictionary[token.Word] = 1;
                    }
                }
            }

            return countDictionary
                .OrderByDescending(pair => pair.Value)
                .Take(termCount)
                .ToDictionary(pair => pair.Key, pair => pair.Value);
        }

        private static Dictionary<string, int> GetMostFrequentTerms(List<LabeledArticle> articles, int termCount = 20)
        {
            List<TokenizedArticle> allTokenizedArticles = new List<TokenizedArticle>();

            foreach (var article in articles)
            {
                var art = TextUtility.ReplaceSpecialCharacters(article.Article.Body);
                var processedWords = StopWordsFilterProcessor.Process(Tokenizer.TokenizeWords(art));
                processedWords = Lemmatizer.Process(processedWords);
                var tokenized = new TokenizedArticle(article, processedWords);
                allTokenizedArticles.Add(tokenized);

            }


            Dictionary<string, int> countDictionary = new Dictionary<string, int>();
            foreach (var tokenizedArticle in allTokenizedArticles)
            {
                foreach (var token in tokenizedArticle.Tokens)
                {
                    if (countDictionary.ContainsKey(token.Word))
                    {
                        countDictionary[token.Word]++;
                    }
                    else
                    {
                        countDictionary[token.Word] = 1;
                    }
                }
            }

            return countDictionary
                .OrderByDescending(pair => pair.Value)
                .Take(termCount)
                .ToDictionary(pair => pair.Key, pair => pair.Value);
        }

        private static void SavedMain(List<LabeledArticle> articles)
        {
            var mostFrequentWords = GetMostFrequentTerms(articles, 5).Select(d => d.Key).ToArray();
            var dictionary = GetMostFrequentTermsForLabel(articles, "usa", 30, mostFrequentWords);
            var d1 = GetMostFrequentTermsForLabel(articles, "west-germany",30, mostFrequentWords);
            var d2 = GetMostFrequentTermsForLabel(articles, "france",30, mostFrequentWords);
            var d3 = GetMostFrequentTermsForLabel(articles, "uk",30, mostFrequentWords);
            var d4 = GetMostFrequentTermsForLabel(articles, "canada", 30, mostFrequentWords);
            var d5 = GetMostFrequentTermsForLabel(articles, "japan", 30, mostFrequentWords);


            Console.WriteLine(dictionary);

        }
    }
}

