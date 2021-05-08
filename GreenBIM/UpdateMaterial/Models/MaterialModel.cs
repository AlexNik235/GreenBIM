using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace GreenBIM.UpdateMaterial.Models
{
    using Autodesk.Revit.DB;
    using System.ComponentModel;

    public class MaterialModel:INotifyPropertyChanged
    {
        private Document _doc;
        private Material _material;
        private bool _choose;


        /// <summary>
        /// Материал модели
        /// </summary>
        public Material material
        {
            get => _material;
            set
            {
                _material = value;
                NotifyPropertyChanged();
            }
        }

        /// <summary>
        /// документ
        /// </summary>
        public Document doc
        {
            get => _doc;
            set
            {
                _doc = value;
                NotifyPropertyChanged();
            }
        }
        /// <summary>
        /// Имя материала
        /// </summary>
        public string materialName
        {
            get
            {
               return _material.Name;
            }
        }

        /// <summary>
        /// Выбран ли материал для какого либо действия
        /// </summary>
        public bool IsChoosen
        {
            get => _choose;
            set
            {
                _choose = value;
                NotifyPropertyChanged();
            }
        }


        public event PropertyChangedEventHandler PropertyChanged;
        private void NotifyPropertyChanged(string propertyName = "")
        {
            PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(propertyName));
        }
    }
}
