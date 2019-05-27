using System;
using System.Collections.Generic;
using Common.Model;

namespace Presentation.Model
{

    [Serializable]
    public class PreparedDataSet
    {
        public List<PureArticle> TrainingSet { get; set; }
        public List<PureArticle> TestSet { get; set; }
        public List<PureArticle> ColdStart { get; set; }
        public List<String> LabelCollection { get; set; }

        public PreparedDataSet(List<PureArticle> trainingSet, List<PureArticle> testSet, List<PureArticle> coldStart, List<string> labelCollection)
        {
            TrainingSet = trainingSet;
            TestSet = testSet;
            ColdStart = coldStart;
            LabelCollection = labelCollection;
        }

        public PreparedDataSet()
        {
        }
    }
}
