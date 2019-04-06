using System.Collections.Generic;
using System.Linq;
using DataSetParser.Model;
using Statistics.model;

namespace Statistics
{
    public static class PerformanceCalculator
    {
        public static PerformanceMeasures CalculatePerformanceMeasures(List<Article> testSet, Dictionary<string, int> labelsCollection)
        {
            int[][] confusionMatrix = CalculateConfusionMatrix(testSet, labelsCollection);
            double precision = CalculatePrecision(confusionMatrix);
            double recall = CalculateRecall(confusionMatrix);
            double specificity = CalculateSpecificity(confusionMatrix);
            double averageAccuracy = CalculateAverageAccuracy(confusionMatrix);

            return new PerformanceMeasures(confusionMatrix,precision, specificity, recall, averageAccuracy);

        }

        private static int[][] CalculateConfusionMatrix(List<Article> testSet, Dictionary<string, int> labelsCollection)
        {
            int numberOfClasses = labelsCollection.Count;
            int[][] intConfusionMatrix = new int[numberOfClasses][];
            for (int i = 0; i < numberOfClasses; i++)
            {
                intConfusionMatrix[i] = new int[numberOfClasses];
            }

            foreach (var article in testSet)
            {
                int row = labelsCollection[article.Label];
                int column = labelsCollection[article.Prediction];
                intConfusionMatrix[row][column]++;
            }

            return intConfusionMatrix;
        }

        private static double CalculatePrecision(int[][] confusionMatrix)
        {
            int l = confusionMatrix[0].Length;
            int ttp = TruePositives(confusionMatrix);
            List<double> classPrecision = new List<double>();

            for (int i = 0; i < l; i++)
            {
                classPrecision.Add(ttp / (1.0 * ttp + FalsePositives(i, confusionMatrix)));
            }

            return classPrecision.Average();
        }

        private static double CalculateRecall(int[][] confusionMatrix)
        {
            int l = confusionMatrix[0].Length;
            int ttp = TruePositives(confusionMatrix);
            List<double> classRecall = new List<double>();

            for (int i = 0; i < l; i++)
            {
                classRecall.Add(ttp / (1.0 * ttp + FalseNegatives(i, confusionMatrix)));
            }

            return classRecall.Average();
        }

        private static double CalculateSpecificity(int[][] confusionMatrix)
        {
            int l = confusionMatrix[0].Length;
            int ttn = Enumerable.Range(0, l).Select(c => TrueNegatives(c, confusionMatrix)).Sum();
            List<double> classSpecificity = new List<double>();

            for (int i = 0; i < l; i++)
            {
                classSpecificity.Add(ttn / (1.0 * ttn + FalsePositives(i, confusionMatrix)));
            }

            return classSpecificity.Average();
        }

        private static double CalculateAverageAccuracy(int[][] confusionMatrix)
        {
            int l = confusionMatrix[0].Length;
            int ttp = TruePositives(confusionMatrix);
            int total = 0;
            for (int i = 0; i < l; i++)
            {
                for (int j = 0; j < l; j++)
                {
                    total += confusionMatrix[i][j];
                }
            }
          

            return (1.0*ttp)/total;
        }

        private static int TruePositives(int[][] confusionMatrix)
        {
            int l = confusionMatrix[0].Length;
            int ttp = 0;

            for (int i = 0; i < l; i++)
            {
                ttp += confusionMatrix[i][i];
            }
            return ttp;
        }

        private static int FalsePositives(int classNumber, int[][] confusionMatrix)
        {
            int l = confusionMatrix[0].Length;
            int tfp = 0;
            for (int i = 0; i < l; i++)
            {
                if (i != classNumber)
                    tfp += confusionMatrix[i][classNumber];
            }
            return tfp;
        }

        private static int FalseNegatives(int classNumber, int[][] confusionMatrix)
        {
            int l = confusionMatrix[0].Length;
            int tfn = 0;
            for (int i = 0; i < l; i++)
            {
                if (i != classNumber)
                    tfn += confusionMatrix[classNumber][i];
            }
            return tfn;
        }

        private static int TrueNegatives(int classNumber, int[][] confusionMatrix)
        {
            int l = confusionMatrix[0].Length;
            int ttn = 0;
            for (int i = 0; i < l; i++)
            {
                for (int j = 0; j < l; j++)
                {
                    if(i != classNumber && j != classNumber)
                        ttn += confusionMatrix[j][i];
                }
            }
            return ttn;
        }
    }
}
