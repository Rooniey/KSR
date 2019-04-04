using System;
using System.Collections.Generic;
using System.Linq;
using DataSetParser.Model;

namespace AttributeExtractor
{
    public class TfIdfCalculator
    {

        public static double CalculateTfIdf(string term, List<string> article, List<List<string>> documents)
        {
            double tf = CalculateTermFrequency(term, article);
            int aggregateCount = 0;

            foreach (var document in documents)
            {
                aggregateCount += document.Exists(t => t == term) ? 1 : 0;
            }

            double idf = Math.Log10(documents.Count / (1.0 + aggregateCount));

            return tf / idf;
        }

        public static double CalculateTermFrequency(string term, List<string> articleTokens)
        {
            int termsCount = articleTokens.Count(t => t == term);
            return (1.0 * termsCount) / articleTokens.Count;
        }

        public static double[] ExtractTfIdfVector(TokenizedArticle article, List<TokenizedArticle> documents)
        {
            return article.Tokens.Select(t => CalculateTfIdf(t.Word, article.Tokens.Select(at => at.Word).ToList(), documents.Select(d => d.Tokens.Select(at => at.Word).ToList()).ToList())).ToArray();
        }

        public static double[] ONLY_TF_ExtractNGramTfIdfVector(TokenizedArticle article, List<TokenizedArticle> documents, int n = 3)
        {
            double[] resultVector = new double[article.Tokens.Count-n];
            List<string> nGrams = new List<string>();
            for (int i = 0; i < article.Tokens.Count - n; i++)
            {
                var ngramToken = String.Join(" ", article.Tokens.Select(t => t.Word).Skip(i).Take(n).ToArray());
                nGrams.Add(ngramToken);
                
            }
            //todo ADD KEYWORDS and maybe dict instead of aray
//            var documentsTokens = documents.Select(d => d.Tokens.Select(at => at.Word).ToList()).ToList();
            return nGrams.Select(g => CalculateTermFrequency(g, nGrams)).ToArray();

        }

    }
}
