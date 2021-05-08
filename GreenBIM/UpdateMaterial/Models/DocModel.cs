using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace GreenBIM.UpdateMaterial.Models
{
    using Autodesk.Revit.DB;
    using GalaSoft.MvvmLight;
    using GalaSoft.MvvmLight.CommandWpf;
    using System.ComponentModel;

    public class DocModel:INotifyPropertyChanged
    {
        private Document _doc;
        private string _docName;
        private bool _changeMaterial;
        private bool _isMaterialFile;

        /// <summary>
        /// Является ли документ семейством
        /// </summary>
        public bool IsFamilyModel
        {
            get
            {
                return _doc.IsFamilyDocument;
            }
        }


        

        /// <summary>
        /// Документ
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
        /// Имя документа
        /// </summary>
        public string DocName
        {
            get => _docName;
            set
            {
                _docName = value;
                NotifyPropertyChanged();
            }
        }

        /// <summary>
        /// Нужно ли в этом документе обновлять материалы
        /// </summary>
        public bool ChangeMaterial
        {
            get => _changeMaterial;
            set
            {
                _changeMaterial = value;
                NotifyPropertyChanged();
            }
        }

        /// <summary>
        /// Будут ли браться материалы из этого файла
        /// </summary>
        public bool IsMaterialFile
        {
            get => _isMaterialFile;
            set
            {
                _isMaterialFile = value;
                NotifyPropertyChanged();
            }
        }

        public event PropertyChangedEventHandler PropertyChanged;

        private void NotifyPropertyChanged(string propertyName="")
        {
            PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(propertyName));
        }
    }
}
