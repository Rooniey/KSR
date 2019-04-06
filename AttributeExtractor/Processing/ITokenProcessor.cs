using System.Collections.Generic;

namespace AttributeExtractor.Processing
{
    public interface ITokenProcessor
    {
        List<string> Process(List<string> tokens);
    }
}
