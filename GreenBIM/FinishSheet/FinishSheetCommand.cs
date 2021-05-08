using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Autodesk.Revit.DB;
using Autodesk.Revit.UI;
using GreenBIM.FinishSheet.Service;
using GreenBIM.FinishSheet.ViewModel;

namespace GreenBIM.FinishSheet
{
    [Autodesk.Revit.Attributes.Transaction(Autodesk.Revit.Attributes.TransactionMode.Manual)]
    class FinishSheetCommand : IExternalCommand
    {
        public Result Execute(ExternalCommandData commandData, ref string message, ElementSet elements)
        {
            Stopwatch sw = new Stopwatch();
            sw.Start();
            UserSettings userSettings = new UserSettings();
            var intersectionFinder = new IntersectionFinder(commandData.Application.ActiveUIDocument, userSettings);
            if(userSettings.FillElementParameters)
            {
                intersectionFinder.GetIntersection2();
            }
            //intersectionFinder.TablCreate();
            sw.Stop();
            TaskDialog.Show("3", sw.Elapsed.ToString());
            return Result.Succeeded;
        }
    }
}
