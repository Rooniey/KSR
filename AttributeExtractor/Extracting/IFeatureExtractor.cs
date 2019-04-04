using System.Collections.Generic;

namespace AttributeExtractor.Extracting
{
    interface IFeatureExtractor
    {
        Dictionary<string, double> ExtractFeature(Token[] token);
    }
}
