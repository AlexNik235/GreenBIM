using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace GreenBIM.ServiceClass
{
    using Autodesk.Revit.UI;
    using Autodesk.Revit.DB;
    using Autodesk.Revit.UI.Selection;
    using Autodesk.Revit.Exceptions;

    public class SelectionClass
    {
        private readonly Selection _sel;
        private readonly Document _doc;
        private readonly UIDocument _uiDoc;

        public SelectionClass(UIDocument uidoc)
        {
            _uiDoc = uidoc;
            _doc = uidoc.Document;
            _sel = uidoc.Selection;
        }

        /// <summary>
        /// Выбираем элементы по классу
        /// </summary>
        /// <param name="obj"></param>
        public Element PickElementByClass<T>()
        {
            try
            {
                return _doc.GetElement(_sel.PickObject(ObjectType.Element, new PikByClass<T>()));
            }
            catch (OperationCanceledException)
            {
                return null;
            }
        }

        /// <summary>
        /// Выбор элемента по категории
        /// </summary>
        /// <param name="category"></param>
        /// <returns></returns>
        public Element PickElementByCategory(Category category)
        {
            try
            {
                return _doc.GetElement(_uiDoc.Selection.PickObject(ObjectType.Element, new PickByCattegory(category)));
            }
            catch (OperationCanceledException)
            {
                return null;
            }
        }
        public List<Element> PickElementsByClass<T>()
        {
            try
            {
                var new_list = _uiDoc.Selection.PickObjects(ObjectType.Element, new PikByClass<T>()).Select(i => _doc.GetElement(i)).ToList();
                return new_list;
            }
            catch (OperationCanceledException)
            {
                return null;
            }
        }


    }

    /// <summary>
    /// Выбирает элемент по категории
    /// </summary>
    class PickByCattegory : ISelectionFilter
    {
        private readonly Category _category;
        public PickByCattegory(Category category)
        {
            _category = category;
        }
        public bool AllowElement(Element elem)
        {
            if (elem.Category == _category)
                return true;
            else 
                return false;

        }

        public bool AllowReference(Reference reference, XYZ position)
        {
            return false;
        }
    }

    /// <summary>
    /// Получение элементов определенного класса
    /// </summary>
    class PikByClass<T> : ISelectionFilter
    {
        private readonly T _elementClass;
        public bool AllowElement(Element elem)
        {
            if (elem.GetType() == _elementClass.GetType()) return true;
            else return false;

        }

        public bool AllowReference(Reference reference, XYZ position)
        {
            return false;
        }
    }

}
