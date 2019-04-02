using System.Collections.Generic;

namespace AttributeExtractor.Processing
{
    class StopWordsFilterProcessor : ITokenProcessor
    {
        List<string> StopWords { get; }

        public StopWordsFilterProcessor(StopWordsFilterProcessor filter)
        {

        }




        public Token[] Process(Token[] tokens)
        {
            throw new System.NotImplementedException();
        }
    }
}
