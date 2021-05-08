using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Autodesk.Revit.DB;
using Autodesk.Revit.UI;
using Autodesk.Revit.UI.Selection;
using Autodesk.Revit.DB.Structure;

namespace GreenBIM.FamilySymbolTestPr
{
    class FindAndRun
    {
        Document doc;
        UIDocument uidoc;
        public FindAndRun(ExternalCommandData commandData)
        {
            doc = commandData.Application.ActiveUIDocument.Document;
            uidoc = commandData.Application.ActiveUIDocument;
            PlaceFamilySymbol();
        }
        private Element FindFamilyInstance()
        {
            Selection sel = uidoc.Selection;
            ElementId elid;

            if(sel.GetElementIds().Count() == 0 
                || !(doc.GetElement(sel.GetElementIds().First()).Category.Name == ("Окна") 
                || doc.GetElement(sel.GetElementIds().First()).Category.Name == ("Двери"))
                )
            {
                TaskDialog.Show("Предупреждение", "Не выбрано ни одного семейства");
                Reference reference = uidoc.Selection.PickObject(ObjectType.Element, new Selectors.DoorAndWindFilter());
                elid = reference.ElementId;
                return doc.GetElement(elid);
            }
            else
            {   elid = sel.GetElementIds().First(); 
                return doc.GetElement(elid);
            }
        }

        private Element GetWall()
        {
            Reference reference = uidoc.Selection.PickObject(ObjectType.Element, new Selectors.WallFilter());
            Element wall = doc.GetElement(reference.ElementId);
            return wall;
        }

        private void PlaceFamilySymbol()
        {
            //Получаем все необходимые элементы для установки семейств
            Element familyInstace = FindFamilyInstance();
            Family family = ((FamilyInstance)familyInstace).Symbol.Family;
            Element wall = GetWall();
            Level level = doc.GetElement(wall.LevelId) as Level;

            WallGeometry wallgeom = new WallGeometry(wall, doc); // Создаем экземпляр класса WallGeometry

            Transaction t = new Transaction(doc);
            t.Start("Установка семейств");
            int n = 1;
            foreach(ElementId elid in family.GetFamilySymbolIds())
            {
                FamilySymbol curSymbol = doc.GetElement(elid) as FamilySymbol;
                if(!curSymbol.IsActive)
                {
                    curSymbol.Activate();
                }
                if(((FamilyInstance)familyInstace).Symbol.Name != curSymbol.Name)
                {
                    XYZ newPoint = wallgeom.GetNextPoint((FamilyInstance)familyInstace, n);
                    doc.Create.NewFamilyInstance(newPoint, curSymbol, wall, level, StructuralType.NonStructural);
                    n++;
                }
            }
            t.Commit();
        }
    }
}
