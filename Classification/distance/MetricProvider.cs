using System;
using System.Collections.Generic;

namespace Classification.distance
{
    public static class MetricProvider
    {
        public static readonly List<string> AVAILABLE_METRICS = new List<string>() { "EUCLIDEAN", "CZEBYSZEW", "JACCARD", "MANHATTAN" };


        public static IMetric GetMetric(string name)
        {
            if (name == null || string.IsNullOrEmpty(name))
                throw new ArgumentNullException(nameof(name));

            string parsedName = name.ToUpperInvariant();

            switch (parsedName)
            {
                case "EUCLIDEAN":
                    return new EuclideanMetric();
                case "CZEBYSZEW":
                    return new CzebyszewMetric();
                case "MANHATTAN":
                    return new ManhattanMetric();
                case "JACCARD":
                    return new JaccardMetric();
                default:
                    throw new ArgumentException("Unknown metric");
            }
        }
    }
}
