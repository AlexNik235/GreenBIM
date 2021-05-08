using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.IO;
using System.ComponentModel;
using GreenBIM.AppServise.ViewModel;
using System.Windows.Forms;

namespace GreenBIM.AppServise.Service
{
    class CheckAndCreateConfig
    {
        //Блок переменных для работы с дирректориями
        private string fileConfigName;//имя файла конфига
        private string DirectoryName;// заменить на значение из файла конфига
        private string ParentDirPath = System.Reflection.Assembly.GetExecutingAssembly()
            .Location.Remove(System.Reflection.Assembly.GetExecutingAssembly()
            .Location.LastIndexOf(@"\", StringComparison.Ordinal)); //Путь к дирректории где лежит файл
        public string FullPathToFileConfig; //Полный путь до файла конфига
        public string FullPathToDirectory;  //Полный путь до директории

        public CheckAndCreateConfig(AppSetting appSetting)
        {
            fileConfigName = appSetting.ConfigFileName;
            DirectoryName = appSetting.DirectoryName;
            FullPathToFileConfig = Path.Combine(ParentDirPath, DirectoryName, fileConfigName);
            FullPathToDirectory = Path.Combine(ParentDirPath, DirectoryName);
        }
        /// <summary>
        /// Проверяет наличие директории в папке с dll файлом
        /// </summary>
        /// <returns></returns>
        public bool SearchCreatableDirectory()
        {
            foreach(string directoryName in Directory.GetDirectories(ParentDirPath))
            {
                if(directoryName.Contains(DirectoryName))
                {
                    return true;
                }    
            }
            return false;
        }
        /// <summary>
        /// Создает и возвращает путь к новой директории с файлом конфигом
        /// </summary>
        /// <returns></returns>
        //private string CreateDirectory()
        //{
        //    string newDirectory = Path.Combine(ParentDirPath, DirectoryName);
        //    Directory.CreateDirectory(newDirectory);
        //    return newDirectory;
        //}


        /// <summary>
        /// Запись в файл 
        /// </summary>
        /// <param name="pathToFile">Путь к файлу</param>
        /// <param name="data">Данные</param>
        public void FileFilling(string pathToFile, string data)
        {

            using (StreamWriter file = new StreamWriter(pathToFile, false, System.Text.Encoding.UTF8))

            {
                file.WriteLine(data);
            }
        }
        /// <summary>
        /// Получить строку из имен панелей для записи
        /// </summary>
        /// <param name="bindingList"></param>
        /// <returns></returns>
        private string GetTabsName(BindingList<TabModel> bindingList)
        {
            string result_string = "";
            foreach(var element in bindingList)
            {
                if (element.IsCheck)
                {
                    result_string += element.TabName + "|";
                }
            }
            return result_string;
        }
        public void CreateConfigAndFillData(BindingList<TabModel> bindingList)
        {
            //string newDirectory = CreateDirectory(); // создаем директори и получаем имя;
            FileFilling(FullPathToFileConfig, GetTabsName(bindingList)); // получаем строку панелей и заполняем ей файл

        }

        /// <summary>
        /// Поиск файла конфига
        /// </summary>
        /// <returns></returns>
        public bool IsConfigFileExist()
        {
            foreach(var el in Directory.GetFiles( Path.Combine(ParentDirPath, DirectoryName)))
            {
                if(el.Contains(fileConfigName))
                {
                    return true;
                }
            }
            return false;
        }
   
    }
}
