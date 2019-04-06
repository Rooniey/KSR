using System.Collections.Generic;
using System.Linq;
using LemmaSharp;


namespace AttributeExtractor.Processing
{
    public class Lemmatizer : ITokenProcessor
    {
        private static readonly LemmatizerPrebuiltCompact _lemmatizer = new LemmatizerPrebuiltCompact(LanguagePrebuilt.English);

        public List<string> Process(List<string> tokens)
        {
            return tokens.Select(t => _lemmatizer.Lemmatize(t)).ToList();
        }

    }
}
