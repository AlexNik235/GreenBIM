using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace GreenBIM.UpdateRebarForm.Models
{
    public class UserSettings
    {
        /// <summary>
        /// Раскрытие групп
        /// </summary>
        public bool OpenGruops { get; set; }

        /// <summary>
        /// Склейка групп
        /// </summary>
        public bool CloseGruops { get; set; }

        /// <summary>
        /// Исправление форм арматурных стержней
        /// </summary>
        public bool ChangeRebarForm { get; set; } = true;
    }
}
