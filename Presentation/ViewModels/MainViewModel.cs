using Presentation.Base;

namespace Presentation.ViewModels
{
    class MainViewModel : BindableBase
    {
        private int _k = 1;

        public int K {
            get => _k;
            set => SetProperty(ref _k, value);
        }

        public MainViewModel()
        {

        }

    }
}
