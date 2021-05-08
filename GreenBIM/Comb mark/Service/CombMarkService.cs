using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace GreenBIM.Comb_mark.Service
{
    using Autodesk.Revit.UI;
    using Autodesk.Revit.DB;
    using GreenBIM.ServiceClass;
    using System.Runtime.CompilerServices;

    public class CombMarkService
    {
        private readonly UIDocument _uiDoс;
        private readonly Document _doc;
        private readonly View _activeView;
        private IndependentTag _mark;
        private readonly SelectionClass _selector;
        private XYZ midle;
        private XYZ head;
        private XYZ point;
        private Category elementCategory;
        /// <summary>
        /// Конструктор класса
        /// </summary>
        /// <param name="uidoc"></param>
        public CombMarkService(UIDocument uidoc)
        {
            _uiDoс = uidoc;
            _doc = uidoc.Document;
            _activeView = _doc.ActiveView;
            _selector = new SelectionClass(uidoc);
            Initialize();
        }

        private void Initialize()
        {
            _mark = (IndependentTag)_selector.PickElementByClass<IndependentTag>();
            if (_mark == null)
            {
                TaskDialog.Show("Ошибка", "Выбрана не марка");
            }
            else
            {
                elementCategory = _doc.GetElement(_mark.TaggedElementId.HostElementId).Category;
                midle = _mark.LeaderElbow;
                head = _mark.TagHeadPosition;
                point = _mark.LeaderEnd;
            }
        }

        public void CreateMark()
        {
            if(_mark != null)
            {
                var plane = Plane.CreateByNormalAndOrigin(_activeView.ViewDirection, _activeView.Origin); // Создаем пласкость
                midle = Project_Onto(plane, midle);
                head = Project_Onto(plane, head);
                point = Project_Onto(plane, point);

                var i = 0;
                while(i == 0)
                {
                    var target = (new SelectionClass(_uiDoс)).PickElementByCategory(elementCategory);
                    if(target != null)
                    {
                        var pos = get_coordinate();
                        var reference = new Reference(target);
                        using(Transaction t = new Transaction(_doc, "Добавить выноску"))
                        {
                            t.Start();
                            var vec = (point - midle).Normalize();
                            var n_pos = Project_Onto(plane, vec);
                            var l1 = Line.CreateUnbound(n_pos, vec);
                            var l2 = Line.CreateUnbound(midle, (head - midle).Normalize());
                            var inter1 = new StrongBox<IntersectionResultArray>();
                            var inter = new IntersectionResultArray();
                            l1.Intersect(l2, out inter);
                            //inter1.
                            //var new_midle = inter.Value

                        }
                    }
                }

            }
        }

        /// <summary>
        /// Какие то хитрые манипуляции с точками
        /// </summary>
        /// <param name="plane">Плоскость</param>
        /// <param name="point">точка</param>
        /// <returns></returns>
        private XYZ Project_Onto(Plane plane, XYZ point)
        {
            var d = SignedDistanceTo(plane, point);
            var q = point - d * plane.Normal;
            return q;
        }

        /// <summary>
        /// Вычисление расстояние от плоскости до точки
        /// </summary>
        /// <param name="plane">Плоскость</param>
        /// <param name="point">Точка</param>
        private double SignedDistanceTo(Plane plane, XYZ point)
        {
            var new_point = point - plane.Origin;
            return plane.Normal.DotProduct(new_point); // Возвращаем скалярное произведение вектора
        }


        /// <summary>
        /// Возвращает вертикальный вектор текущего вида
        /// </summary>
        /// <param name="view"></param>
        /// <returns></returns>
        private XYZ vert_cur_view(View view)
        {
            var v_right = _activeView.RightDirection; // Направление правого бока экрана
            v_right = new XYZ(Math.Abs(v_right.X), Math.Abs(v_right.Y), Math.Abs(v_right.Z)); // Получаем новую точку из правого направления экрана
            var v_up = _activeView.UpDirection; // Направление верха экрана
            v_up = new XYZ(Math.Abs(v_up.X), Math.Abs(v_up.Y), Math.Abs(v_up.Z)); // Получаем точку с абсолютными величинами из верхнего вектора экрана
            return (v_right + v_up);
        }

        /// <summary>
        /// Возвращает текущее активное окно
        /// </summary>
        /// <param name="uidoc"></param>
        /// <returns></returns>
        private UIView get_active_ui_view(UIDocument uidoc)
        {
            var activeView = uidoc.Document.ActiveView;
            var uiViews = uidoc.GetOpenUIViews(); // Возвращает все открытые виды 
            UIView uiview = null;
            foreach(var view in uiViews)
            {
                if(view.ViewId.Equals(activeView.Id))
                {
                    uiview = view;
                    break;
                }
            }
            return uiview;
        }

        /// <summary>
        /// ФИг знает что делает эта функция
        /// </summary>
        /// <returns></returns>
        private XYZ get_coordinate()
        {
            var uiView = get_active_ui_view(_uiDoс); // Получаем текущий активный вид
            var rect = uiView.GetWindowRectangle(); // Получаем прямоугольник видового экрана
            var p = System.Windows.Forms.Cursor.Position; // Получаем позицию курсора
            var dx = (double)(p.X - rect.Left) / (double)(rect.Right - rect.Left);
            var dy = (double)(p.Y - rect.Bottom) / (double)(rect.Top - rect.Bottom);
            var v_right = _activeView.RightDirection;
            v_right = new XYZ(Math.Abs(v_right.X), Math.Abs(v_right.Y), Math.Abs(v_right.Z));
            var v_up = _activeView.UpDirection;
            v_up = new XYZ(Math.Abs(v_up.X), Math.Abs(v_up.Y), Math.Abs(v_up.Z));
            var dxyz = dx * v_right + dy * v_up;

            var corners = uiView.GetZoomCorners();
            var a = corners[0];
            var b = corners[1];
            var v = b - a;
            var q = a + dxyz.X * v.X * XYZ.BasisX + dxyz.Y * v.Y * XYZ.BasisY + dxyz.Z * XYZ.BasisZ * v.Z;
            return q;

        }
    }
}
