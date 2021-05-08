using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Autodesk.Revit.DB;
using Autodesk.Revit.UI;

namespace GreenBIM
{
    [Autodesk.Revit.Attributes.Transaction(Autodesk.Revit.Attributes.TransactionMode.Manual)]
    class RenameViews : IExternalCommand
    {

        public Result Execute(ExternalCommandData commandData, ref string message, ElementSet elements)
        {
			Document doc = commandData.Application.ActiveUIDocument.Document;
			//Создает форму
			RenameViewsForm form1 = new RenameViewsForm();
			form1.ShowDialog();

			//Проверяем DialogResult
			if (form1.DialogResult != System.Windows.Forms.DialogResult.OK)
            {
				return Result.Cancelled;
            } 

			string oldName = form1.oldName;
			string newName = form1.newName;

			List<Element> ViewSheetsList = new FilteredElementCollector(doc).OfClass(typeof(ViewSheet)).ToList();
			List<Element> ViewScheduleList = new FilteredElementCollector(doc).OfClass(typeof(ViewSchedule)).ToList();


			Transaction t = new Transaction(doc);
			t.Start("begin");
			foreach (Element element in ViewSheetsList)
			{
				foreach (Element el in GetDependentViews(doc, element))
				{
					if (isNeedName(el, oldName))
					{
						el.Name = el.Name.Replace(oldName, newName);
					}
				}

				if (isNeedName(element, oldName))
				{
					element.Name = element.Name.Replace(oldName, newName);
				}

			}
			foreach (Element element in ViewScheduleList)
			{
				if (isNeedName(element, oldName))
				{
					element.Name = element.Name.Replace(oldName, newName);
				}
			}

			t.Commit();
			TaskDialog.Show("Отчет", "Работа плагина завершена");



			return Result.Succeeded;
        }

		private bool isNeedName(Element el, string str)
		{
			if (el.Name.Contains(str))
			{
				return true;
			}
			else
			{
				return false;
			}
		}

		private List<Element> GetDependentViews(Document doc,Element el)
		{


			ElementClassFilter filter1 = new ElementClassFilter(typeof(Viewport), false);

			List<Element> views = new List<Element>();

			foreach (ElementId elementid in el.GetDependentElements(filter1))
			{
				Viewport view = doc.GetElement(elementid) as Viewport;
				if (!doc.GetElement(view.ViewId).Name.Contains("Ведомость"))
				{
					if (doc.GetElement(view.ViewId).Name != el.Name)
					{
						views.Add(doc.GetElement(view.ViewId));

					}

				}

			}

			return views;
		}
	}
}
