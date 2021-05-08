using System;
using System.Collections.Generic;

using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace GreenBIM.UpdateMaterial.Models
{
    using System.ComponentModel;
    public class UserSettings : INotifyPropertyChanged
    {
        private bool _isFamilyDoc;
        private bool _copyMaterial = true;
        private bool _DelitMaterial;
        private bool _updateMaterial;
        private bool _selectAll = false;

        public bool SelectAll
        {
            get => _selectAll;
            set
            {
                _selectAll = value;
                NotifyPropertyChanged();
            }
        }


        /// <summary>
        /// Показывать документы редактора семейств
        /// </summary>
        public bool IsFamilyDocument
        {
            get => _isFamilyDoc;
            set
            {
                _isFamilyDoc = value;
                NotifyPropertyChanged();
            }
        }


        /// <summary>
        /// Копировать материалы
        /// </summary>
        public bool CopyMaterial
        {
            get => _copyMaterial;
            set
            {
                _copyMaterial = value;
                NotifyPropertyChanged();
            }
        }

        /// <summary>
        /// Удалить материалы
        /// </summary>
        public bool DelitMaterial
        {
            get => _DelitMaterial;
            set
            {
                _DelitMaterial = value;
                NotifyPropertyChanged();
            }
        }

        public bool UpdateMaterial
        {
            get => _updateMaterial;
            set
            {
                _updateMaterial = value;
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
