using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Autodesk.Revit.DB;
using Autodesk.Revit.UI;
using GreenBIM.ElementAnchor.View;

namespace GreenBIM.ElementAnchor
{
    using ViewModel;
    [Autodesk.Revit.Attributes.Transaction(Autodesk.Revit.Attributes.TransactionMode.Manual)]
    class ElementAnchor : IExternalCommand
    {
        public Document _doc;
        private MainWindow mainWindow;

        public Result Execute(ExternalCommandData commandData, ref string message, ElementSet elements)
        {
            _doc = commandData.Application.ActiveUIDocument.Document;

            try
            {
                var ViewModel = new ViewModel.ViewModel(_doc);
                mainWindow = new MainWindow
                {
                    DataContext = ViewModel
                };
                mainWindow.ShowDialog();
                return Result.Succeeded;
            }
            catch
            {
                TaskDialog.Show("Ошибка", "Ошибка при создании окна");
                return Result.Failed;
            }

            
        }
    }
}
