using System.Collections.Generic;
using Common.Model;

namespace AttributeExtractor.Extracting
{
    public interface IFeatureExtractor
    {
        Dictionary<string, double> ExtractFeatures(Article article, IEnumerable<string> keywords);
    }
}