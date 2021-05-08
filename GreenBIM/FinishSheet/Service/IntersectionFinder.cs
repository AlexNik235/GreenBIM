using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Autodesk.Revit.DB;
using Autodesk.Revit.DB.Architecture;
using GreenBIM.FinishSheet.Helper;
using Autodesk.Revit.UI;
using GreenBIM.FinishSheet.ViewModel;
using Newtonsoft.Json;
using System.IO;

namespace GreenBIM.FinishSheet.Service
{
    public class IntersectionFinder
    {
        private Document _doc;
        private UIDocument _uidoc;
        private GetElements _getElement;
        private AppSettings _appSettings;
        private UserSettings _userSettings;
        private TablCreater _tablCreater;
        public IntersectionFinder(UIDocument uidoc, UserSettings userSettings)
        {
            _uidoc = uidoc;
            _doc = uidoc.Document;
            _userSettings = userSettings;
            _appSettings = JsonConvert.DeserializeObject<AppSettings>(File.ReadAllText(@"D:\Users\nikitenkoaa\Desktop\GreenBIM\GreenBIM\FinishSheet\UsersSetting.json", System.Text.Encoding.Default));
            _getElement = new GetElements(_doc, _appSettings, _userSettings);
            _tablCreater = new TablCreater(uidoc, _getElement, _appSettings);
            
            
        }
        /// <summary>
        /// возвращаетд долю от соллда
        /// </summary>
        /// <param name="room"></param>
        /// <param name="el"></param>
        /// <returns></returns>
        private double GetPart(Room room, Element el)
        {
            List<Solid> roomSolids = (new GetGeometry(room)).Solids;
            List<Solid> elementSolids = (new GetGeometry(el)).Solids;
            double old_part = 1000000.0;
            foreach (var roomSolid in roomSolids)
            {
                foreach (var elementSolid in elementSolids)
                {
                    Solid new_solid = BooleanOperationsUtils.ExecuteBooleanOperation(roomSolid, elementSolid, BooleanOperationsType.Union);
                    double new_part = Math.Round(new_solid.Volume - (roomSolid.Volume + elementSolid.Volume) + new_solid.SurfaceArea-(roomSolid.SurfaceArea + elementSolid.SurfaceArea), 10);
                    if(new_part < old_part)
                    {
                        old_part = new_part;
                    }

                }
            }
            return old_part;

        }

        /// <summary>
        /// Основная проверка пересечения солидов
        /// </summary>
        /// <param name="room">комната</param>
        /// <param name="el">елемент</param>
        /// <returns></returns>
        private bool IsIntersection(Room room, Element el)
        {
            List<Solid> roomSolids = (new GetGeometry(room)).Solids;
            List<Solid> elementSolids = (new GetGeometry(el)).Solids;

            foreach (var roomSolid in roomSolids)
            {
                foreach (var elementSolid in elementSolids)
                {
                    Solid new_solid = BooleanOperationsUtils.ExecuteBooleanOperation(roomSolid, elementSolid, BooleanOperationsType.Union);
                    if (Math.Round((new_solid.Volume - roomSolid.Volume - elementSolid.Volume + new_solid.SurfaceArea - roomSolid.SurfaceArea - elementSolid.SurfaceArea), 11) != 0)
                    {
                        return true;
                    }
                }
            }
            return false;

            //else
            //{
            //    foreach (var roomSolid in roomSolids)
            //    {
            //        foreach (var elementSolid in elementSolids)
            //        {
            //            Solid new_solid = BooleanOperationsUtils.ExecuteBooleanOperation(roomSolid, elementSolid, BooleanOperationsType.Union);
            //            if (Math.Round((new_solid.SurfaceArea -
            //                (roomSolid.SurfaceArea + elementSolid.SurfaceArea)), 10) != 0)
            //            {
            //                return true;
            //            }

            //        }
            //    }
            //    return false;
            //}
            
        }
        /// <summary>
        /// Нах
        /// </summary>
        public void GetIntersection()
        {
            using(Transaction t = new Transaction(_doc, "Заполнение параметров"))
            {
                t.Start();
                string bad_list = "";
                var selectedList = new List<Element>();
                foreach (var room in _getElement.listRoom)
                {
                    foreach (var element in _getElement.allobjects)
                    {
                        try
                        {
                            if (IsIntersection(room, element))
                            {
                                selectedList.Add(element);
                                FillProperties(room, element);
                            }
                        }
                        catch
                        {
                            bad_list += room.Id + "|" + element.Id + "\n";
                        }
                    }
                }
                TaskDialog.Show("2", bad_list);
                var selector = new Selector(_uidoc);
                selector.SelectEleventOnView(selectedList);
                t.Commit();
            }
   
        }
        /// <summary>
        /// Второй подход к поиску пересечений
        /// </summary>
        public void GetIntersection2()
        {
            using(Transaction t = new Transaction(_doc, "заполнение параметров"))
            {
                t.Start();
                string bad_list = "";
                var selectedList = new List<Element>();
                foreach (var roomLevel in _getElement.roomGroup.Keys)
                {
                    foreach (var elementLevel in _getElement.elemGroup.Keys)
                    {
                        if (roomLevel == elementLevel)
                        {
                            foreach (var room in _getElement.roomGroup[roomLevel])
                            {
                                foreach (var el in _getElement.elemGroup[elementLevel])
                                {
                                    try
                                    {
                                        //if (el.part > GetPart(room, el.element))
                                        //{
                                            if (IsIntersection(room, el.element))
                                            {
                                                //selectedList.Add(el.element);
                                                el.part = GetPart(room, el.element);
                                                el.room = room;
                                                el.RoomNumber = room.Number;
                                                el.RoomName = room.Name;
                                                el.hasRoom = true;
                                                FillProperties(room, el.element);
                                                continue;
                                            }
                                            else
                                            {
                                                //element.LookupParameter(_userSetting.NameFillParameter).Set("Не заполнено");
                                            }
                                        //}

                                    }
                                    catch
                                    {
                                        bad_list += room.Id + "|" + el.element.Id + "\n";
                                    }
                                
                                }
                            }
                        }
                    }
                }
                TaskDialog.Show("2", bad_list);
                var selector = new Selector(_uidoc);
                foreach(var key in _getElement.elemGroup.Keys)
                {
                    foreach(var el in _getElement.elemGroup[key])
                    {
                        selectedList.Add(el.element);
                        if(!el.hasRoom)
                        {
                            el.element.LookupParameter(_appSettings.NameFillParameter).Set("Не заполнено");
                            el.RoomName = "Не найдено";
                            el.RoomNumber = "Не найдено";
                        }
                    }
                }
                selector.SelectEleventOnView(selectedList);
                t.Commit();
            }    
            
        }
        /// <summary>
        /// Заполнятор параметров
        /// </summary>
        /// <param name="room"></param>
        /// <param name="el"></param>
        public void FillProperties(Room room, Element el)
        {
            string data = room.LookupParameter(_appSettings.RoomNumberParameter).AsString() + "|" + room.LookupParameter(_appSettings.RoomNameParameter).AsString();
            el.LookupParameter(_appSettings.NameFillParameter).Set(data);
        }

        public void TablCreate()
        {
            _tablCreater.CreateTableTest();
        }

    }
}
