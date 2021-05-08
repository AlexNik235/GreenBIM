using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Autodesk.Revit.UI;
using System.Windows.Media.Imaging;
using Autodesk.Revit.DB;
using Autodesk.Revit.UI.Selection;
using System.Windows.Forms;
using GreenBIM.AppServise.ViewModel;
using GreenBIM.AppServise.View;

namespace GreenBIM
{
    using AppServise;
    [Autodesk.Revit.Attributes.Transaction(Autodesk.Revit.Attributes.TransactionMode.Manual)]
    public class App: IExternalApplication
    {
        
        public Result OnStartup(UIControlledApplication application)
        {

            //new AppServise.ButtonCreator(application);
            //try
            //{
                MainContext VM = new MainContext(application);
                if (VM.IsDirectoryExist)
                {
                    if(VM.IsFileConfigExist)
                    {
                        VM.CreateButtons();
                    }
                    else
                    {
                        MainWindow mainWindow = new MainWindow
                        {
                            DataContext = VM

                        };
                        mainWindow.Show();
                    }
                   

                }
                else
                {
                TaskDialog.Show("Ошибка", "Отсутствует файл с дирректорией");
                return Result.Failed;

                }
                return Result.Succeeded;
            //}
            //catch
            //{
            //    MessageBox.Show("Не удалось сформировать окно");
            //    return Result.Failed;
            //}



            // string assemblyPath = System.Reflection.Assembly.GetExecutingAssembly().Location;

            // //Создаем вкладку
            // string tabName = "GreenBIM";
            // try { application.CreateRibbonTab(tabName);  } catch { }

            // //Создаем панель видов
            // string panelName = "Виды";
            // RibbonPanel panel1 = null;
            // List<RibbonPanel> tryPanels = application.GetRibbonPanels(tabName).Where(i => i.Name == panelName).ToList();
            // if (tryPanels.Count == 0)
            // {
            //     panel1 = application.CreateRibbonPanel(tabName, panelName);
            // }
            // else
            // {
            //     panel1 = tryPanels.First();
            // }

            // //Создаем панель проверок
            // string panelName1 = "Проверки";
            // RibbonPanel panel2 = null;
            // tryPanels = application.GetRibbonPanels(tabName).Where(i => i.Name == panelName1).ToList();
            // if (tryPanels.Count == 0)
            // {
            //     panel2 = application.CreateRibbonPanel(tabName, panelName1);
            // }
            // else
            // {
            //     panel2 = tryPanels.First();
            // }

            // //Создаем панель для бимов
            // string panelName2 = "BIM";
            // RibbonPanel BIMpanel = null;
            // tryPanels = application.GetRibbonPanels(tabName).Where(i => i.Name == panelName2).ToList();
            // if (tryPanels.Count == 0)
            // {
            //     BIMpanel = application.CreateRibbonPanel(tabName, panelName2);
            // }
            // else
            // {
            //     BIMpanel = tryPanels.First();
            // }

            // //Создаем кнопку "Корректировки названия"
            // string buttonName1 = "RenameViews";
            // string buttonText1 = "Корректировка \n названия";
            // PushButton btn1 = panel1.AddItem(new PushButtonData(
            //     buttonName1,
            //     buttonText1,
            //     assemblyPath,
            //     "GreenBIM.RenameViews")
            //     ) as PushButton;
            // btn1.LargeImage = PngImageSource("GreenBIM.Resources.gbscripts32.png");

            // //Создаем кнопку "Проверка вложенных"
            // string buttonName2 = "GetInputElement";
            // string buttonText2 = "Проверка \n вложенных";
            // PushButton btn2 = panel2.AddItem(new PushButtonData(
            //     buttonName2,
            //     buttonText2,
            //     assemblyPath,
            //     "GreenBIM.GetInputElement")
            //     ) as PushButton;
            //btn2.LargeImage = PngImageSource("GreenBIM.Resources.gbscripts32.png");

            // //Создаем кнопку "Соединения коннекторов"
            // string buttonName3 = "ConnectingConnectors";
            // string buttonText3 = "Соединение \n коннекторов";
            // PushButton btn3 = BIMpanel.AddItem(new PushButtonData(
            //     buttonName3,
            //     buttonText3,
            //     assemblyPath,
            //     "GreenBIM.ConnectingConnectors")
            //     ) as PushButton;
            // btn3.LargeImage = PngImageSource("GreenBIM.Resources.gbscripts32.png");

            // //Создаем кнопку "Тест типоразмеров семейств"
            // string buttonName4 = "FamilySymbolTest";
            // string buttonText4 = "Тест типоразмеров \n семейств";
            // PushButton btn4 = BIMpanel.AddItem(new PushButtonData(
            //     buttonName4,
            //     buttonText4,
            //     assemblyPath,
            //     "GreenBIM.FamilySymbolTestPr.FamilySymbolTest")
            //     ) as PushButton;
            // btn4.LargeImage = PngImageSource("GreenBIM.Resources.gbscripts32.png");




        }

        public Result OnShutdown(UIControlledApplication application)
        {
            return Result.Succeeded;
        }

        private System.Windows.Media.ImageSource PngImageSource(string embeddedPathname)
        {
            System.IO.Stream st = this.GetType().Assembly.GetManifestResourceStream(embeddedPathname);

            PngBitmapDecoder decoder = new PngBitmapDecoder(st, BitmapCreateOptions.PreservePixelFormat, BitmapCacheOption.Default);

            return decoder.Frames[0];
        }


    }
}
