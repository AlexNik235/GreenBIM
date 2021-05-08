namespace GreenBIM.SketchRebar.Services
{
    using Autodesk.Revit.DB;
    using Autodesk.Revit.UI;
    using GreenBIM.SketchRebar.Models;
    using Newtonsoft.Json;
    using System;
    using System.Collections.Generic;
    using System.IO;
    using System.Linq;
    using System.Text;
    using System.Threading.Tasks;
    public class RebarSketchService
    {
        private readonly UIDocument _uidoc;
        private readonly Document _doc;
        private ScheduleSheetInstance _scheduleSheetInstance;
        private ViewSchedule _viewSchedule;
        private List<AnnotationSymbol> _annotationSymbolList;
        private TableSectionData _tsd;
        private BoundingBoxXYZ _boundingBox;
        private ScheduleDefinition _scheduleDefinition;
        private readonly List<string> _parametersList = JsonConvert.DeserializeObject<List<string>>(File.ReadAllText(@"D:\Users\nikitenkoaa\Desktop\GreenBIM\GreenBIM\SketchRebar\appSettings.RebarSketch.json", System.Text.Encoding.Default));

        public RebarSketchService(UIDocument uidoc)
        {
            _uidoc = uidoc;
            _doc = uidoc.Document;
            GetSchedule(); // Получаем спецификацию
        }

        /// <summary>
        /// Получаем спецификацию
        /// </summary>
        private void GetSchedule()
        {
            // Получаем выбранный элемент
            var selection = _uidoc
                .Selection
                .GetElementIds()
                .Select(i => _doc.GetElement(i))
                .FirstOrDefault();

            // Если элемент является спецификацией запишем спецификацию
            if (selection is ScheduleSheetInstance)
            {
                _scheduleSheetInstance = selection as ScheduleSheetInstance;
                _viewSchedule = (ViewSchedule)_doc.GetElement(_scheduleSheetInstance.ScheduleId);

                // Проверка является ли спецификация ведомостью деталей
                if (!_scheduleSheetInstance.Name.Contains("Ведомость деталей"))
                {
                    TaskDialog.Show("Ошибка", "Выбрана не ведомость деталей");
                    return;
                }    
            }

            // Если элемент яляется группой то расскидаем значения
            else if (selection is Group)
            {
                var listElement = ((Group)selection)
                    .GetMemberIds()
                    .Select(i => _doc.GetElement(i))
                    .ToList();
                _scheduleSheetInstance = listElement
                    .Where(i => i is ScheduleSheetInstance)
                    .Cast<ScheduleSheetInstance>()
                    .ToList()
                    .FirstOrDefault();
                if (_scheduleSheetInstance == null)
                {
                    TaskDialog.Show("Ошибка", "В группе нет спецификации");
                    return;
                }

                // Проверка является ли спецификация ведомостью деталей
                if (!_scheduleSheetInstance.Name.Contains("Ведомость деталей"))
                {
                    TaskDialog.Show("Ошибка", "Выбрана не ведомость деталей");
                    return;
                }
                _viewSchedule = (ViewSchedule)_doc.GetElement(_scheduleSheetInstance.ScheduleId);
                _annotationSymbolList = listElement
                    .Where(i => i is AnnotationSymbol)
                    .Cast<AnnotationSymbol>()
                    .ToList();
            }
            else
            {
                TaskDialog.Show("Ошибка", "Не выбрана спецификация или группа со спецификацией");
                return;
            }
            // Получаем объект TableSectionData
            _tsd = _viewSchedule.GetTableData().GetSectionData(SectionType.Body);

            // Получаем баундингБокс
            _boundingBox = _scheduleSheetInstance.get_BoundingBox(_doc.ActiveView);

            // Получаем представление спецификации
            _scheduleDefinition = _viewSchedule.Definition;
        }

        /// <summary>
        /// Определяет имеется ли одна колонка или более
        /// </summary>
        public bool IsOneColumn()
        {


            // Получаем ширину спецификации
            double scheduleWidth = 0;
            for (int i = 0; i < _tsd.NumberOfColumns; i++)
            {
                scheduleWidth += _tsd.GetColumnWidth(i);
            }

            // Ширина баундинг бокса
            double boundingBoxWidth = Math.Abs(_boundingBox.Min.X - _boundingBox.Max.X);

            if (boundingBoxWidth/ scheduleWidth <1.2)
            {
                return true;
            }
            else
            {
                return false;
            }


        }

        /// <summary>
        /// Определяем количество колонок
        /// </summary>
        /// <returns></returns>
        public int CountColumns()
        {

            // Получаем ширину спецификации
            double scheduleWidth = 0;
            for (int i = 0; i < _tsd.NumberOfColumns; i++)
            {
                scheduleWidth += _tsd.GetColumnWidth(i);
            }

            // Ширина баундинг бокса
            double boundingBoxWidth = Math.Abs(_boundingBox.Min.X - _boundingBox.Max.X);

            return (int)Math.Round(boundingBoxWidth / scheduleWidth + 0.031168, 0);

        }

        /// <summary>
        /// Получаем список моделей строк
        /// </summary>
        /// <param name="listUserSettings">Список пользовательских настроек со строками</param>
        private List<RowModel> CreateModelList(List<UserSettings> listUserSettings)
        {
            var rowModelList = new List<RowModel>();
            if (IsOneColumn())
            {
                var firstCellPosition = GetStartPoint();

                // Проходимся по циклу всех строк
                for (int j = 4; j < _tsd.NumberOfRows; j++)
                {
                    var rowModel = new RowModel()
                    {
                        
                        coordinate = new XYZ(firstCellPosition.X, firstCellPosition.Y - (0.08431759) *(j-4), firstCellPosition.Z), //понять как вычислить толщину строки
                        SheduleFieldList = new List<ScheduleFieldModel>(),
                        NumberOfRow = j,
                        NumberOfColumn = 1
                    };
                    rowModelList.Add(rowModel);
                }
                FillListModelParameters(rowModelList);
                return rowModelList;

            }
            else
            {
                // Стартовая позиция первой модели
                var firstCellPosition = GetStartPoint();
                // опеределяем смещение равно ширине спеки
                double dx = 0;
                for (int i = 0; i < _tsd.NumberOfColumns; i++)
                {
                    dx += _tsd.GetColumnWidth(i);
                }
                dx += 0.031168; //Добавление отступа между спек
                var numberOfRow = 4;
                foreach (var el in listUserSettings)
                {

                    for (int j = 0; j < el.NumberOfRow; j++)
                    {

                        var rowModel = new RowModel()
                        {

                            coordinate = new XYZ(firstCellPosition.X + dx * listUserSettings.IndexOf(el), firstCellPosition.Y - (0.08431759) * (j), firstCellPosition.Z), //понять как вычислить толщину строки
                            SheduleFieldList = new List<ScheduleFieldModel>(),
                            NumberOfRow = numberOfRow,
                            NumberOfColumn = listUserSettings.IndexOf(el)+1
                        };
                        
                        rowModelList.Add(rowModel);
                        numberOfRow++;
                    }
                }
                
                FillListModelParameters(rowModelList);
                return rowModelList;

            }
        }

        /// <summary>
        /// Заполнение листа моделей значением параметров
        /// </summary>
        /// <param name="rowModelList"></param>
        private void FillListModelParameters(List<RowModel> rowModelList)
        {
            foreach (var parameter in _parametersList)
            {

                OpenColumnByName(parameter);
                for (int j = 4; j < _tsd.NumberOfRows; j++)
                {
                    if (parameter == "КМ.ГруппаКонструкций" || parameter == "Арм.НомерПодтипаФормы")
                    {
                        var el = rowModelList.Where(i => i.NumberOfRow == j).ToList().First();
                        if (parameter == "КМ.ГруппаКонструкций")
                        {

                            el.ArmNumberOfForm = _tsd.GetCellText(j, _tsd.LastColumnNumber);

                        }
                        else if (parameter == "Арм.НомерПодтипаФормы")
                        {
                            if (_tsd.GetCellText(j, _tsd.LastColumnNumber) == string.Empty)
                            {
                                el.ArmNumberOfUnderForm = "0";
                            }
                            else
                            {
                                el.ArmNumberOfUnderForm = _tsd.GetCellText(j, _tsd.LastColumnNumber);
                            }


                        }

                    }
                    else
                    {
                        var el = rowModelList.Where(i => i.NumberOfRow == j).ToList().First();

                        var test = _viewSchedule.GetCellText(SectionType.Body,j, _tsd.LastColumnNumber);
                        if (_viewSchedule.GetCellText(SectionType.Body, j, _tsd.LastColumnNumber) != string.Empty)
                        {
                            var newCell = new ScheduleFieldModel()
                            {
                                FieldName = parameter,
                                ValueAsString = _viewSchedule.GetCellText(SectionType.Body, j, _tsd.LastColumnNumber)
                            };
                            el.SheduleFieldList.Add(newCell);
                        }

                    }
                }
                CloseColumnByName(parameter);
            }
        }

        /// <summary>
        /// Получаем стартовую точку
        /// </summary>
        /// <returns></returns>
        private XYZ GetStartPoint()
        {
            // Получаем позиции первой клетки
            var firstYposition = _scheduleSheetInstance.Point.Y;
            for (int i = 0; i < 4; i++)
            {
                firstYposition -= _tsd.GetRowHeight(i);
            }
            firstYposition -= (0.08431759 - 0.00557743); //понять как вычислить толщину строки

            var firstXposition = _scheduleSheetInstance.Point.X;
            for (int i = 0; i < 2; i++)
            {
                firstXposition += _tsd.GetColumnWidth(i);
            }
            firstXposition += _tsd.GetColumnWidth(3) / 2;

            var firstCellPosition = new XYZ(firstXposition, firstYposition, _scheduleSheetInstance.Point.Z);

            return firstCellPosition;
        }
        /// <summary>
        /// Поиск моделей
        /// </summary>
        /// <param name="listModels"></param>
        private void FindFamily(List<RowModel> listModels)
        {
            // Находим список семейств
            var FamilyList = new FilteredElementCollector(_doc)
                .WhereElementIsElementType()
                .OfCategory(BuiltInCategory.OST_GenericAnnotation)
                .Where(i => ((AnnotationSymbolType)i).Family.Name.Contains("Ведомость деталей"))
                .ToList();

            foreach (var model in listModels)
            {
                var el = FamilyList
                    .Where(i => i.LookupParameter("КМ.ГруппаКонструкций").AsValueString() == model.ArmNumberOfForm && i.LookupParameter("Арм.НомерПодтипаФормы").AsValueString() == model.ArmNumberOfUnderForm)
                    .ToList()
                    .FirstOrDefault();
                if (el == null)
                {
                    TaskDialog.Show("Ошибка", $"Картинки с номером формы {model.ArmNumberOfForm} и номером подформы {model.ArmNumberOfUnderForm} не существует");
                    return ;
                }
                model.family = el;
            }

        }


        private void FillFamilyInstance(FamilyInstance familyInstance, RowModel rowModel)
        {
            foreach (Parameter parameter in familyInstance.Parameters)
            {
                foreach(var field in rowModel.SheduleFieldList)
                {
                    if (parameter.Definition.Name == field.FieldName)
                    {
                        parameter.Set(field.ValueAsString);
                    }

                }
            }
        }
        /// <summary>
        /// Заполнение спецификации
        /// </summary>
        /// <param name="listModels"></param>
        private void FillSchedule(List<RowModel> listModels)
        {
            var sel = _uidoc.Selection.GetElementIds().Select(i => _doc.GetElement(i)).ToList().First();
            if (sel is Group)
            {
                ((Group)sel).UngroupMembers().Where(i => i != _scheduleSheetInstance.Id).Select(i => _doc.Delete(i)).ToList().FirstOrDefault();
            }

            var familyList = new List<ElementId>();
            foreach (var model in listModels)
            {
                var newFamily = _doc.Create.NewFamilyInstance(model.coordinate, (FamilySymbol)model.family, _doc.ActiveView);
                familyList.Add(newFamily.Id);
                FillFamilyInstance(newFamily, model);

            }
            familyList.Add(_scheduleSheetInstance.Id);
            _doc.Create.NewGroup(familyList);
        }
        public void Start(List<UserSettings> list)
        {

            using (Transaction t = new Transaction(_doc, "тест"))
            {
                t.Start();
                var modelList = CreateModelList(list);
                FindFamily(modelList);
                FillSchedule(modelList);
                t.Commit();
            }
            TaskDialog.Show("1", "Готово");
        }

        /// <summary>
        /// Открытие ячеек спецификации
        /// </summary>
        private void OpenColumns()
        {
            for (int i = 0; i < _scheduleDefinition.GetFieldCount(); i++)
            {
                if (_parametersList.Contains(_scheduleDefinition.GetField(i).GetName()))
                {
                    _scheduleDefinition.GetField(i).IsHidden = false;
                }
            }
        }

        /// <summary>
        /// Закрытие колонок спецификации
        /// </summary>
        private void CloseColumns()
        {
            for (int i = 0; i < _scheduleDefinition.GetFieldCount(); i++)
            {
                if (_parametersList.Contains(_scheduleDefinition.GetField(i).GetName()))
                {
                    _scheduleDefinition.GetField(i).IsHidden = true;
                }
            }
        }

        /// <summary>
        /// Открыть ячейку по имени
        /// </summary>
        /// <param name="str"></param>
        private void OpenColumnByName(string str)
        {
            for (int i = 0; i < _scheduleDefinition.GetFieldCount(); i++)
            {
                if (_scheduleDefinition.GetField(i).GetName() == str)
                {
                    _scheduleDefinition.GetField(i).IsHidden = false;
                    _tsd.RefreshData();
                    _doc.Regenerate();
                    break;
                }
            }
        }

        /// <summary>
        /// Закрыть ячейку по имени
        /// </summary>
        /// <param name="str"></param>
        private void CloseColumnByName(string str)
        {
            for (int i = 0; i < _scheduleDefinition.GetFieldCount(); i++)
            {
                if (_scheduleDefinition.GetField(i).GetName() == str)
                {
                    _scheduleDefinition.GetField(i).IsHidden = true;
                    _tsd.RefreshData();
                    _doc.Regenerate();
                    break;
                }
            }
        }
    }
}
