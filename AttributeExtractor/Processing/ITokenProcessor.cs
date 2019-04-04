using System.Collections.Generic;

namespace AttributeExtractor.Processing
{
    public interface ITokenProcessor
    {
        List<Token> Process(List<Token> tokens);
    }
}
