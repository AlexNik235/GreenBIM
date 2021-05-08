using GalaSoft.MvvmLight;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Autodesk.Revit.DB;

namespace GreenBIM.SetFilter.ViewModel
{
    class FilterElementModel:ViewModelBase
    {
        private string _filterName;
        private Element _filter;
        private bool _isOn;
        private ElementId _elementId;
        private bool _delitOnView;

        /// <summary>
        /// Имя фильтра
        /// </summary>
        public string FilterName
        {
            get { return _filterName; }
            set 
            { 
                _filterName = value;
                RaisePropertyChanged();
            }
            
        }

        /// <summary>
        /// Включен ли фильтр на текущем виде
        /// </summary>
        public bool IsOn
        {
            get { return _isOn; }
            set
            {
                _isOn = value;
                RaisePropertyChanged();
            }
        }

        /// <summary>
        /// Айдишник элемента фильтра
        /// </summary>
        public ElementId elementId
        {
            get { return _elementId; }
            set 
            { _elementId = value;
                RaisePropertyChanged();
            }
        }
        /// <summary>
        /// Получение самого элемента
        /// </summary>
        public Element FilterElement
        {
            get => _filter;
            set
            {
                _filter = value;
                RaisePropertyChanged();
            }
        }
        public bool DelitOnView
        {
            get => _delitOnView;
            set
            {
                _delitOnView = value;
                RaisePropertyChanged();
            }
        }

    }
}
