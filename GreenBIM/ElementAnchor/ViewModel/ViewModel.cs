using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Autodesk.Revit.DB;
using Autodesk.Revit.UI;
using System.Windows.Input;
using GreenBIM.ElementAnchor.ViewModel;
using System.Windows;
using GalaSoft.MvvmLight.CommandWpf;
using GalaSoft.MvvmLight;
using GreenBIM.ElementAnchor;


namespace GreenBIM.ElementAnchor.ViewModel
{

    using View;
    using Service;
    using GreenBIM.ServiceClass;

    class ViewModel: ViewModelBase
    {
        private bool _var1 = false;
        private bool _var2 = false;
       

        public bool _Var1
        {
            get => _var1;
            set
            {
                _var1 = value;
                RaisePropertyChanged();
            }
        }

        public bool _Var2
        {
            get => _var2;
            set
            {
                _var2 = value;
                RaisePropertyChanged();
            }
        }

        private MainWindow UC;
        private Service _service;

        public ViewModel()
        {

        }
        public ViewModel(Document doc)
        {
            
            _service = new Service(doc);
        }
        
        
        private void Apply(IClosable win)
        {
            if(_Var1)
            {
                _service.AnchorElementInModel();
            }
            else if(_Var2)
            {
                _service.AnchorElementInSheet();
            }

            win.Close();
            TaskDialog.Show("Внимание", "Работа плагина завершена");
        }
        private void Cancel(IClosable win)
        {
            win.Close();
        }

        public ICommand ApplyCommand => new RelayCommand<IClosable>(Apply);
        public ICommand CancelCommand => new RelayCommand<IClosable>(win=>win.Close());
        

    }

}
