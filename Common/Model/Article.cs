﻿using System.Collections.Generic;

namespace Common.Model
{
    public class Article
    {
        public string Body { get; set; }

        public List<string> Tokens { get; set; }

        public string Label { get; set; }

        public string Prediction { get; set; }

        public Dictionary<string, double> FeatureVector { get; set; } = new Dictionary<string, double>();

        public Article(string body, string label)
        {
            Body = body;
            Label = label;
        }

        public Article(PureArticle ar)
        {
            Body = ar.Body;
            Label = ar.Label;
        }
    }
}
