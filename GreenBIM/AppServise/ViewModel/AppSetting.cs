using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace GreenBIM.AppServise.ViewModel
{
    public class AppSetting
    {
        private string _directoryName;
        private string _configName;
        /// <summary>
        /// Имя дирректории куда сохраняться настройки
        /// </summary>
        public string DirectoryName
        {
            get => _directoryName;
            set { _directoryName = value; }
        }
        /// <summary>
        /// Имя файла в который сохраняться настройки
        /// </summary>
        public string ConfigFileName
        {
            get => _configName;
            set { _configName = value; }
        }
        

    }
}
