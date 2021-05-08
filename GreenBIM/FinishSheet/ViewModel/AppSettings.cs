using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Newtonsoft.Json;

namespace GreenBIM.FinishSheet.ViewModel
{
    public class AppSettings
    {
        private string _stringInDocName;
        private string _nameFillParameter;
        private string _roomNumberParameter;
        private string _roomNameParameter;
        private string _markOfType;
        private string _squareOfElement;
        private string _commentToType;
        /// <summary>
        /// Переменная для фильтрации связанных документов
        /// </summary>
        public string StringInDocName
        {
            get { return _stringInDocName; }
            set { _stringInDocName = value; }
        }
        /// <summary>
        /// Имя параметра в который будет заполняться данные
        /// </summary>
        public string NameFillParameter
        {
            get { return _nameFillParameter; }
            set { _nameFillParameter = value; }
        }
        /// <summary>
        /// Имя параметра из которого будет браться номер
        /// </summary>
        public string RoomNumberParameter
        {
            get { return _roomNumberParameter; }
            set { _roomNumberParameter = value; }
        }
        /// <summary>
        /// Имя параметра из которого будет браться имя
        /// </summary>
        public string RoomNameParameter
        {
            get { return _roomNameParameter; }
            set { _roomNameParameter = value; }
        }
        /// <summary>
        /// Имя параметра Маркировка типоразмера
        /// </summary>
        public string MarkOfType
        {
            get => _markOfType;
            set { _markOfType = value; }
        }

        /// <summary>
        /// Параметр с площадью элемента
        /// </summary>
        public string SquareOfElement
        {
            get => _squareOfElement;
            set { _squareOfElement = value; }
        }
    }
}
