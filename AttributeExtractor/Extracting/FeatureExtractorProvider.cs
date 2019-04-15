using System;
using System.Collections.Generic;

namespace AttributeExtractor.Extracting
{
    public static class FeatureExtractorProvider
    {
        public static readonly List<string> AVAILABLE_FEATURE_EXTRACTORS = new List<string>() { "BINARY", "COUNT", "FREQUENCY", "DISTANCE", "MULTIDISTANCE", "FREQUENCY-DISTANCE" };

        public static IFeatureExtractor GetFeatureExtractorExtractor(string name)
        {
            if (name == null || string.IsNullOrEmpty(name))
                throw new ArgumentNullException(nameof(name));

            string parsedName = name.ToUpperInvariant();

            switch (parsedName)
            {
                case "BINARY":
                    return new KeywordBinaryFeatureExtractor();
                case "COUNT":
                    return new KeywordCountFeatureExtractor();
                case "FREQUENCY":
                    return new KeywordFrequencyExtractor();
                case "DISTANCE":
                    return new KeywordDistanceFromStartFeatureExtractor();
                case "MULTIDISTANCE":
                    return new KeywordMultiDistanceFromStartFeatureExtractor();
                case "FREQUENCY-DISTANCE":
                    return new KeywordFrequencyAndDistanceFeatureExtractor();

                default:
                    throw new ArgumentException("Unknown feature extractor");
            }
        }

    }
}
