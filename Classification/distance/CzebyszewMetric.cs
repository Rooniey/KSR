using System;
using System.Linq;

namespace Classification.distance
{
    public class CzebyszewMetric : IMetric
    {
        public double CalculateDistance(double[] a, double[] b)
        {
            return a.Zip(b, (x, y) => Math.Abs(x - y)).Max();
        }
    }
}