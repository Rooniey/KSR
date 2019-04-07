using System;
using System.Linq;

namespace Classification.distance
{
    public class ManhattanMetric : IMetric
    {
        public double CalculateDistance(double[] a, double[] b)
        {
            return a.Zip(b, (x, y) => Math.Abs(x - y)).Sum();
        }
    }
}