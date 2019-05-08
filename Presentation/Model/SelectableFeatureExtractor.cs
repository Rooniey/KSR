using System;
using AttributeExtractor.Extracting;
using Presentation.Base;

namespace Presentation.Model
{
    public class SelectableFeatureExtractor : BindableBase
    {
        private IFeatureExtractor _featureExtractor;

        public IFeatureExtractor FeatureExtractor
        {
            get => _featureExtractor;
            set => SetProperty(ref _featureExtractor, value);
        }

        private bool _isSelected = true;

        public bool IsSelected
        {
            get => _isSelected;
            set => SetProperty(ref _isSelected, value);
        }


        public string Name => _featureExtractor.GetType().Name;

        public SelectableFeatureExtractor(IFeatureExtractor featureExtractor)
        {
            _featureExtractor = featureExtractor;
        }
    }
}
