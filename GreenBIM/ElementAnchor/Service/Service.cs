using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Autodesk.Revit.DB;

namespace GreenBIM.ElementAnchor.Service
{
    class Service
    {
        private readonly Document _doc;

        public Service(Document doc)
        {
            _doc = doc;
        }
        public void AnchorElementInModel()
        {
            using(Transaction t = new Transaction(_doc, "Закрепление элементов"))
            {
                t.Start();
                var ListElement = GetElementListInModel();
                foreach (var el in ListElement)
                {
                    SetPinned(el);
                }
                t.Commit();
            }
            

        }
        public void AnchorElementInSheet()
        {
            using(Transaction t = new Transaction(_doc, "Закрепление элементов"))
            {
                t.Start();
                var SheetList = new FilteredElementCollector(_doc).OfClass(typeof(ViewSheet)).ToList();
                foreach (var sheet in SheetList)
                {
                    foreach (var el in GetElementListInSheet((ViewSheet)sheet))
                    {
                        SetPinned(el);
                    }
                }
                t.Commit();
            }
            
        }
        


        private List<Element> GetElementListInModel()
        {
            var MultiClassFilter = new ElementMulticlassFilter(new List<Type>
            {
                typeof(TextNote),
                typeof(FilledRegion),
                typeof(Dimension),
                typeof(FamilyInstance),
                typeof(Wall),
                typeof(Floor),
                typeof(RevitLinkInstance),
                typeof(IndependentTag),
                typeof(Group)
            }
                );
            List<Element> ListElement = new FilteredElementCollector(_doc).WherePasses(MultiClassFilter).ToList();
            return ListElement;
        }
        private List<Element> GetElementListInSheet(ViewSheet _viewsheet)
        {
            List<Element> listElement = new List<Element>();  
            foreach(var elid in _viewsheet.GetDependentElements(null).ToList())
            {
                listElement.Add(_doc.GetElement(elid));
            }
            return listElement;
        }
        private void SetPinned(Element el)
        {
            if(!el.Pinned)
            {
                el.Pinned = true;
            }
        }
    }
}
