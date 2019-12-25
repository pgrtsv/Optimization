using System;
using System.Collections.Generic;
using System.Text;
using DynamicData;
using Optimization.Core;

namespace Optimization.UI.ViewModels
{
    public class MainWindowViewModel : ViewModelBase
    {
        private SourceList<ICityPlace> _cityPlaces;

        public MainWindowViewModel()
        {
        }
    }
}
