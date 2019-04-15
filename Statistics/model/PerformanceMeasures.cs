namespace Statistics.model
{
    public class PerformanceMeasures
    {
        public double Precision { get; set; }
        public double Specificity { get; set; }
        public double Recall { get; set; }
        public double AverageAccuracy { get; set; }
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
