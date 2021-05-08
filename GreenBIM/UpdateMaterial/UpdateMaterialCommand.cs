using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace GreenBIM.UpdateMaterial
{
    using Autodesk.Revit.DB;
    using Autodesk.Revit.UI;
    using GreenBIM.UpdateMaterial.ViewModels;
    using GreenBIM.UpdateMaterial.View;

    [Autodesk.Revit.Attributes.Transaction(Autodesk.Revit.Attributes.TransactionMode.Manual)]
    class UpdateMaterialCommand : IExternalCommand
    {
        public Result Execute(ExternalCommandData commandData, ref string message, ElementSet elements)
        {
            var mainContext = new MainContext(commandData);
            SelectWindow selectWindow = new SelectWindow()
            {
                DataContext = mainContext
            };

            selectWindow.ShowDialog();
            
            return Result.Succeeded;
        }
    }
}
