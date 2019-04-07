using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Linq;
using System.Security.Policy;
using System.Threading.Tasks;
using AttributeExtractor;
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
        private int _k = 1;

        public int K {
            get => _k;
            set => SetProperty(ref _k, value);
        }

        private string _dataSetDirectory = "../../../Data/reuters";

        public string DataSetDirectory
        {
            get => _dataSetDirectory;
            set => SetProperty(ref _dataSetDirectory, value);
        }


        public bool IsCustomDataSet => _dataSetDirectory == "../../../Data/reuters";

        private string _selectedLabel = "PLACES";

        public string SelectedLabel
        {
            get => _selectedLabel;
            set => SetProperty(ref _selectedLabel, value);
        }

        private readonly List<string> _labels = new List<string>() {"PLACES", "TOPICS"};

        public List<string> Labels => _labels;


        private readonly  List<string> _wordComparators = new List<string>() {"STEMMING", "LEMMATIZATION"};

        public List<string> WordComparators => _wordComparators;

        private string _wordComparator = "STEMMING";

        public string WordComparator
        {
            get => _wordComparator;
            set => SetProperty(ref _wordComparator, value);
        }

        private readonly List<string> _metrics = new List<string>() { "EUCLIDEAN", "MANHATTAN", "CHEBYSHEV" };

        public List<string> Metrics => _metrics;

        private string _metric = "EUCLIDEAN";

        public string Metric
        {
            get => _metric;
            set => SetProperty(ref _metric, value);
        }

        private readonly List<string> _featureExtractors = new List<string>() { "BINARY", "COUNT", "TFIDF" };

        public List<string> FeatureExtractors => _featureExtractors;

        private string _featureExtractor = "BINARY";

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

        private int _mostFrequentTermsToCutCount = 3;

        public int MostFrequentTermsToCutCount
        {
            get => _mostFrequentTermsToCutCount;
            set => SetProperty(ref _mostFrequentTermsToCutCount, value);
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
            get { return _currentStep; }
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


        private AsyncCommand _startCommand;

        public AsyncCommand StartCommand => _startCommand;

        private IMetric GetMetric(string name)
        {
            switch (name)
            {
                case "EUCLIDEAN":
                    return new EuclideanMetric();
                default:
                    throw new InvalidEnumArgumentException("metric");
            }
        }

        private ITokenProcessor GetWordComparator(string name)
        {
            switch (name)
            {
                case "LEMMATIZATION":
                    return new Lemmatizer();
                case "STEMMING":
                    return new Stemmer();
                default:
                    throw new InvalidEnumArgumentException("wordComparator");
            }
        }

        private string[] GetLabels(string name)
        {
            switch (name)
            {
                case "PLACES":
                    return Constants.PLACES;
                case "TOPICS":
                    return Constants.PLACES;
                default:
                    throw new InvalidEnumArgumentException("TODO add different labels"); //TODO add different labels
            }
        }

        public MainViewModel()
        {

            IsLoading = true;
            _startCommand = new AsyncCommand((async () =>
            {
                await Task.Run(() =>
                {
                    CurrentStep = "Initializing";
                    List<string> labels = GetLabels(SelectedLabel).ToList();
                    var labelsCollection = Enumerable.Range(0, labels.Count).ToDictionary(i => labels[i], i => i);

                    KeywordsExtractor keywordsExtractor = new KeywordsExtractor(new TfIdfTokenCalculator(), MostFrequentTermsToCutCount);

                    var allArticles = SgmDataReader.GetArticles(DataSetDirectory, SelectedLabel);
                    foreach (var article in allArticles)
                    {
                        article.Tokens = Tokenizer.TokenizeWords(article.Body);
                    }

                    CurrentStep = "Preprocessing";
                    List<ITokenProcessor> preProcessors = new List<ITokenProcessor>()
                    {
                        new StopWordsFilterProcessor(),
                        GetWordComparator(WordComparator)
                    };

                    foreach (var article in allArticles)
                    {
                        foreach (var tokenProcessor in preProcessors)
                        {
                            article.Tokens = tokenProcessor.Process(article.Tokens);
                        }
                    }

                    var (trainingSet, testSet) = Utility.DivideDataSet(allArticles, TrainingSetFraction);

                    CurrentStep = "Extracting keywords";
                    var keywords = keywordsExtractor.ExtractKeywords(trainingSet, labels);


                    IFeatureExtractor featureExtractor = new KeywordCountFeatureExtractor();

                    CurrentStep = "Extracting features";
                    foreach (var article in testSet)
                    {
                        article.FeatureVector = featureExtractor.ExtractFeatures(article.Tokens, allArticles.Select(a => a.Tokens).ToList(), keywords);
                    }

                    foreach (var article in trainingSet)
                    {
                        article.FeatureVector = featureExtractor.ExtractFeatures(article.Tokens, allArticles.Select(a => a.Tokens).ToList(), keywords);
                    }


                    CurrentStep = "Predicting";
                    KNNApi.UseKNN(trainingSet, GetMetric(Metric), K, testSet);

                    CurrentStep = "Calculating statistics";
                    PerformanceMeasures = PerformanceCalculator.CalculatePerformanceMeasures(testSet, labelsCollection);

                    Data = new DataTable();

                    Data.Columns.Add("actual\\predicted");

                    var tmp = labelsCollection.ToDictionary(pair => pair.Value, pair => pair.Key);
                    for (int i = 0; i < PerformanceMeasures.ConfusionMatrix[0].Length; i++)
                    {
                        Data.Columns.Add(tmp[i]);
                    }

                    int numberOfClasses = PerformanceMeasures.ConfusionMatrix[0].Length;
                    for (int i = 0; i < numberOfClasses; i++)
                    {
                        Object[] row = new object[numberOfClasses+1];
                        row[0] = tmp[i];
                        for (int j = 0; j < numberOfClasses; j++)
                        {
                            row[j + 1] = PerformanceMeasures.ConfusionMatrix[i][j];
                        }
                        Data.Rows.Add(row);
                    }
                    CurrentStep = "";
                    IsLoading = false;
                });
            }));
            
        }

    }
}
