using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Autodesk.Revit.DB;
using Autodesk.Revit.UI;
using Autodesk.Revit.UI.Selection;

namespace GreenBIM
{
    [Autodesk.Revit.Attributes.Transaction(Autodesk.Revit.Attributes.TransactionMode.Manual)]
    class GetInputElement : IExternalCommand
    {
        public Result Execute(ExternalCommandData commandData, ref string message, ElementSet elements)
        {
            Document doc = commandData.Application.ActiveUIDocument.Document;
            UIDocument uidoc = commandData.Application.ActiveUIDocument;

            //Получаем инстансы в модели
            FilteredElementCollector inModel = new FilteredElementCollector(doc).OfClass(typeof(FamilyInstance)).OfCategory(BuiltInCategory.OST_Rebar);
            List<FamilyInstance> listInModel = inModel.Cast<FamilyInstance>().ToList();

            //Создаем лист со вложенными
            List<FamilyInstance> inPutList = new List<FamilyInstance>();

            //Проверяем и добавлем элементы в список вложенных
            foreach (FamilyInstance i in listInModel)
            {
                if (isDependetElement(i))
                {
                    inPutList.Add(i);
                }
            }

            //Выбираем элементы в модели
            SelectElement(uidoc, inPutList);

            TaskDialog.Show("1", "Плагин завершил работу");
            return Result.Succeeded;
        }

        //Выборка элементов в модели
        private void SelectElement(UIDocument uidoc, List<FamilyInstance> list)
        {


            Selection sel = uidoc.Selection;
            List<ElementId> Ids = new List<ElementId>();
            foreach (FamilyInstance element in list)
            {
                Ids.Add(element.Id);
            }
            sel.SetElementIds(Ids);
        }

        //Обпределяем зависимые элементы
        private bool isDependetElement(FamilyInstance el)
        {

            if (el.SuperComponent == null && el.Symbol.Family.LookupParameter("Общий").AsInteger().ToString() == "1")
            {
                if (ChekName(el))
                {
                    return true;
                }
                return false;

            }
            else
            {
                return false;
            }
        }

        //Дополнительно проверяем элементы по имени
        private bool ChekName(FamilyInstance FI)
        {
            List<string> exceptions = new List<string>()
            {
                "261_Стержень вертикально (НесАрм_РабПлоск)",
                "261_Стержень П вертикально (НесАрм_РабПлоск)",
                "261_Хомут прямоугольный загиб 180 (НесАрм_РабПлоск)",
                "264_4 стержня вертикально (ОбщМод_2ур)"

            };
            if (int.Parse(FI.Symbol.Family.Name.ToString().Substring(0, 3)) > 349 || exceptions.IndexOf(FI.Symbol.Family.Name.ToString()) != -1)
            {

                return true;
            }
            else
            {
                return false;
            }

        }
    }
}
