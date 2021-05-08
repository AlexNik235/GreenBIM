using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Autodesk.Revit.DB;
using Autodesk.Revit.UI;

namespace GreenBIM
{
    class ClassHelper
    {
        public void PrintListElement(List<Element> elementList)
        {
            string resultString = "";

            foreach(Element el in elementList)
            {
                resultString += el.Name + "\n";
            }
            TaskDialog.Show("Element list", resultString);
        }
    }
}
