using System.Collections.Generic;

namespace AttributeExtractor.Extracting
{
    public interface IFeatureExtractor
    {
        Dictionary<string, double> ExtractFeatures(List<string> article, List<List<string>> allArticles, List<string> keywords);
    }
}
