using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace GreenBIM.UpdateMaterial.ViewModels
{
    using Autodesk.Revit.DB;
    using Autodesk.Revit.UI;
    using GalaSoft.MvvmLight.CommandWpf;
    using GreenBIM.ServiceClass;
    using GreenBIM.UpdateMaterial.Models;
    using GreenBIM.UpdateMaterial.Services;
    using System.Collections.ObjectModel;
    using System.Windows.Input;

    public class MainContext
    {

        /// <summary>
        /// Инстанс сервиса получения материалов
        /// </summary>
        private GetResourseService getResourseService;
        /// <summary>
        /// Инстанс обновления материалов
        /// </summary>
        private UpdateMaterialService updateMaterialService;
        /// <summary>
        /// Инстанс сервиса Сопирования и удаления материалов
        /// </summary>
        private CopyDelitMaterialService copyDelitMaterialService;
        /// <summary>
        /// Настройки пользователя
        /// </summary>
        public UserSettings userSettings { get; set; }

        /// <summary>
        /// Лист с моделями
        /// </summary>
        public ObservableCollection<DocModel> DocumentList { get; set; }

        /// <summary>
        /// Лист с материалами моделей
        /// </summary>
        public ObservableCollection<MaterialModel> MaterialModelList { get; set; } = new ObservableCollection<MaterialModel>();

        public MainContext(ExternalCommandData commandData)
        {
            getResourseService = new GetResourseService(commandData);
            updateMaterialService = new UpdateMaterialService();
            DocumentList = getResourseService.DocumentList;
            userSettings = new UserSettings();
            copyDelitMaterialService = new CopyDelitMaterialService();

        }

        private void ApplyFirstWin(IClosable win)
        {

            win.Close();
            if(userSettings.CopyMaterial)
            {
                copyDelitMaterialService.CopyMaterial(DocumentList, MaterialModelList);
            }
            else if(userSettings.DelitMaterial)
            {
                copyDelitMaterialService.DelitMaterial(MaterialModelList);
            }
            else if(userSettings.UpdateMaterial)
            {
                getResourseService.GetMatchingFile();
                updateMaterialService.CheckMaterialInFiles(DocumentList, getResourseService.materialsList);
            }
            try
            {
                System.Diagnostics.Process.Start(ConstantClass.LogFilePath);
            }
            catch
            {

            }
            

        }

        /// <summary>
        /// Комманда открытия первого окна
        /// </summary>
        public ICommand FirstWinApplyCommand => new RelayCommand<IClosable>(ApplyFirstWin);

        /// <summary>
        /// Комманда закрытия первого окна
        /// </summary>
        public ICommand CancelCommand => new RelayCommand<IClosable>(win => win.Close());

        /// <summary>
        /// Обновление листа материалов
        /// </summary>
        public void updateMaterialList()
        {
            MaterialModelList.Clear();
            foreach (var el in getResourseService.curentMaterialList)
            {
                if(userSettings.SelectAll)
                {
                    el.IsChoosen = true;
                    MaterialModelList.Add(el);
                }
                else
                {
                    MaterialModelList.Add(el);
                }
                
            }
        }

        /// <summary>
        /// Обновить лист документов
        /// </summary>
        public void updateDocumentlList()
        {
            DocumentList.Clear();
            foreach (var el in getResourseService.UpdateOpenDoc(userSettings))
            {
                DocumentList.Add(el);
            }

        }
    }
}
