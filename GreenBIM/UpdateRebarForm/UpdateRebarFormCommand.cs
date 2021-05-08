using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace GreenBIM.UpdateRebarForm
{
    using Autodesk.Revit.DB;
    using Autodesk.Revit.UI;
    using GreenBIM.UpdateRebarForm.ViewModels;
    using GreenBIM.UpdateRebarForm.Views;

    [Autodesk.Revit.Attributes.Transaction(Autodesk.Revit.Attributes.TransactionMode.Manual)]
    class UpdateRebarFormCommand : IExternalCommand
    {
        public Result Execute(ExternalCommandData commandData, ref string message, ElementSet elements)
        {
            var mainContext = new MainContext(commandData.Application);
            MainWindow mainWindow = new MainWindow()
            {
                DataContext = mainContext
            };
            mainWindow.ShowDialog();
            return Result.Succeeded;
        }
    }
}
