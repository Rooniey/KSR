using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Threading.Tasks;
using System.Windows.Input;
using AttributeExtractor.Extracting;
using AttributeExtractor.Processing;
using Classification;
using Classification.distance;
using DataSetParser;
using Presentation.Base;
using Statistics;
using Statistics.model;

namespace Presentation.ViewModels
{
    class MainViewModel : BindableBase
    {
        private int _k = 5;

        public int K {
            get => _k;
            set => SetProperty(ref _k, value);
        }

        private bool _isCustomDataSet = true;

        public bool IsCustomDataSet
        {
            get => _isCustomDataSet;
            set
            {
                SetProperty(ref _isCustomDataSet, value);
                UpdateDataSetDirectory();
            }
        }

        private string _keywordsFilePath = "../../../Data/customKeywords(L=100C=5WC=LEMMATIZATION).txt";

        public string KeywordsFilePath
        {
            get { return _keywordsFilePath; }
            set => SetProperty(ref _keywordsFilePath, value);
        }


        public string DataSetDirectory => _isCustomDataSet ? "../../../Data/custom" : "../../../Data/reuters";

        private void UpdateDataSetDirectory() =>
            KeywordsFilePath = _isCustomDataSet
                ? $"../../../Data/customKeywords(L=100C=5WC={WordComparator}).txt"
                : $"../../../Data/{_selectedLabel}Keywords(L=100C=5WC={WordComparator}).txt";

        private string _selectedLabel = "PLACES";

        public string SelectedLabel
        {
            get => _selectedLabel;
            set
            {
                SetProperty(ref _selectedLabel, value);

                UpdateDataSetDirectory();
            }
        }

        private readonly List<string> _labels = new List<string>() {"PLACES", "TOPICS"};

        public List<string> Labels => _labels;

        private int _keywordsCount;

        public int KeywordsCount
        {
            get { return _keywordsCount; }
            set => SetProperty(ref _keywordsCount, value);
        }


        private readonly List<string> _wordComparators = WordComparatorProvider.AVAILABLE_WORD_COMPARATORS;

        public List<string> WordComparators => _wordComparators;

        private string _wordComparator = WordComparatorProvider.AVAILABLE_WORD_COMPARATORS[0];

        public string WordComparator
        {
            get => _wordComparator;
            set => SetProperty(ref _wordComparator, value);
        }

        private readonly List<string> _metrics = MetricProvider.AVAILABLE_METRICS;

        public List<string> Metrics => _metrics;

        private string _metric = MetricProvider.AVAILABLE_METRICS[0];

        public string Metric
        {
            get => _metric;
            set => SetProperty(ref _metric, value);
        }

        private readonly List<string> _featureExtractors = FeatureExtractorProvider.AVAILABLE_FEATURE_EXTRACTORS;

        public List<string> FeatureExtractors => _featureExtractors;

        private string _featureExtractor = FeatureExtractorProvider.AVAILABLE_FEATURE_EXTRACTORS[0];

        public string FeatureExtractor
        {
            get => _featureExtractor;
            set => SetProperty(ref _featureExtractor, value);
        }

        private double _trainingSetFraction = 0.6;

        public double TrainingSetFraction
        {
            get => _trainingSetFraction;
            set  => SetProperty(ref _trainingSetFraction, value);
        }

        private PerformanceMeasures _performanceMeasures;

        public PerformanceMeasures PerformanceMeasures
        {
            get => _performanceMeasures;
            set => SetProperty(ref _performanceMeasures, value);
        }

        private string _currentStep;

        public string CurrentStep
        {
            get => _currentStep;
            set => SetProperty(ref _currentStep, value);
        }

        private DataTable _data;

        public DataTable Data
        {
            get => _data;
            set => SetProperty(ref _data, value);
        }

        private bool _isLoading = false;

        public bool IsLoading
        {
            get => _isLoading;
            set => SetProperty(ref _isLoading, value); 
        }

        private int _coldStartSize = 200;

        public int ColdStartSize
        {
            get => _coldStartSize;
            set => SetProperty(ref _coldStartSize, value);
        }

        public AsyncCommand StartCommand { get; }

        public MainViewModel()
        {

            
            StartCommand = new AsyncCommand((async () =>
            {
                await Task.Run(() =>
                {
                    IsLoading = true;
                    CurrentStep = "Initializing";
                    var (allArticles, labels) = DataSetReader.GetArticles(DataSetDirectory, SelectedLabel);
                    var labelsCollection = Enumerable.Range(0, labels.Count).ToDictionary(i => labels[i], i => i);

                    foreach (var article in allArticles)
                    {
                        article.Tokens = Tokenizer.TokenizeWords(article.Body);
                    }

                    CurrentStep = "Preprocessing";
                    List<ITokenProcessor> preProcessors = new List<ITokenProcessor>()
                    {
                        new StopWordsFilterProcessor(),
                        WordComparatorProvider.GetWordComparator(WordComparator)
                    };

                    foreach (var article in allArticles)
                    {
                        foreach (var tokenProcessor in preProcessors)
                        {
                            article.Tokens = tokenProcessor.Process(article.Tokens);
                        }
                    }

                    Console.WriteLine("Reading keywords...");
                    var keywords = KeywordsAccessObject.ReadKeywords(KeywordsFilePath).Take(KeywordsCount);
                    IFeatureExtractor featureExtractor = FeatureExtractorProvider.GetFeatureExtractorExtractor(FeatureExtractor);

                    CurrentStep = "Extracting features";
                    foreach (var article in allArticles)
                    {
                        article.FeatureVector = featureExtractor.ExtractFeatures(article.Tokens, keywords);
                    }

                    List <PerformanceMeasures> Performances = new List<PerformanceMeasures>();
                    int allTries = 5;
                    for (int i = 0; i < allTries; i++)
                    {

                        allArticles = allArticles.OrderBy(e => Guid.NewGuid()).ToList();

                        var (trainingSet, testSet) = Utility.DivideDataSet(allArticles, TrainingSetFraction);


                        CurrentStep = $"Predicting - {i}/{allTries}";
                        KNNApi.UseKNN(trainingSet.Take(ColdStartSize).ToList(), MetricProvider.GetMetric(Metric), K, testSet);

                        CurrentStep = $"Calculating statistics - {i}/{allTries}";
//                        PerformanceMeasures = PerformanceCalculator.CalculatePerformanceMeasures(testSet, labelsCollection);

                        Performances.Add(PerformanceCalculator.CalculatePerformanceMeasures(testSet, labelsCollection));

                    }

                    var measures = Performances.Aggregate((sum, next) =>
                    {
                        sum.AverageAccuracy += next.AverageAccuracy;
                        sum.Precision += next.Precision;
                        sum.Recall += next.Recall;
                        sum.Specificity += next.Specificity;
                        return sum;
                    });
                    measures.AverageAccuracy /= Performances.Count;
                    measures.Precision /= Performances.Count;
                    measures.Recall/= Performances.Count;
                    measures.Specificity /= Performances.Count;

                    PerformanceMeasures = measures;

                    ShowDataGrid(labelsCollection);
                    CurrentStep = "";
                    IsLoading = false;
                });
            }));
            
        }

        private void ShowDataGrid(Dictionary<string, int> labelsCollection)
        {
            Data = new DataTable();

            Data.Columns.Add("Example actual\\predicted");

            var tmp = labelsCollection.ToDictionary(pair => pair.Value, pair => pair.Key);
            for (int i = 0; i < PerformanceMeasures.ConfusionMatrix[0].Length; i++)
            {
                Data.Columns.Add(tmp[i]);
            }

            int numberOfClasses = PerformanceMeasures.ConfusionMatrix[0].Length;
            for (int i = 0; i < numberOfClasses; i++)
            {
                Object[] row = new object[numberOfClasses + 1];
                row[0] = tmp[i];
                for (int j = 0; j < numberOfClasses; j++)
                {
                    row[j + 1] = PerformanceMeasures.ConfusionMatrix[i][j];
                }

                Data.Rows.Add(row);
            }
        }
    }
}
