using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Autodesk.Revit.DB;
using Autodesk.Revit.DB.Architecture;

namespace GreenBIM.FinishSheet.Service
{
    public class GetGeometry
    {
        private Element _el;
        private Options _opt = new Options();
        public List<Solid> Solids = new List<Solid>();
        public List<XYZ> Points = new List<XYZ>();
        public List<Line> Lines = new List<Line>();

        public GetGeometry(dynamic el)
        {
            _el = (Element)el;
            GetGeom();
        }
        private void GetGeom()
        {
            _opt.DetailLevel = ViewDetailLevel.Medium;
            var geom = _el.get_Geometry(_opt);
            foreach(var element in geom)
            {
                if(element is Solid)
                {
                    Solids.Add((Solid)element);
                }
                //    else if(element is Line)
                //    {
                //        Lines.Add((Line)element);
                //        for(int i = 0; i<2; i++)
                //        {
                //            if(!Points.Contains((element as Line).GetEndPoint(i)))
                //            {
                //                Points.Add((element as Line).GetEndPoint(i));
                //            }
                //        }
                //    }
            }


        }

    }
}
