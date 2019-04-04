namespace DataSetParser.Model
{
    public class LabeledArticle
    {
        public Article Article { get; set; }

        public string Label { get; set; }

        public LabeledArticle(Article article, string label)
        {
            Article = article;
            Label = label;
        }
    }
}
