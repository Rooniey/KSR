using System;
using System.Linq;

namespace Classification.distance
{
    public class EuclideanMetric : IMetric
    {
        public double CalculateDistance(double[] a, double[] b)
        {
            return Math.Sqrt(a.Zip(b, (x, y) => Math.Pow(x - y, 2)).Sum());
        }
    }
}
