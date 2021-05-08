using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using GalaSoft.MvvmLight;

namespace GreenBIM.FinishSheet.ViewModel
{
    public class UserSettings:ViewModelBase
    {
        private bool _fillElementParameters = true;

        /// <summary>
        /// Значение кнопки заполнять значение элементов
        /// </summary>
        public bool FillElementParameters
        {
            get => _fillElementParameters;
            set
            {
                _fillElementParameters = value;
                RaisePropertyChanged();

            }
        }
    }
}
