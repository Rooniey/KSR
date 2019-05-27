using System;

namespace Common.Model
{
    [Serializable]
    public class PureArticle
    {
        public string Body { get; set; }

        public string Label { get; set; }


        public PureArticle(Article ar)
        {
            Body = ar.Body;
            Label = ar.Label;
        }
    }
}
