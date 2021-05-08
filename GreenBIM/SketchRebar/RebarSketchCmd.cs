namespace GreenBIM.SketchRebar
{
    using Autodesk.Revit.DB;
    using Autodesk.Revit.UI;
    using GreenBIM.SketchRebar.View;
    using GreenBIM.SketchRebar.ViewModels;

    [Autodesk.Revit.Attributes.Transaction(Autodesk.Revit.Attributes.TransactionMode.Manual)]
    class RebarSketchCmd : IExternalCommand
    {
        public Result Execute(ExternalCommandData commandData, ref string message, ElementSet elements)
        {
            var mainContext = new MainContext(commandData.Application.ActiveUIDocument);
            if (!mainContext.IsOneColumn)
            {
                MainWindow selectWindow = new MainWindow()
                {
                    DataContext = mainContext
                };

                selectWindow.ShowDialog();
            }
            else
            {
                mainContext.StartCommand();
            }
            
            return Result.Succeeded;
        }
    }
}
