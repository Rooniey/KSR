using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using Common.Model;

namespace DataSetParser
{
    public static class DataSetReader
    {
        public static readonly int LIMIT_OF_LABELS = 6;

        public static ValueTuple<List<Article>, List<string>> GetArticles(string dirPath, string labelName)
        {
             List<Article> allArticles = Directory.GetFiles(dirPath).Any(f => f.EndsWith(".csv")) ? 
                CsvDataReader.GetAllArticles(dirPath, labelName) : SgmDataReader.GetAllArticles(dirPath, labelName);

             List<string> labelCollection = allArticles.GroupBy(a => a.Label)
                 .OrderByDescending(g => g.Count())
                 .Take(LIMIT_OF_LABELS)
                 .Select(g => g.Key)
                 .ToList();

             List<Article> articlesSelected = allArticles.Where(a => labelCollection.Contains(a.Label))
                 .ToList();

             return (articlesSelected, labelCollection);
        }

    }
}
