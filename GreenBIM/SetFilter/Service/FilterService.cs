using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.ComponentModel;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using Autodesk.Revit.DB;
using Autodesk.Revit.UI;
using GalaSoft.MvvmLight;
using GreenBIM.SetFilter.ViewModel;

namespace GreenBIM.SetFilter.Service
{
    class FilterService:ViewModelBase, IExternalEventHandler
    {
        private ExternalEvent externalEvent;
        private UIApplication _app;
        private Document _doc;
        private View _view;
        private ObservableCollection<FilterElementModel> _allFilterList = new ObservableCollection<FilterElementModel>();
        private ObservableCollection<FilterElementModel> _currentFilterList = new ObservableCollection<FilterElementModel>();



        /// <summary>
        /// Только для того, что бы передать значение в поле после выполнения обновления
        /// </summary>
        public ObservableCollection<FilterElementModel> CurrentFilterList
        {
            get
            {
                return UpdateCurenFilters();
            }
            set
            {

            }
        }

        public UserSetting userSetting;
        public FilterService(UIApplication app)
        {
            _app = app;
            _doc = app.ActiveUIDocument.Document;
            _view = _doc.ActiveView;
        }
        /// <summary>
        /// Обновить текущий вид
        /// </summary>
        public void updateDoc()
        {
            var new_view = _app.ActiveUIDocument.Document.ActiveView;
            if(new_view != _view)
            {
                _view = new_view;
                CurrentFilterList = UpdateCurenFilters();
            }
            

        }

        /// <summary>
        /// Получаем список всех фильров-моделей
        /// </summary>
        /// <returns></returns>
        public ObservableCollection<FilterElementModel> GetAllFilterList()
        {
            ///Отчищаем список
            _allFilterList.Clear();
            //Поулчаем все фильтры в модели
            List<ParameterFilterElement> filterlist = new FilteredElementCollector(_doc)
                .OfClass(typeof(ParameterFilterElement))
                .Cast<ParameterFilterElement>()
                .ToList();

            //Получаем все текущие модели
            foreach(var el in _currentFilterList)
            {
                _allFilterList.Add(el);
            }

            ///Добавляем оставшиеся фильтры
            foreach(var el in filterlist)
            {
                if(!IsExistInListModel(_allFilterList, (Element)el))
                {
                    var new_model = new FilterElementModel()
                    {
                        FilterName = el.Name,
                        IsOn = false,
                        //IsOn = _view.GetFilterVisibility(el.Id),
                        elementId = el.Id,
                        FilterElement = el
                    };
                    _allFilterList.Add(new_model);
                }
                
            }

            return _allFilterList;
        }
        /// <summary>
        /// Содержит ли список моделей текущий фильтр
        /// </summary>
        /// <param name="listModel"></param>
        /// <param name="filter"></param>
        /// <returns></returns>
        private bool IsExistInListModel(ObservableCollection<FilterElementModel> listModel, Element filter)
        {
            foreach(var model in listModel)
            {
                if(model.FilterElement.Id == filter.Id)
                {
                    return true;
                }
            }
            return false;
        }
        /// <summary>
        /// Обновляем список моделей фильтров
        /// </summary>
        /// <returns></returns>
        public ObservableCollection<FilterElementModel> UpdateCurenFilters()
        {
            //Отчищаем лист
            _currentFilterList.Clear();

            //Получаем модели текущих фильтров
            if (_view.GetFilters() != null)
            {
                foreach (var filterId in _view.GetFilters())
                {
                    var new_filter = new FilterElementModel()
                    {
                        FilterName = _doc.GetElement(filterId).Name,
                        IsOn = _view.GetFilterVisibility(filterId),
                        FilterElement = _doc.GetElement(filterId),
                        elementId = filterId
                    };
                    _currentFilterList.Add(new_filter);

                }
            }
            //Получаем модели из всех фильтров
            foreach (var el in _allFilterList)
            {
                if (el.IsOn && !IsExistInListModel(_currentFilterList, el.FilterElement))
                {
                    _currentFilterList.Add(el);
                }
            }
            _allFilterList.Clear();
            return _currentFilterList;
        }


        /// <summary>
        /// Установка шаблона временного вида
        /// </summary>
        private void SetTemporaryTemplate()
        {
            if(_view.IsInTemporaryViewMode(TemporaryViewMode.TemporaryViewProperties))
            {

            }
            else
            {
                if (_view.CanEnableTemporaryViewPropertiesMode())
                {
                    _view.EnableTemporaryViewPropertiesMode(_view.Id);
                    //foreach (var el in _view.GetFilters())
                    //{
                    //    _view.RemoveFilter(el);
                    //}
                }
                else
                {
                    TaskDialog.Show("Ошибка", "На данном виде невозможно применить шаблон временного вида");
                    return;
                }
            }
  
        }

        

        /// <summary>
        /// Установка фильтров
        /// </summary>
        public void SetFilters(UIApplication app)
        {

            using (Transaction t = new Transaction(app.ActiveUIDocument.Document, "Установка фильтров"))
            {
                t.Start();
                if (userSetting.SetTemporaryView)
                {
                    SetTemporaryTemplate();
                }
                else
                {
                    _view.EnableTemporaryViewPropertiesMode(ElementId.InvalidElementId);
                }
                foreach (var el in _currentFilterList)
                {
                    if(userSetting._delitElement)
                    {

                        if (el.IsOn)
                        {
                            app.ActiveUIDocument.Document.ActiveView.RemoveFilter(el.elementId);
                        }
                                                
                    }
                    else
                    {
                        app.ActiveUIDocument.Document.ActiveView.SetFilterVisibility(el.elementId, el.IsOn);
                    }
                    
                }
                CurrentFilterList = UpdateCurenFilters();
                t.Commit();
            }
            

        }


        public void Initialize()
        {
            externalEvent = ExternalEvent.Create(this);
        }
        public void Raise() => externalEvent.Raise();
        public void Execute(UIApplication app1) => SetFilters(app1);

        public string GetName() => nameof(FilterService);

    }
}
