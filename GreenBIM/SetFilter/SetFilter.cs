using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Autodesk.Revit.DB;
using Autodesk.Revit.UI;
using Autodesk.Revit.UI.Selection;
using Autodesk.Revit.DB.Mechanical;


namespace GreenBIM.SetFilter
{
    using GreenBIM.SetFilter.ViewModel;
    using Views;
    [Autodesk.Revit.Attributes.Transaction(Autodesk.Revit.Attributes.TransactionMode.Manual)]
    class SetFilter : IExternalCommand
    {

        public Result Execute(ExternalCommandData commandData, ref string message, ElementSet elements)
        {
            MainContext mainContext = new MainContext(commandData.Application);
            MainWindow windowSecond = new MainWindow()
            {
                DataContext = mainContext
            };
            windowSecond.Show();
            return Result.Succeeded;
        }


    }
}
