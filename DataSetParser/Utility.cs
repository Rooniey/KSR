using System;
using System.Collections.Generic;
using DataSetParser.Model;

namespace DataSetParser
{
    public static class Utility
    {
        public static ValueTuple<List<Article>, List<Article>> DivideDataSet(List<Article> allArticles, double trainingSetFraction)
        {
            List<Article> trainingArticles = new List<Article>();
            List<Article> testArticles = new List<Article>();
            for (int i = 0, cutoffIndex = (int)(allArticles.Count * trainingSetFraction); i < allArticles.Count; i++)
            {
                if (i < cutoffIndex)
                    trainingArticles.Add(allArticles[i]);
                else
                    testArticles.Add(allArticles[i]);
            }

            return (trainingArticles, testArticles);
        }

    }
}
