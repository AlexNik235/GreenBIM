using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Autodesk.Revit.UI;
using GalaSoft.MvvmLight;

namespace GreenBIM.AppServise.ViewModel
{
    public class ButtonModel
    {

        public string buttonName { get; set; }

        public string buttonText { get; set; }

        public string direction { get; set; }

        public string tabName { get; set; }

        public string image { get; set; }

        public string panelName { get; set; }


    }
}
