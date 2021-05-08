using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Autodesk.Revit.UI;
using System.Windows.Media.Imaging;
using Autodesk.Revit.DB;
using Autodesk.Revit.UI.Selection;
using Newtonsoft.Json;
using System.IO;
using System.Windows.Forms;
using GreenBIM.AppServise.ViewModel;

namespace GreenBIM.AppServise.Service
{
    class ButtonCreater
    {

        private string assemblyPath = System.Reflection.Assembly.GetExecutingAssembly().Location; //путь к самому файлу
        //private string ParentDirPath = System.Reflection.Assembly.GetExecutingAssembly()
        //    .Location.Remove(System.Reflection.Assembly.GetExecutingAssembly()
        //    .Location.LastIndexOf(@"\", StringComparison.Ordinal)); //Путь к дирректории где лежит файл
        private UIControlledApplication aplication; //приложение
        private List<RibbonPanel> panels; //Список элементов самих панелей
        private List<ButtonModel> ButtonList; //список всех имеющихся кнопок
        private string _fullPathToDirectory;
        private string _configFileName = "buttonConfig.json";
        public List<string> tabsName = new List<string>();//Список с вкладками


        
        /// <summary>
        /// Конструктор
        /// </summary>
        /// <param name="app"></param>
        public ButtonCreater(UIControlledApplication app, string FullPathToDirectory)
        {
            aplication = app;
            _fullPathToDirectory = FullPathToDirectory;
            var allButton = JsonConvert.DeserializeObject<AllButton>(File.ReadAllText(Path.Combine(_fullPathToDirectory, _configFileName), System.Text.Encoding.Default));
            ButtonList = allButton.Buttons;
            tabsName = GetTabs();

            

        }
        
        /// <summary>
        /// Создаем кнопку
        /// </summary>
        private void CreateButton(ButtonModel button)
        {
            RibbonPanel panel = CreatePanel(button);
            PushButton btn = panel.AddItem(new PushButtonData(
                button.buttonName,
                button.buttonText,
                assemblyPath,
                button.direction)
                ) as PushButton;
            btn.LargeImage = PngImageSource(button.image);
        }

        /// <summary>
        /// Создаем панель
        /// </summary>
        /// <returns></returns>
        private RibbonPanel CreatePanel(ButtonModel button)
        {
            RibbonPanel panel = null;
            CreateTab(button);
            panels = aplication.GetRibbonPanels(button.tabName).Where(i => i.Name == button.panelName).ToList();
            if(panels.Count ==0)
            {
                panel = aplication.CreateRibbonPanel(button.tabName, button.panelName);
            }
            else
            {
                panel = panels.First();
            }
            return panel;

        }

        /// <summary>
        /// Созадем вкладку
        /// </summary>
        private void CreateTab(ButtonModel button)
        {
            try
            {
                aplication.CreateRibbonTab(button.tabName);
            }
            catch { }
        }

        /// <summary>
        /// Поиск конфиг файла в дирректории
        /// </summary>

        /// <summary>
        /// Команда для создания кнопки из конфиг файла
        /// </summary>
        public void CreateButtons(string FullPathToFile)
        {
            List<string> tabsNames = GetTabsnFormConfigFile(FullPathToFile);
            foreach (var btn in ButtonList)
            {
                if(tabsNames.Contains(btn.tabName))
                {
                    CreateButton(btn);
                }
            }
        }
        /// <summary>
        /// Создание картинки для кнопки
        /// </summary>
        /// <param name="embeddedPathname">Путь до картинки внутри сборки</param>
        /// <returns></returns>
        private System.Windows.Media.ImageSource PngImageSource(string embeddedPathname)
        {
            System.IO.Stream st = this.GetType().Assembly.GetManifestResourceStream(embeddedPathname);

            PngBitmapDecoder decoder = new PngBitmapDecoder(st, BitmapCreateOptions.PreservePixelFormat, BitmapCacheOption.Default);

            return decoder.Frames[0];
        }


        /// <summary>
        /// Получение вкладок из файла
        /// </summary>
        /// <param name="FullPathToFile">Полный путь до файла</param>
        /// <returns></returns>
        public List<string> GetTabsnFormConfigFile(string FullPathToFile)
        {
            List<string> resul_List = new List<string>();
            string FileData = "";
            using(StreamReader file = new StreamReader(FullPathToFile,System.Text.Encoding.UTF8))
            {
                FileData = file.ReadToEnd();
            }

            foreach(var el in FileData.Split('|'))
            {
                resul_List.Add(el);
            }
            return resul_List;
        }
        private List<string> GetTabs()
        {
            List<string> result_list = new List<string>();
            foreach(var el in ButtonList)
            {
                if(!result_list.Contains(el.tabName))
                {
                    result_list.Add(el.tabName);
                }
            }
            return result_list;
        }

        /// <summary>
        /// Словарь для создания кнопок
        /// </summary>
        //Dictionary<string, Dictionary<string, string>> configDict = new Dictionary<string, Dictionary<string, string>>()
        //{
        //    {
        //        "ConnectingConnectors", new Dictionary<string, string>()
        //        {
        //            {"buttonName","ConnectingConnectors" },
        //            {"buttonText","Соединение \n коннекторов" },
        //            {"direction", "GreenBIM.ConnectingConnectors" },
        //            {"image", "GreenBIM.Resources.gbscripts32.png" },
        //            {"panelName","BIM" },
        //            {"tabName","GB:BIM" }
        //        }
        //    },
        //    {
        //        "GetInputElement", new Dictionary<string, string>()
        //        {
        //            {"buttonName", "GetInputElement" },
        //            {"buttonText", "Проверка \n вложеных" },
        //            {"direction", "GreenBIM.GetInputElement" },
        //            {"image", "GreenBIM.Resources.gbscripts32.png" },
        //            {"panelName","BIM" },
        //            {"tabName","GB:BIM" }
        //        }
        //    },
        //    {
        //        "RenameViews", new Dictionary<string, string>()
        //        {
        //            {"buttonName", "RenameViews" },
        //            {"buttonText", "Корректировка \n названия" },
        //            {"direction", "GreenBIM.RenameViews" },
        //            {"image", "GreenBIM.Resources.gbscripts32.png" },
        //            {"panelName","Проверки" },
        //            {"tabName","GB:КЖ" }
        //        }
        //    },
        //    {
        //        "FamilySymbolTest", new Dictionary<string, string>()
        //        {
        //            {"buttonName", "FamilySymbolTest" },
        //            {"buttonText", "Тест типоразмеров \n семейств" },
        //            {"direction", "GreenBIM.FamilySymbolTestPr.FamilySymbolTest" },
        //            {"image", "GreenBIM.Resources.gbscripts32.png" },
        //            {"panelName","BIM" },
        //            {"tabName","GB:BIM" }
        //        }
        //    },
        //    {
        //        "SetFilters", new Dictionary<string, string>()
        //        {
        //            {"buttonName", "SetFilters" },
        //            {"buttonText", "Установить \n фильтры" },
        //            {"direction", "GreenBIM.SetFilter.SetFilter" },
        //            {"image", "GreenBIM.Resources.gbscripts32.png" },
        //            {"panelName","Графика" },
        //            {"tabName","GB:КЖ" }
        //        }
        //    },
        //    {
        //        "ElementAnchor", new Dictionary<string, string>()
        //        {
        //            {"buttonName", "ElementAnchor" },
        //            {"buttonText", "Закрепить \n элементы" },
        //            {"direction", "GreenBIM.ElementAnchor.ElementAnchor" },
        //            {"image", "GreenBIM.Resources.gbscripts32.png" },
        //            {"panelName","Общее" },
        //            {"tabName","GB:КЖ" }
        //        }
        //    }



        //};


    }
}
