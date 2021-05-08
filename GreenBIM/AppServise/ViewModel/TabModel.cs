using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using GalaSoft.MvvmLight;

namespace GreenBIM.AppServise.ViewModel
{
    class TabModel:ViewModelBase
    {
        private string _name;
        private bool _isChek;
        public List<string> panelsList { get; set; } = new List<string>() { "1", "2", "3" };

        public string TabName
        {
            get { return _name; }
            set { _name = value;
                RaisePropertyChanged();
            }
        }
        public bool IsCheck
        {
            get { return _isChek; }
            set { _isChek = value;
                RaisePropertyChanged();
            }
        }
    }
}
