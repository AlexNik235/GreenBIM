using Autodesk.Revit.DB;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace GreenBIM.SketchRebar.Models
{
    public class RowModel
    {
        /// <summary>
        /// Координата строки
        /// </summary>
        public XYZ coordinate { get; set; }

        /// <summary>
        /// Номер колонки
        /// </summary>
        public int NumberOfColumn { get; set; }

        /// <summary>
        /// Номер строки
        /// </summary>
        public int NumberOfRow { get; set; }

        /// <summary>
        /// Список параметров
        /// </summary>
        public List<ScheduleFieldModel> SheduleFieldList { get; set; }

        /// <summary>
        /// Значение параметра Арм.НомерФормы
        /// </summary>
        public string ArmNumberOfForm { get; set; }

        /// <summary>
        /// Значение параметра Арм.НомерПодформы
        /// </summary>
        public string ArmNumberOfUnderForm { get; set; }

        /// <summary>
        /// Получаем семейство
        /// </summary>
        public Element family { get; set; }

        ///// <summary>
        ///// Значение параметра Арм_А
        ///// </summary>
        //public double Arm_A { get; set; }

        ///// <summary>
        ///// Значение параметра Арм_Б
        ///// </summary>
        //public double Arm_B { get; set; }

        ///// <summary>
        ///// Значение параметра Арм_В
        ///// </summary>
        //public double Arm_V { get; set; }

        ///// <summary>
        ///// Значение параметра Арм_Г
        ///// </summary>
        //public double Arm_G { get; set; } 

        ///// <summary>
        ///// Значение параметра Арм_Д
        ///// </summary>
        //public double Arm_D { get; set; }

        ///// <summary>
        ///// Значение параметра Арм_Е
        ///// </summary>
        //public double Arm_E { get; set; }

        ///// <summary>
        ///// Значение параметра Арм_Ж
        ///// </summary>
        //public double Arm_J { get; set; }

        ///// <summary>
        ///// Значение параметра Арм_И
        ///// </summary>
        //public double Arm_I { get; set; }

        ///// <summary>
        ///// Значение параметра Арм_К
        ///// </summary>
        //public double Arm_K { get; set; }

        ///// <summary>
        ///// Значение параметра Арм_Л
        ///// </summary>
        //public double Arm_L { get; set; }

        ///// <summary>
        ///// Значение параметра Арм_М
        ///// </summary>
        //public double Arm_M { get; set; }

        ///// <summary>
        ///// Значение параметра Арм_Н
        ///// </summary>
        //public double Arm_N { get; set; }

        ///// <summary>
        ///// Значение параметра Арм_О
        ///// </summary>
        //public double Arm_O { get; set; }

        ///// <summary>
        ///// Значение параметра Арм_Н1
        ///// </summary>
        //public double Arm_H1 { get; set; }

        ///// <summary>
        ///// Значение параметра Арм_Н2
        ///// </summary>
        //public double Arm_H2 { get; set; }

        ///// <summary>
        ///// Значение параметра Арм_R
        ///// </summary>
        //public double Arm_R { get; set; }
    }
}
