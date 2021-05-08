using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Autodesk.Revit.UI;
using Autodesk.Revit.DB;
using GreenBIM.FinishSheet.ViewModel;

namespace GreenBIM.FinishSheet.Service
{
    public class TablCreater
    {
        private GetElements _getElements;
        private UIDocument _uidoc;
        private Document _doc;
        private AppSettings _appSettings;
        public TablCreater(UIDocument uidoc, GetElements getElements, AppSettings appSettings)
        {
            _uidoc = uidoc;
            _doc = uidoc.Document;
            _getElements = getElements;
            _appSettings = appSettings;
        }

        /// <summary>
        /// Получаем выбранные спецификации
        /// </summary>
        private List<Element> GetTable()
        {
            List<Element> tablList = new List<Element>();
            var elidList = _uidoc.Selection.GetElementIds().ToList();
            foreach (var elid in elidList)
            {
                tablList.Add(_doc.GetElement(elid));
            }
            return tablList;
        }

        /// <summary>
        /// Получаем Объект заголовка для работы с ним
        /// </summary>
        /// <param name="schedule"></param>
        /// <returns></returns>
        private TableSectionData GetHeader(Element schedule)
        {
            //Получаем объект изменяемого текста
            ElementId schedId = ((ScheduleSheetInstance)schedule).ScheduleId;
            TableData td = ((ViewSchedule)_doc.GetElement(schedId)).GetTableData();
            //Получаем объект заголовка содержащий текст
            TableSectionData tsd = td.GetSectionData(SectionType.Header);

            return tsd;
        }

        /// <summary>
        /// Создание строк
        /// </summary>
        /// <param name="schedule"></param>
        /// <param name="elementGroupByRoomName"></param>
        private void RowCreater(Element schedule, Dictionary<string, List<ElementModel>> elementGroupByRoomName)
        {
            TableSectionData tsd = GetHeader(schedule);

            var listGroupElement = new List<ElementGroupByNumber_Categoty>();

            foreach(KeyValuePair<string, List<ElementModel>> keyValue in elementGroupByRoomName)
            {
                ElementGroupByNumber_Categoty specialElementGroup = new ElementGroupByNumber_Categoty()
                {
                    AllElementForRoom = keyValue.Value,
                    RoomNumber = keyValue.Key,
                    RoomName = keyValue.Value.First().RoomName
                };
                specialElementGroup.SortAllElementfromGroup();
                listGroupElement.Add(specialElementGroup);
            }

            ///Создаем строки и заполняем их
            int startIndex = 3;
            using(Transaction t = new Transaction(_doc, "Заполнение спецификации"))
            {
                t.Start();
                foreach (var el in listGroupElement)
                {
                    CreateRow(tsd, el, startIndex);
                    startIndex += el.GetRowCount();
                }
                t.Commit();
            }    
            
        }
        /// <summary>
        /// Создать строку
        /// </summary>
        /// <param name="tsd"></param>
        /// <param name="element"></param>
        /// <param name="index"></param>
        private void CreateRow(TableSectionData tsd, ElementGroupByNumber_Categoty element, int index)
        {
            for (int i = 0; i<element.GetRowCount(); i++)
            {
                tsd.InsertRow(i + index);
                //tsd.SetRowHeight(i + index, 0.262467);
            }
            if(element.GetRowCount()>1)
            {
                tsd.MergeCells(new TableMergedCell(index, 0, index + element.GetRowCount()-1, 0));
                tsd.MergeCells(new TableMergedCell(index, 1, index + element.GetRowCount()-1, 1));
            }
            FillColumns(tsd, element, index);


        }
        /// <summary>
        /// Заполнить строку
        /// </summary>
        /// <param name="tsd"></param>
        /// <param name="element"></param>
        /// <param name="index"></param>
        private void FillColumns(TableSectionData tsd, ElementGroupByNumber_Categoty element, int index)
        {
            tsd.SetCellText(index, 0, element.RoomNumber); ///Заполняем первую ячейку
            tsd.SetCellText(index, 1, element.RoomName); // Заполняем вторую ячейку
            int n = index;
            int k = 0;
            foreach (var el in element.FloorMarkinfoDict) // Заполняем третью, четвертую и пятую ячейку
            {

                tsd.SetCellText(n, 2, el.Key);
                tsd.SetCellText(n, 3, el.Value);
                k = el.Value.Length;
                tsd.SetRowHeight(n, Math.Round((double)(k / 9)) * 0.0262467);
                tsd.SetCellText(n, 4, element.FloorMarkSquareDict[el.Key].ToString());
                n++;

                
            }
            n = index;
            foreach (var el in element.CeilingMarkinfoDict)
            {
                tsd.SetCellText(n, 6, el.Key);
                tsd.SetCellText(n, 7, el.Value);
                if(k<el.Value.Length)
                {
                    k = el.Value.Length;
                    tsd.SetRowHeight(n, Math.Round((double)(k / 9)) * 0.0262467);
                }
                tsd.SetCellText(n, 8, element.CeilingMarkSquareDict[el.Key].ToString());
                n++;
                
            }
            n = index;
            foreach (var el in element.WallMarkinfoDict)
            {
                tsd.SetCellText(n, 9, el.Key);
                tsd.SetCellText(n, 10, el.Value);
                if (k < el.Value.Length)
                {
                    k = el.Value.Length;
                    tsd.SetRowHeight(n, Math.Round((double)(k / 9)) * 0.0262467);
                }
                tsd.SetCellText(n, 11, element.WallMarkSquareDict[el.Key].ToString());
                n++;
            }
            



        }

        public void CreateTableTest()
        {
            var selectidTable = GetTable();

            foreach(var table in selectidTable)
            {
                RowCreater(table, _getElements.GroupByRoom());
            }
        }

    }
}
