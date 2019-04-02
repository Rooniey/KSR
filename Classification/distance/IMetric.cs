namespace Classification.distance
{
    public interface IMetric
    {
        double CalculateDistance(double[] a, double[] b);
    }
}
