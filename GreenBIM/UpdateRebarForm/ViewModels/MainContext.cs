using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace GreenBIM.UpdateRebarForm.ViewModels
{
    using Autodesk.Revit.DB;
    using Autodesk.Revit.UI;
    using GalaSoft.MvvmLight.CommandWpf;
    using GreenBIM.ServiceClass;
    using GreenBIM.UpdateRebarForm.Models;
    using GreenBIM.UpdateRebarForm.Service;
    using System.Windows.Input;

    public class MainContext
    {
        private readonly UIApplication _uiApp;
        public UserSettings userSettings { get; set; }
        private UpdateRebarFormService _updateRebarFormService;
        public MainContext(UIApplication uiApp)
        {
            _uiApp = uiApp;
            userSettings = new UserSettings();
            _updateRebarFormService = new UpdateRebarFormService(_uiApp);
        }



        private void Apply(IClosable win)
        {
            _updateRebarFormService.ServiceCommand(userSettings);
            win.Close();

        }

        /// <summary>
        /// Комманда открытия первого окна
        /// </summary>
        public ICommand ApplyCommand => new RelayCommand<IClosable>(Apply);

        /// <summary>
        /// Комманда закрытия первого окна
        /// </summary>
        public ICommand CancelCommand => new RelayCommand<IClosable>(win => win.Close());
    }
}
