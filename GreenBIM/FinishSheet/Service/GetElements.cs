using Autodesk.Revit.DB;
using Autodesk.Revit.DB.Architecture;
using GreenBIM.FinishSheet.ViewModel;
using System;
using System.Collections.Generic;
using System.Linq;


namespace GreenBIM.FinishSheet.Service
{
    public class GetElements
    {
        private string StringInDocName { get; set; } ///Переменная для фильтрации связанных документов
        private Document _doc;
        private Document _linkDoc;
        public List<Room> listRoom;
        public List<Element> listWall;
        public List<Element> listFloor;
        public List<Element> listCeilings;
        public List<Element> allobjects = new List<Element>();
        public Dictionary<string, List<ElementModel>> elemGroup;
        public Dictionary<string, List<Room>> roomGroup;
        private List<ElementModel> ListElementModel = new List<ElementModel>();
        private UserSettings _userSettings;


        private AppSettings _appSettings { get; set; }
        public GetElements(Document doc, AppSettings appSettings, UserSettings userSettings)
        {
            _doc = doc;
            _appSettings = appSettings;
            _userSettings = userSettings;
            StringInDocName = _appSettings.StringInDocName;
            _linkDoc = GetLinkedDocument(StringInDocName);
            listRoom = new FilteredElementCollector(_linkDoc)
                .OfCategory(BuiltInCategory.OST_Rooms)
                .Where(i=>i.Location != null)
                .Cast<Room>()
                .ToList();
            listWall = new FilteredElementCollector(_doc)
                .OfClass(typeof(Wall))
                .ToList();
            if(listWall.Count()>0) allobjects.AddRange(listWall);
            listFloor = new FilteredElementCollector(_doc)
                .OfClass(typeof(Floor))
                .ToList();
            if(listFloor.Count()>0) allobjects.AddRange(listFloor);
            listCeilings = new FilteredElementCollector(_doc)
                .OfClass(typeof(Ceiling))
                .ToList();
            if(listCeilings.Count()>0) allobjects.AddRange(listCeilings);
            roomGroup = GroupRoom(listRoom);
            elemGroup = GroupElement(roomGroup, allobjects);


        }

        /// <summary>
        /// Получаем документ связанного файла
        /// </summary>
        /// <returns></returns>
        private Document GetLinkedDocument(string StringInName)
        {
            ///Создаем фильтр для получения RevitLinkInstanse
            ElementClassFilter fil = new ElementClassFilter(typeof(RevitLinkInstance));
            RevitLinkType LinkedFileType = new FilteredElementCollector(_doc)
                .OfClass(typeof(RevitLinkType))
                .Cast<RevitLinkType>()
                .Where(i => i.Name.Contains(StringInName))
                .ToList()
                .First();
            ///Получаем айдишник инстанса связи
            ElementId linkInstanceId = (ElementId)(LinkedFileType.GetDependentElements(fil).First());
            Document _linkDoc = ((RevitLinkInstance)_doc.GetElement(linkInstanceId)).GetLinkDocument();
            return _linkDoc;
        }
        //private List<dynamic> GetElem(Document doc, dynamic type)
        //{

        //    var list = new FilteredElementCollector(doc)
        //        .OfClass(type.GetType())
        //        .Cast<type.GetType()>()
        //        .ToList();
        //    return list;

        //}

        /// <summary>
        /// Группировка помнат по уровню
        /// </summary>
        /// <param name="listRooms">лист с элементами групп</param>
        /// <returns></returns>
        private Dictionary<string, List<Room>> GroupRoom(List<Room> listRooms)
        {
            Dictionary<string, List<Room>> roomGroup = new Dictionary<string, List<Room>>();
            foreach (var room in listRooms)
            {
                string level = Math.Round((((LocationPoint)room.Location).Point).Z).ToString();
                if (!roomGroup.ContainsKey(level))
                {
                    roomGroup.Add(level, new List<Room>());
                }
  
            }
            foreach(var room in listRooms)
            {
                string roomLevel = Math.Round((((LocationPoint)room.Location).Point).Z).ToString();
                roomGroup[roomLevel].Add(room);
            }
            return roomGroup;
        }
        /// <summary>
        /// Сортирует элементы по уровням
        /// </summary>
        /// <param name="listRooms"></param>
        /// <param name="listElements"></param>
        /// <returns></returns>
        private Dictionary<string, List<ElementModel>> GroupElement(Dictionary<string, List<Room>> roomGroup, List<Element> listElements)
        {
            Dictionary<string, List<ElementModel>> elementGroup = new Dictionary<string, List<ElementModel>>();
            foreach(var lv in roomGroup.Keys)
            {
                elementGroup.Add(lv, new List<ElementModel>());
            }
            foreach (var level in roomGroup.Keys)
            {
                foreach(var elem in listElements)
                {
                    ElementId levelId = elem.LevelId;
                    string lv = Math.Round(((Level)_doc.GetElement(levelId)).Elevation).ToString();
                    if (level == lv)
                    {
                        var elementModel = new ElementModel() { level = lv, element = elem, hasRoom = false, room = null, part = 100000.0 };
                        elementGroup[level].Add(elementModel);
                        ListElementModel.Add(elementModel);
                    }
                }
            }
            return elementGroup;
        }

        /// <summary>
        /// Группировка ElementModel по имени комнаты
        /// </summary>
        /// <returns></returns>
        public Dictionary<string, List<ElementModel>> GroupByRoom()
        {
            if (_userSettings.FillElementParameters)
            {
                Dictionary<string, List<ElementModel>> resultDict = new Dictionary<string, List<ElementModel>>();
                foreach (var element in ListElementModel)
                {
                    if (resultDict.ContainsKey(element.RoomNumber))
                    {
                        resultDict[element.RoomNumber].Add(element);
                    }
                    else
                    {
                        var new_listElement = new List<ElementModel>();
                        new_listElement.Add(element);
                        resultDict.Add(element.RoomNumber, new_listElement);

                    }
                }
                resultDict = resultDict.OrderBy(pair => pair.Key).ToDictionary(pair => pair.Key, pair => pair.Value);
                return resultDict;
            }
            else
            {
                Dictionary<string, List<ElementModel>> resultDict = new Dictionary<string, List<ElementModel>>();
                foreach(var el in allobjects)
                {
                    if(el.LookupParameter(_appSettings.NameFillParameter).AsString() == "Не заполнено")
                    {
                        continue;
                    }    
                    string roomNumber = el.LookupParameter(_appSettings.NameFillParameter).AsString().Split(new char[] { '|' })[0];
                    string roomName = el.LookupParameter(_appSettings.NameFillParameter).AsString().Split(new char[] { '|' })[1];
                    if (!resultDict.ContainsKey(roomNumber))
                    {
                        var new_listElement = new List<ElementModel>() { new ElementModel() { element = el, RoomName = roomName, RoomNumber = roomNumber }
                        };
                        resultDict.Add(roomNumber, new_listElement);
                    }
                    else
                    {
                        resultDict[roomNumber].Add(new ElementModel() { element = el, RoomName = roomName, RoomNumber = roomNumber });
                    }

                }
                resultDict = resultDict.OrderBy(pair => pair.Key).ToDictionary(pair => pair.Key, pair => pair.Value);
                return resultDict;
            }
                
            
        }




    }
    //private class DictSort : IComparable
    //{
    //    public int CompareTo(object obj)
    //    {
    //        throw new NotImplementedException();
    //    }
    //}
}
