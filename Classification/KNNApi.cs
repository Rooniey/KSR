using System;
using System.Collections.Generic;
using System.Linq;
using Classification.distance;
using Classification.domain;
using Common.Model;
using Common.Utility;

namespace Classification
{
    public static class KNNApi
    {
        public static void UseKNN(List<Article> trainingSet, IMetric metric, int K, List<Article> testSet)
        {
            NormalizeVectors(trainingSet, testSet);

            var centroids = trainingSet
                .Select(ta => new Centroid(ta.FeatureVector.OrderBy(p => p.Key).Select(f => f.Value).ToArray(), ta.Label)).ToArray();

            KNNAlgorithm algorithm = new KNNAlgorithm(metric, centroids, K);

            foreach (var vectorizedTestArticle in testSet)
            {
                var x = algorithm.ProcessInput(vectorizedTestArticle.FeatureVector.OrderBy(p => p.Key).Select(f => f.Value).ToArray());
                vectorizedTestArticle.Prediction = x;
            }
        }

        public static void NormalizeVectors(List<Article> trainingSet, List<Article> testSet)
        {
            Dictionary<string, double> featureMinima = new Dictionary<string, double>();
            Dictionary<string, double> featureMaxima = new Dictionary<string, double>();
            foreach (var article in trainingSet)
            {
                foreach (var pair in article.FeatureVector)
                {
                    featureMinima[pair.Key] =
                        Math.Min(pair.Value, featureMinima.GetOrCreate(pair.Key, pair.Value));

                    featureMaxima[pair.Key] =
                        Math.Max(pair.Value, featureMaxima.GetOrCreate(pair.Key, pair.Value));
                }
            }

            foreach (var article in testSet)
            {
                article.FeatureVector = article.FeatureVector.ToDictionary(pair => pair.Key, pair =>
                    (pair.Value - featureMinima[pair.Key])
                    / (featureMaxima[pair.Key] - featureMinima[pair.Key]));
            }

            foreach (var article in trainingSet)
            {
                article.FeatureVector = article.FeatureVector.ToDictionary(pair => pair.Key, pair =>
                    (pair.Value - featureMinima[pair.Key])
                    / (featureMaxima[pair.Key] - featureMinima[pair.Key]));
            }
        }
    }
}
