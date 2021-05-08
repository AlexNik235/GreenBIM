using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Autodesk.Revit.DB;
using Autodesk.Revit.UI.Selection;
using Autodesk.Revit.UI;

namespace GreenBIM.FinishSheet.Helper
{
    public class Selector
    {
        private UIDocument _uidoc;
        //private List<Element> _listElement;
        public Selector(UIDocument uidoc)
        {
            _uidoc = uidoc;
            //_listElement = listElement;
        }
        /// <summary>
        /// Выбирает список элементов на виде
        /// </summary>
        /// <param name="_listElement"></param>
        public void SelectEleventOnView(List<Element> _listElement)
        {
            Selection sel = _uidoc.Selection;
            List < ElementId > listElId= new List<ElementId>();
            if(_listElement.Count()>0)
            {
                foreach (var element in _listElement)
                {
                    listElId.Add(element.Id);
                }
                sel.SetElementIds(listElId);
            }
            else
            {
                TaskDialog.Show("1", "в списке нет элементов");
            }
            
        }
    }
}
