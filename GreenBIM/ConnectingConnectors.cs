using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Autodesk.Revit.DB;
using Autodesk.Revit.UI;
using Autodesk.Revit.UI.Selection;
using Autodesk.Revit.DB.Mechanical;

namespace GreenBIM
{
    [Autodesk.Revit.Attributes.Transaction(Autodesk.Revit.Attributes.TransactionMode.Manual)]
    class ConnectingConnectors : IExternalCommand
    {
        public Result Execute(ExternalCommandData commandData, ref string message, ElementSet elements)
        {
            Document doc = commandData.Application.ActiveUIDocument.Document;
            UIDocument uidoc = commandData.Application.ActiveUIDocument;

            //Получаем воздуховоды и воздухораспределители
            List<Duct> DuctList = new FilteredElementCollector(doc)
                .OfClass(typeof(Duct))
                .Cast<Duct>()
                .ToList();
            List<FamilyInstance> AirDiffusersList = new FilteredElementCollector(doc)
                .OfClass(typeof(FamilyInstance))
                .Cast<FamilyInstance>()
                .Where(i => i.Category.Name == "Воздухораспределители")
                .ToList();


			int resultN = 0;//для подсчета коннекторов
			//Соединяем коннекторы
			Transaction t = new Transaction(doc);
			t.Start("Соединение коннекторов");
			foreach (FamilyInstance element in AirDiffusersList)
			{
				foreach (Connector connector in element.MEPModel.ConnectorManager.Connectors)
				{
					if (!connector.IsConnected)
					{
						double Xcoordinate = connector.Origin.X;
						double Ycoordinate = connector.Origin.Y;
						double Zcoordinate = connector.Origin.Z;
						foreach (Duct duct in DuctList)
						{
							foreach (Connector _connector in duct.ConnectorManager.Connectors)
							{
								if (!_connector.IsConnected)
								{
									double ductConXcoordinate = _connector.Origin.X;
									double ductConYCoordinate = _connector.Origin.Y;
									double ductConZCoordinate = _connector.Origin.Z;
									if (Math.Abs(element.HandOrientation.Y) == 1
									   && Math.Abs(Xcoordinate - ductConXcoordinate) < 1
									   && Math.Round(ductConYCoordinate - connector.Origin.Y) == 0
									   && Math.Round(ductConZCoordinate - connector.Origin.Z) == 0)

									{

										connector.ConnectTo(_connector);
										_connector.Origin = connector.Origin;
										resultN++;


									}
									else if (Math.Abs(element.HandOrientation.X) == 1
											&& Math.Abs(Ycoordinate - ductConYCoordinate) < 1
											&& Math.Round(Xcoordinate - ductConXcoordinate) == 0
											&& Math.Round(ductConZCoordinate - connector.Origin.Z) == 0)
									{
										connector.ConnectTo(_connector);
										_connector.Origin = connector.Origin;
										resultN++;
									}
								}
							}
						}
					}
				}


			}
			t.Commit();
			TaskDialog.Show("результат", string.Format("{0} коннекторов соединены", resultN));


            return Result.Succeeded;
        }
    }
}
