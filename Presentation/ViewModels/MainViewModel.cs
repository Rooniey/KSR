using System;
using System.Collections.Generic;
using System.Data;
using System.IO;
using System.Linq;
using System.Runtime.Serialization;
using System.Runtime.Serialization.Formatters.Binary;
using System.Threading.Tasks;
using System.Windows.Input;
using AttributeExtractor.Extracting;
using AttributeExtractor.Processing;
using Classification;
using Classification.distance;
using Common.Model;
using DataSetParser;
using Microsoft.Win32;
using Presentation.Base;
using Presentation.Model;
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
            get => _keywordsFilePath;
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

        private int _keywordsCount = 40;

        public int KeywordsCount
        {
            get => _keywordsCount;
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

        public List<SelectableFeatureExtractor> FeatureExtractors { get; } = new List<SelectableFeatureExtractor>()
        {
            new SelectableFeatureExtractor(new AverageWordLengthFeatureExtractor()),
            new SelectableFeatureExtractor(new BeginningKeywordCountFeatureExtractor()),
            new SelectableFeatureExtractor(new FirstKeywordDistanceFromStartFeatureExtractor()),
            new SelectableFeatureExtractor(new KeywordCountFeatureExtractor()),
            new SelectableFeatureExtractor(new KeywordFrequencyExtractor()),
            new SelectableFeatureExtractor(new LastKeywordDistanceFromEndFeatureExtractor()),
            new SelectableFeatureExtractor(new WordCountFeatureExtractor()),
            new SelectableFeatureExtractor(new AverageSentenceLengthFeatureExtractor()),
            new SelectableFeatureExtractor(new ProperNameCountFeatureExtractor()),
            new SelectableFeatureExtractor(new UniqueWordsRatioFeatureExtractor())
        };


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

        private List<Article> allArticles;
        private List<Article> trainingSet;
        private List<Article> testSet;
        private List<Article> coldStart;
        private List<string> labelCollection;


        public AsyncCommand StartCommand { get; }
        public AsyncCommand GeneratePreprocessedDataSet { get; }
        public AsyncCommand SaveDataSet { get; }
        public AsyncCommand ReadDataSet { get; }

        public MainViewModel()
        {
            GeneratePreprocessedDataSet = new AsyncCommand(async () => {
                await Task.Run(() => 
                {
                    var (all, labels) = DataSetReader.GetArticles(DataSetDirectory, SelectedLabel);
                    labelCollection = labels;
                    allArticles = all.OrderBy(e => Guid.NewGuid()).ToList();
                    var (training, test) = Utility.DivideDataSet(allArticles, TrainingSetFraction);
                    trainingSet = training;
                    testSet = test;
                    coldStart = trainingSet.Take(ColdStartSize).ToList();
                }
            );});

            SaveDataSet = new AsyncCommand(async () =>
            {
                await Task.Run(() =>
                {
                    FileStream fs = new FileStream("../../../Data/serialized/file.dat", FileMode.Create);

                    BinaryFormatter formatter = new BinaryFormatter();
                    try
                    {
                        var cold = coldStart.Select(ar => new PureArticle(ar)).ToList();
                        var training = trainingSet.Select(ar => new PureArticle(ar)).ToList();
                        var test = testSet.Select(ar => new PureArticle(ar)).ToList();
                        formatter.Serialize(fs, new PreparedDataSet(training, test, cold, labelCollection));
                    }
                    catch (SerializationException e)
                    {
                        Console.WriteLine("Failed to serialize. Reason: " + e.Message);
                        throw;
                    }
                    finally
                    {
                        fs.Close();
                    }
                });
            });

            ReadDataSet = new AsyncCommand(async () =>
            {
                await Task.Run(() =>
                {
                    OpenFileDialog openFileDialog1 = new OpenFileDialog
                    {
                        Title = "Choose to serialize",
                        DefaultExt = "dat",
                        Filter = "dat files (*.dat)|*.dat",
                        FilterIndex = 2,
                    };

                    if (openFileDialog1.ShowDialog() == true)
                    {
                        
                        FileStream fs = new FileStream(openFileDialog1.FileName, FileMode.Open);
                        try
                        {
                            BinaryFormatter formatter = new BinaryFormatter();
                            var preparedDataSet = (PreparedDataSet)formatter.Deserialize(fs);
                            coldStart = preparedDataSet.ColdStart.Select(ar => new Article(ar)).ToList();
                            trainingSet = preparedDataSet.TrainingSet.Select(ar => new Article(ar))
                                .Where(t => !coldStart.Exists(ar => ar.Body == t.Body)).Concat(coldStart).ToList();
                            testSet = preparedDataSet.TestSet.Select(ar => new Article(ar)).ToList();
                            labelCollection = preparedDataSet.LabelCollection;
                            allArticles = new List<Article>().Concat(trainingSet).Concat(testSet).ToList();
                        }
                        catch (SerializationException e)
                        {
                            Console.WriteLine("Failed to deserialize. Reason: " + e.Message);
                            throw;
                        }
                        finally
                        {
                            fs.Close();
                        }
                    }
                });
            });

            StartCommand = new AsyncCommand((async () =>
            {
                await Task.Run(() =>
                {
                    IsLoading = true;
                    CurrentStep = "Initializing";

                    foreach (var article in allArticles)
                    {
                        article.FeatureVector = new Dictionary<string, double>();
                    }

                    var labelList = Enumerable.Range(0, labelCollection.Count).ToDictionary(i => labelCollection[i], i => i);

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

                    CurrentStep = "Extracting features";
                    var extractors = FeatureExtractors.Where(fe => fe.IsSelected).ToList();
                    foreach (var article in allArticles)
                    {
                        foreach (var selectableFeature in extractors)
                        {
                            var features = selectableFeature.FeatureExtractor.ExtractFeatures(article, keywords);
                            foreach (KeyValuePair<string, double> feature in features)
                            {
                               article.FeatureVector.Add(feature.Key, feature.Value); 
                            }
                        }
                    }

                    CurrentStep = $"Predicting";
                    KNNApi.UseKNN(coldStart, MetricProvider.GetMetric(Metric), K, testSet);
                    CurrentStep = $"Calculating statistics";
                   
                    PerformanceMeasures = PerformanceCalculator.CalculatePerformanceMeasures(testSet, labelList); 

                    ShowDataGrid(labelList);

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
