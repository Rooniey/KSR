namespace Statistics.model
{
    public class PerformanceMeasures
    {
        public double Precision { get; }
        public double Specificity { get; }
        public double Recall { get; }
        public double AverageAccuracy { get; }

        public PerformanceMeasures(double precision, double specificity, double recall, double averageAccuracy)
        {
            Precision = precision;
            Specificity = specificity;
            Recall = recall;
            AverageAccuracy = averageAccuracy;
        }
    }
}
