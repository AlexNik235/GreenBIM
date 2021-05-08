using GalaSoft.MvvmLight;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Autodesk.Revit.DB;
using Autodesk.Revit.UI;
using GreenBIM.SetFilter.Service;
using System.ComponentModel;
using System.Windows.Input;
using GalaSoft.MvvmLight.CommandWpf;
using GreenBIM.ServiceClass;
using GreenBIM.SetFilter.Views;
using System.IO;
using System.Windows.Forms;
using System.Collections.ObjectModel;

namespace GreenBIM.SetFilter.ViewModel
{
    
    class MainContext: ViewModelBase
    {
        public ObservableCollection<FilterElementModel> AllFiltersList { get; set; }
        public FilterService _filterService;
        public ObservableCollection<FilterElementModel> CurFinlterList { get; set; }
        public UserSetting _userSetting { get; set; } = new UserSetting();// Пользовательские параметры
        public bool updateWind { get; set; }
        public MainContext()
        {

        }
        public MainContext(UIApplication app)
        {
            _filterService = new FilterService(app);
            _filterService.Initialize();
            CurFinlterList = _filterService.CurrentFilterList;
        }

        private void ApplyAddFilterWin(IClosable win)
        {
            win.Close();
            CurFinlterList = _filterService.CurrentFilterList;
        }
        private void ApplyMainWin(IClosable win)
        {
            _filterService.userSetting = _userSetting;
            _filterService.Raise();            
        }
        private void ShowAllFilters(IClosable win)
        {
            //win.Close();
            AllFiltersList = _filterService.GetAllFilterList();
            AddFilterWindow mainWindow = new AddFilterWindow()
            {
                DataContext = this
            };
            mainWindow.Show();
            
        }

        /// <summary>
        /// Комманда открытия первого окна
        /// </summary>
        public ICommand AddFilterWinApplyCommand => new RelayCommand<IClosable>(ApplyAddFilterWin);
        /// <summary>
        /// Комманда закрытия первого окна
        /// </summary>
        public ICommand CancelCommand => new RelayCommand<IClosable>(win => win.Close());

        /// <summary>
        /// Комманда открытия второго окна
        /// </summary>
        public ICommand MainWindowApplyCommand => new RelayCommand<IClosable>(ApplyMainWin);
        /// <summary>
        /// Показать главное окно
        /// </summary>
        public ICommand ShowFirstWinCommand => new RelayCommand<IClosable>(ShowAllFilters);

        

    }
}
