using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Autodesk.Revit.UI;
using Autodesk.Revit.DB;
using GreenBIM.AppServise;
using System.ComponentModel;
using GalaSoft.MvvmLight;
using GreenBIM.ServiceClass;
using System.Windows.Input;
using GalaSoft.MvvmLight.CommandWpf;
using GreenBIM.AppServise.Service;
using Newtonsoft.Json;
using System.IO;
using System.Windows.Forms;

namespace GreenBIM.AppServise.ViewModel
{
    class MainContext: ViewModelBase
    {
        public BindingList<TabModel> tabModelList { get; set; } = new BindingList<TabModel>();
        private UIControlledApplication _app;
        private ButtonCreater btnCreator;
        private CheckAndCreateConfig _checkAndCreate;
        public bool IsDirectoryExist; //Свойство наличия нужной директории
        public bool IsFileConfigExist;
        private string ParentDirPath = System.Reflection.Assembly.GetExecutingAssembly()
            .Location.Remove(System.Reflection.Assembly.GetExecutingAssembly()
            .Location.LastIndexOf(@"\", StringComparison.Ordinal));



        public MainContext() { }
        public MainContext(UIControlledApplication app)
        {
            AppSetting appSetting = JsonConvert.DeserializeObject<AppSetting>(File.ReadAllText(Path.Combine(ParentDirPath, @"AppDirrectory\AppConfig.json"), System.Text.Encoding.Default));
            _app = app;
            _checkAndCreate = new CheckAndCreateConfig(appSetting);
            IsDirectoryExist = _checkAndCreate.SearchCreatableDirectory();
            btnCreator = new ButtonCreater(_app, _checkAndCreate.FullPathToDirectory);
            IsFileConfigExist = _checkAndCreate.IsConfigFileExist();
            FillTabModelList();
        }
        /// <summary>
        /// Заполняем список элементов биндингЛист
        /// </summary>
        private void FillTabModelList()
        {
            foreach(var el in btnCreator.tabsName)
            {
                tabModelList.Add(new TabModel() { TabName = el, IsCheck = false });
            }
        }
        /// <summary>
        /// Создаем необходимые кнопки
        /// </summary>
        public void CreateButtons()
        {
            btnCreator.CreateButtons(_checkAndCreate.FullPathToFileConfig);
        }

        private void Apply(IClosable win)
        {
            _checkAndCreate.CreateConfigAndFillData(tabModelList);
            win.Close();
            TaskDialog.Show("Внимание", "Для появления вкладок необходимо перезапустить ревит");
        }



        public ICommand ApplyCommand => new RelayCommand<IClosable>(Apply);
        public ICommand CancelCommand => new RelayCommand<IClosable>(win => win.Close());


    }
}
