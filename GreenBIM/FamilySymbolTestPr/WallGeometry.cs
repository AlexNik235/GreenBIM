using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Autodesk.Revit.DB;
using Autodesk.Revit.UI;
using Autodesk.Revit.UI.Selection;

namespace GreenBIM.FamilySymbolTestPr
{
    class WallGeometry
    {
        private Element wall;
        private Line WallLine;
        private XYZ StartPoint;
        private XYZ EndPoint;
        private Document doc;

        public WallGeometry(Element el, Document document)
        {
            wall = el;
            WallLine = GetLine();
            StartPoint = WallLine.GetEndPoint(0);
            EndPoint = WallLine.GetEndPoint(1);
            doc = document;
        }
        private Line GetLine()
        {
            Line line = ((LocationCurve)wall.Location).Curve as Line;
            return line;
        }
        public XYZ GetNextPoint(FamilyInstance fi, int n)
        {
            XYZ statPoint = ((LocationPoint)fi.Location).Point;
            Line newLine = Line.CreateBound(statPoint, EndPoint);
            double paramForDivide = GetLengthFamily(fi, doc);
            XYZ newPoint = newLine.Evaluate(paramForDivide * n, false);
            return newPoint;
        }
        private double GetLengthFamily(FamilyInstance fi, Document doc)
        {
            View view = doc.ActiveView;
            XYZ MaxPoint = fi.get_BoundingBox(view).Max;
            XYZ MinPoint = fi.get_BoundingBox(view).Min;
            double result = Math.Sqrt(Math.Pow(MaxPoint.X - MinPoint.X, 2) + Math.Pow(MaxPoint.Y - MinPoint.Y,2));
            return result;
        }

    }
}
