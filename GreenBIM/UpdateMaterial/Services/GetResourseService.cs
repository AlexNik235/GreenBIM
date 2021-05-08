using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace GreenBIM.UpdateMaterial.Services
{
    using Autodesk.Revit.UI;
    using Autodesk.Revit.DB;
    using System.Collections.ObjectModel;
    using GreenBIM.UpdateMaterial.ViewModels;
    using GreenBIM.UpdateMaterial.Models;
    using System.Windows.Forms;
    using Newtonsoft.Json;
    using System.IO;

    public class GetResourseService
    {
        private DocumentSet _docSet;
        private ObservableCollection<DocModel> _docModelList = new ObservableCollection<DocModel>();

        /// <summary>
        /// Поле со списком всех октрытых документов
        /// </summary>
        public ObservableCollection<DocModel> DocumentList { get; set; } = new ObservableCollection<DocModel>();
        public Dictionary<string, List<string>> materialsList;

        /// <summary>
        /// Лист с моделями материалов выбранного документа
        /// </summary>
        public ObservableCollection<MaterialModel> curentMaterialList
        {
            get
            {
                var return_list = new ObservableCollection<MaterialModel>();
                try
                {
                    var cur_doc = DocumentList.Where(i => i.IsMaterialFile).ToList().First();
                    var materialList = new FilteredElementCollector(cur_doc.doc).OfClass(typeof(Material)).Cast<Material>().ToList();
                    foreach (var material in materialList)
                    {
                        return_list.Add(new MaterialModel()
                        {
                            material = material,
                            doc = cur_doc.doc
                        });
                    }
                }
                catch
                {
                    return_list.Add(new MaterialModel()
                    {
                        material = null
                    });
                }
                
                return return_list;
            }
            set { }
        }
        
        public GetResourseService(ExternalCommandData commandData)
        {
            _docSet = commandData.Application.Application.Documents;
            Initialize();
        }

        /// <summary>
        /// Инициализируем файл
        /// </summary>
        private void Initialize()
        {
            _docModelList = CreateDocModelList();
            foreach(var el in _docModelList)
            {
                if(!el.IsFamilyModel)
                {
                    DocumentList.Add(el);
                }
            }
        }

        /// <summary>
        /// Получаем коллекцию открытых документов
        /// </summary>
        /// <returns></returns>
        private ObservableCollection<DocModel> CreateDocModelList()
        {
           var _docList = new ObservableCollection<DocModel>();
            foreach (Document doc in _docSet)
            {
                _docList.Add(new DocModel()
                {
                    doc = doc,
                    DocName = doc.Title,
                    ChangeMaterial = false,
                    IsMaterialFile = false
                });
            }
            return _docList;
        }

        /// <summary>
        /// Обновить список моделей
        /// </summary>
        /// <param name="userSettrins"></param>
        /// <returns></returns>
        public ObservableCollection<DocModel> UpdateOpenDoc(UserSettings userSettrins)
        {
            var _docList = new ObservableCollection<DocModel>();
            if (userSettrins.IsFamilyDocument)
            {
                foreach(var el in _docModelList)
                {
                    if(el.IsFamilyModel)
                    {
                        _docList.Add(el);
                    }
                }
            }
            else
            {
                foreach (var el in _docModelList)
                {
                    if (!el.IsFamilyModel)
                    {
                        _docList.Add(el);
                    }
                }
            }
            return _docList;
        }

        /// <summary>
        /// Получить путь к файлу
        /// </summary>
        /// <returns></returns>
        public void GetMatchingFile()
        {
            OpenFileDialog openFileDialog = new OpenFileDialog();
            openFileDialog.ShowDialog();
            string fileName = openFileDialog.FileName;
            try
            {
                materialsList = JsonConvert.DeserializeObject<Dictionary<string, List<string>>>(File.ReadAllText(fileName, System.Text.Encoding.Default));
            }
            catch
            {
                TaskDialog.Show("Ошибка", "либо файл не найден, либо он не соотв. требованиям");
            }

        }
    }
}
