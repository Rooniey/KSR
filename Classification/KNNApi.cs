using System.Collections.Generic;
using System.Linq;
using Classification.distance;
using Classification.domain;
using DataSetParser.Model;

namespace Classification
{
    public static class KNNApi
    {
        public static void UseKNN(List<Article> trainingSet, IMetric metric, int K, List<Article> testSet)
        {
            var centroids = trainingSet
                .Select(ta => new Centroid(ta.FeatureVector.Select(f => f.Value).ToArray(), ta.Label)).ToArray();

            KNNAlgorithm algorithm = new KNNAlgorithm(metric, centroids, K);

            foreach (var vectorizedTestArticle in testSet)
            {
                var x = algorithm.ProcessInput(vectorizedTestArticle.FeatureVector.Select(f => f.Value).ToArray());
                vectorizedTestArticle.Prediction = x;
            }
        }
    }
}
