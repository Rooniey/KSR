using System.Collections.Generic;
using AttributeExtractor;

namespace DataSetParser.Model
{
    public class TokenizedArticle
    {
        public LabeledArticle LabeledArticle { get; set; }

        public List<Token> Tokens { get; set; }

        public TokenizedArticle(LabeledArticle labeledArticle, List<Token> tokens)
        {
            LabeledArticle = labeledArticle;
            Tokens = tokens;
        }
    }
}
