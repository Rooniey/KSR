using System.Collections.Generic;
using System.IO;
using System.Linq;
using DataSetParser.Model;

namespace DataSetParser
{
    public class CsvDataReader
    {
        public static List<Article> GetAllArticles(string dirPath, string labelName)
        {
            return Directory.GetFiles(dirPath)
                .Where(p => Path.GetExtension(p) == ".csv")
                .SelectMany(f => ReadAllSamples(f, labelName))
                .ToList();
        }

        public static List<Article> ReadAllSamples(string filePath, string label)
        {
            List<Article> articles = new List<Article>();

            using (var reader = new StreamReader(filePath))
            {
                reader.ReadLine();
                while (!reader.EndOfStream)
                {
                    var line = reader.ReadLine();
                    var values = line.Split(',');

                    articles.Add(new Article(values[1], values[0]));
                }
            }

            return articles;
        }
    }
}