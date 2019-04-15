using System.Collections.Generic;

namespace AttributeExtractor.Extracting
{
    public interface IFeatureExtractor
    {
        Dictionary<string, double> ExtractFeatures(List<string> article, IEnumerable<string> keywords);
    }
}
