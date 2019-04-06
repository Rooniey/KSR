using Classification.distance;
using System;
using System.Linq;

namespace Classification.domain
{
    public class KNNAlgorithm
    {
        private readonly IMetric _metric;

        public Centroid[] Centroids { get; }

        public int K { get; set; }

        public KNNAlgorithm(IMetric metric, Centroid[] centroids, int k)
        {
            _metric = metric ?? throw new ArgumentNullException();
            int dimension = centroids[0].Dimensions;
            foreach (var centroid in centroids)
            {
                if (centroid.Dimensions != dimension)
                    throw new ArgumentException("Every centroid must have the same dimensions");
            }

            Centroids = centroids;
            K = k;
        }


        public string ProcessInput(double[] featureVector)
        {
            //TODO what if n = 3, and there is 1 : 1 : 1
            return Centroids
                .Select(centroid => (centroid, _metric.CalculateDistance(centroid.Position, featureVector)))
                .OrderBy(t => t.Item2)
                .Take(K)
                .GroupBy(t => t.Item1.Label)
                .OrderBy(grouping => grouping.Count())
                .Last().Key;
        }
    }
}
