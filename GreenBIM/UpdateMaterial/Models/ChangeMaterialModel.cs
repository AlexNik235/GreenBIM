using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace GreenBIM.UpdateMaterial.Models
{
    using Autodesk.Revit.DB;
    public class ChangeMaterialModel
    {
        public FamilyType fmtype { get; set; }
        public FamilyParameter fmParam { get; set; }
        public Material old_material { get; set; }
        public string old_material_Name
        {
            get => old_material.Name;
        }
        public Material new_material { get; set; }
        public string new_material_Name
        {
            get => new_material.Name;
        }

        public Document doc { get; set; }

        public FamilyType familyType { get; set; }

        public string IdList { get; set; }
        public List<string> matchingList { get; set; } = new List<string>();

        public bool Ignoreble { get; set; }
    }
}
