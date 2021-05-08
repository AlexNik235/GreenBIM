using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Autodesk.Revit.DB;
using Autodesk.Revit.DB.Architecture;
using Newtonsoft.Json;
using System.IO;

namespace GreenBIM.FinishSheet.ViewModel
{
    public class ElementModel
    {

        private Element _element;
        private Room _room;
        private string _level;
        private bool _hasRoom;
        private double _part;
        private string _roomName;
        private string _roomNumber;
        private AppSettings _appSettings;

        public Element element
        {
            get { return _element; }
            set { _element = value; }
        }
        public Room room 
        {
            get { return _room; }
            set { _room = value; }
        }
        public string level
        {
            get { return _level; }
            set { _level = value; }
        }
        public bool hasRoom
        {
            get { return _hasRoom; }
            set { _hasRoom = value; }
        }
        /// <summary>
        /// Доля вхождения
        /// </summary>
        public double part
        {
            get { return _part; }
            set { _part = value; }
        }
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
        /// Описание элемента
        /// </summary>
        public string InfoAboutMaterial
        {
            get
            { return (element.Name.Split(new char[] { '_','(', ')' }))[2]; }
        }
        /// <summary>
        /// Площадь элемента
        /// </summary>
        public double Squre
        {
            get
            {
                return (element.LookupParameter("Площадь").AsDouble() / 10.7639);
            }
        }
        /// <summary>
        /// Описание элемента
        /// </summary>
        public string MarkOfType
        {
            get
            { return (element.Name.Split(new char[] { '_','(', ')' }))[0]; }
        }

    }
}
