using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Autodesk.Revit.DB;

namespace GreenBIM.FinishSheet.ViewModel
{
    class ElementGroupByNumber_Categoty
    {
        public ElementGroupByNumber_Categoty()
        {

        }
        private string _roomName;
        private string _roomNumber;
        private List<ElementModel> _allElementsForRoom;
        private List<ElementModel> _walls = new List<ElementModel>();
        private List<ElementModel> _floors = new List<ElementModel>();
        private List<ElementModel> _ceilings = new List<ElementModel>();


        /// <summary>
        /// Имя комнаты
        /// </summary>
        public string RoomName
        {
            get => _roomName;
            set
            {
                _roomName = value;
            }
        }
        /// <summary>
        /// Номер комнаты
        /// </summary>
        public string RoomNumber
        {
            get => _roomNumber;
            set
            {
                _roomNumber = value;
            }
        }
        /// <summary>
        /// Поля для получения всех элементов комнаты
        /// </summary>
        public List<ElementModel> AllElementForRoom
        {
            set { _allElementsForRoom = value; }
        }

        /// <summary>
        /// Получение словаря стен Марка - Площадь
        /// </summary>
        public Dictionary<string, double> WallMarkSquareDict
        {
            get
            {
                var new_dict = new Dictionary<string, double>();
                foreach(var el in _walls)
                {
                    if(!new_dict.ContainsKey(el.MarkOfType))
                    {
                        new_dict.Add(el.MarkOfType, Math.Round(el.Squre , 2));
                    }
                    else
                    {
                        new_dict[el.MarkOfType] += Math.Round(el.Squre , 2);
                    }
                }
                return new_dict;
            }
        }
        /// <summary>
        /// Получение словаря стен Марка - Инфо
        /// </summary>
        public Dictionary<string, string> WallMarkinfoDict
        {
            get
            {
                var new_dict = new Dictionary<string, string>();
                foreach (var el in _walls)
                {
                    if (!new_dict.ContainsKey(el.MarkOfType))
                    {
                        new_dict.Add(el.MarkOfType, el.InfoAboutMaterial);
                    }
                    else
                    {

                    }

                }
                return new_dict;
            }
        }

        /// <summary>
        /// Получение словаря полов Марка - Площадь
        /// </summary>
        public Dictionary<string, double> FloorMarkSquareDict
        {
            get
            {
                var new_dict = new Dictionary<string, double>();
                foreach (var el in _floors)
                {
                    if (!new_dict.ContainsKey(el.MarkOfType))
                    {
                        new_dict.Add(el.MarkOfType, Math.Round(el.Squre , 2));
                    }
                    else
                    {
                        new_dict[el.MarkOfType] += Math.Round(el.Squre , 2);
                    }
                }
                return new_dict;
            }
        }
        /// <summary>
        /// Получение словаря полов Марка - Инфо
        /// </summary>
        public Dictionary<string, string> FloorMarkinfoDict
        {
            get
            {
                var new_dict = new Dictionary<string, string>();
                foreach (var el in _floors)
                {
                    if (!new_dict.ContainsKey(el.MarkOfType))
                    {
                        new_dict.Add(el.MarkOfType, el.InfoAboutMaterial);
                    }

                }
                return new_dict;
            }
        }

        /// <summary>
        /// Получение словаря полов Марка - Площадь
        /// </summary>
        public Dictionary<string, double> CeilingMarkSquareDict
        {
            get
            {
                var new_dict = new Dictionary<string, double>();
                foreach (var el in _ceilings)
                {
                    if (!new_dict.ContainsKey(el.MarkOfType))
                    {
                        new_dict.Add(el.MarkOfType, Math.Round(el.Squre,2));
                    }
                    else
                    {
                        new_dict[el.MarkOfType] += Math.Round(el.Squre , 2);
                    }
                }
                return new_dict;
            }
        }
        /// <summary>
        /// Получение словаря полов Марка - Инфо
        /// </summary>
        public Dictionary<string, string> CeilingMarkinfoDict
        {
            get
            {
                var new_dict = new Dictionary<string, string>();
                foreach (var el in _ceilings)
                {
                    if (!new_dict.ContainsKey(el.MarkOfType))
                    {
                        new_dict.Add(el.MarkOfType, el.InfoAboutMaterial);
                    }

                }
                return new_dict;
            }
        }

        /// <summary>
        /// Сортировка элементам по категории
        /// </summary>
        public void SortAllElementfromGroup()
        {
            foreach(var element in _allElementsForRoom)
            {
                if(element.element is Wall)
                {
                    _walls.Add(element);
                }
                else if (element.element is Floor)
                {
                    _floors.Add(element);
                }
                else if (element.element is Ceiling)
                {
                    _ceilings.Add(element);
                }
            }
        }

        public int GetRowCount()
        {
            int count = WallMarkSquareDict.Count();
            if (count < FloorMarkSquareDict.Count()) count = FloorMarkSquareDict.Count();
            if (count < CeilingMarkSquareDict.Count()) count = CeilingMarkSquareDict.Count();
            return count;
        }

    }
}
