using System.Collections.Generic;
using System.Linq;
using Iveonik.Stemmers;

namespace AttributeExtractor.Processing
{
    public class Stemmer : ITokenProcessor
    {
        private static readonly EnglishStemmer _stemmer = new EnglishStemmer();

        public List<string> Process(List<string> tokens)
        {
            return tokens.Select(t => _stemmer.Stem(t)).ToList();
        }

    }
}
