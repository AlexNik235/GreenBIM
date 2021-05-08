namespace GreenBIM.SketchRebar.ViewModels
{
    using Autodesk.Revit.UI;
    using GalaSoft.MvvmLight.Command;
    using GreenBIM.ServiceClass;
    using GreenBIM.SketchRebar.Models;
    using GreenBIM.SketchRebar.Services;
    using System;
    using System.Collections.Generic;
    using System.Collections.ObjectModel;
    using System.Linq;
    using System.Text;
    using System.Threading.Tasks;
    using System.Windows.Input;

    class MainContext
    {
        private readonly UIDocument _uidoc;
        private RebarSketchService rebarSketchService;
        public bool IsOneColumn;
        public List<UserSettings> UserSettingsList { get; set; } = new List<UserSettings>();
        public MainContext(UIDocument uidoc)
        {
            _uidoc = uidoc;
            rebarSketchService = new RebarSketchService(uidoc);
            IsOneColumn = rebarSketchService.IsOneColumn();
            if (!IsOneColumn)
            {
                for (int i = 1; i < rebarSketchService.CountColumns() + 1; i++)
                {
                    UserSettingsList.Add(new UserSettings() { NumberOfColumn = i });
                }
            }
            
        }

        /// <summary>
        /// Стартовая комманда тест
        /// </summary>
        public void StartCommand()
        {
            rebarSketchService.Start(UserSettingsList);
        }
        private void ApplyFirstWin(IClosable win)
        {
            rebarSketchService.Start(UserSettingsList);
            win.Close();
        }
        /// <summary>
        /// Комманда открытия первого окна
        /// </summary>
        public ICommand FirstWinApplyCommand => new RelayCommand<IClosable>(ApplyFirstWin);
        /// <summary>
        /// Комманда закрытия первого окна
        /// </summary>
        public ICommand CancelCommand => new RelayCommand<IClosable>(win => win.Close());
    }
}
