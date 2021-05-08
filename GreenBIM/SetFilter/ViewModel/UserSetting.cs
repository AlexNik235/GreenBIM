using GalaSoft.MvvmLight;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace GreenBIM.SetFilter.ViewModel
{
    public class UserSetting:ViewModelBase
    {
        private bool _setTemporaryView = false;
        public bool SetTemporaryView
        {
            get => _setTemporaryView;
            set
            {
                _setTemporaryView = value;
                RaisePropertyChanged();
            }
        }
        public bool _delitElement = false;

        /// <summary>
        /// Удаление элементов
        /// </summary>
        public bool DelitElement
        {
            get => _delitElement;
            set
            {
                _delitElement = value;
                RaisePropertyChanged();
            }
        }
    }
}
