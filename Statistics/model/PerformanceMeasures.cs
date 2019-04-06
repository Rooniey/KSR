namespace Statistics.model
{
    public class PerformanceMeasures
    {
        public double Precision { get; }
        public double Specificity { get; }
        public double Recall { get; }
        public double AverageAccuracy { get; }
        public int[][] ConfusionMatrix { get; set; }

        public PerformanceMeasures(int[][] confusionMatrix, double precision, double specificity, double recall, double averageAccuracy)
        {
            ConfusionMatrix = confusionMatrix;
            Precision = precision;
            Specificity = specificity;
            Recall = recall;
            AverageAccuracy = averageAccuracy;
        }
    }
}
