using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace GreenBIM.ServiceClass
{
    using Autodesk.Revit.DB;
    using Autodesk.Revit.UI;
    using System.IO;

    public class Loger
    {
        private string log_string { get; set; } = "";
        private string _filepath;

        public Loger()
        {

        }
        public Loger(string filePath)
        {
            _filepath = filePath;
        }

        /// <summary>
        /// Добавить лог материала и документа
        /// </summary>
        /// <param name="doc"></param>
        /// <param name="material"></param>
        public void AddLog(Document doc, Material material)
        {
            log_string += $"В файле сопоставления нет следующего материала |{material.Name}|{doc.Title}" + "\n";

        }

        /// <summary>
        /// Записать лог в Txt файл
        /// </summary>
        /// <returns></returns>
        public async Task ShowTxtLog()
        {
            using (StreamWriter file = new StreamWriter(_filepath, false, System.Text.Encoding.Default))
            {
                await file.WriteAsync(log_string);
            }

        }
        /// <summary>
        /// Показать результат лога
        /// </summary>
        public void ShowLog()
        {
            TaskDialog.Show("Отчет", log_string);
        }

        /// <summary>
        /// Добавить лог с материалом и документом
        /// </summary>
        /// <param name="doc"></param>
        /// <param name="material"></param>
        /// <param name="message"></param>
        public void AddLog(Document doc, Material material, string message)
        {
            log_string += $"{message} |{material.Name}|{doc.Title}" + "\n";
        }

        /// <summary>
        /// Добавить обычный лог сообщение
        /// </summary>
        /// <param name="message"></param>
        public void AddLog(string message)
        {
            log_string += message + "\n";
        }

        /// <summary>
        /// Добавить в лог строковый массив
        /// </summary>
        /// <param name="list"></param>
        public void AddLog(params string[] list)
        {
            foreach(var el in list)
            {
                log_string += el + " ";
            }
            log_string += "\n";
        }
    }
}
