using System;
using System.Linq;

namespace Classification.distance
{
    public class JaccardMetric : IMetric
    {
        public double CalculateDistance(double[] a, double[] b)
        {
             var minSum = a.Zip(b, Math.Min).Sum();
             var maxSum = a.Zip(b, Math.Max).Sum();

             return 1 - (minSum / maxSum);
        }
    }
}