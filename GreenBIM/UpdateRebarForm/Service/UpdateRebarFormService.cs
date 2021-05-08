using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace GreenBIM.UpdateRebarForm.Service
{
    using Autodesk.Revit.DB;
    using Autodesk.Revit.UI;
    using Autodesk.Revit.UI.Selection;
    using Autodesk.Revit.DB.Structure;
    using GreenBIM.UpdateRebarForm.Models;

    public class UpdateRebarFormService
    {
        private readonly Document _doc;
        private readonly UIDocument _uiApp;
        private readonly List<Rebar> _duplicateRebar;
        private readonly char DivideChar = '*';
        public UpdateRebarFormService(UIApplication uiApp)
        {
            _uiApp = uiApp.ActiveUIDocument;
            _doc = uiApp.ActiveUIDocument.Document;
            _duplicateRebar = GetDublicatRebar();
            Validation();
        }

        private void Validation()
        {
            if (_duplicateRebar.Count == 0)
            {
                TaskDialog.Show("Предупреждение", "Не найдено диблирующих элементов");
                return;
            }
        }
        /// <summary>
        /// Получаем все арматурные дублируемые стержни
        /// </summary>
        /// <returns></returns>
        private List<Rebar> GetDublicatRebar()
        {
            int n = 0;
            var rebarList = new FilteredElementCollector(_doc)
                .OfClass(typeof(Rebar))
                .Cast<Rebar>()
                .Where(i => int.TryParse(i.LookupParameter("Форма").AsValueString().Split(' ').Last(), out n) && i.LookupParameter("Форма").AsValueString().Split(' ').Count()>1)
                .ToList();

            return rebarList;
        }

        /// <summary>
        /// Получаем группы с дублирующимися стержнями
        /// </summary>
        /// <returns></returns>
        private List<Group> GetGruipWithDuplicateRebar()
        {
            var resultList = new List<Group>();

            foreach(var el in _duplicateRebar)
            {
                if(!resultList.Contains((Group)_doc.GetElement(el.GroupId)))
                {
                    resultList.Add((Group)_doc.GetElement(el.GroupId));
                }
            }
            //var groupList = new FilteredElementCollector(_doc)
            //    .OfClass(typeof(Group))
            //    .Cast<Group>()
            //    .ToList();

            //foreach (var group in groupList)
            //{
            //    foreach (var rebar in _duplicateRebar)
            //    {
            //        if (group.GetMemberIds().Contains(rebar.Id) && !resultList.Contains(group))
            //        {
            //            resultList.Add(group);
            //        }
            //    }
            //}

            return resultList;
        }

        /// <summary>
        /// Раскрываем группы
        /// </summary>
        /// <param name="GroupList"></param>
        private void OpenGroup(List<Group> GroupList)
        {
            var dictList = new Dictionary<string, List<Group>>();

            //Группируем экземпляры групп
            foreach (Group group in GroupList)
            {

                if (!dictList.ContainsKey(group.Name))
                {
                    var new_list = new List<Group>();
                    new_list.Add(group);
                    dictList.Add(group.Name, new_list);
                }
                else
                {
                    dictList[group.Name].Add(group);
                }
            }

            //Переименовываем экземпляры групп
            foreach (var keyValuePair in dictList)
            {
                for (int i = 1; i < keyValuePair.Value.Count; i++)
                {
                    string new_name = keyValuePair.Value[i].Name + DivideChar + ((double)i / 100).ToString();
                    var ElementIdCollection = keyValuePair.Value[i].UngroupMembers();
                    _doc.Delete(keyValuePair.Value[i].Id); //Удаление дубликата из памяти
                    Group new_gruop = _doc.Create.NewGroup(ElementIdCollection);
                    _doc.GetElement(new_gruop.GetTypeId()).Name = new_name;
                }
            }


        }

        /// <summary>
        /// Устанавливаем дубликатам значение истиной формы
        /// </summary>
        private void SetRebarTrueForm()
        {
            foreach(Rebar rebar in _duplicateRebar)
            {
                rebar.LookupParameter("Форма").Set(GetTrueForm(rebar));
            }
        }

        /// <summary>
        /// Определяем оригинальный типФормы
        /// </summary>
        /// <param name="rebar"></param>
        /// <returns></returns>
        private ElementId GetTrueForm(Rebar rebar)
        {
            int LenthString = rebar.LookupParameter("Форма").AsValueString().Split(' ').Length;
            string new_string = "";
            var DuplicateNumber = rebar.LookupParameter("Форма").AsValueString().Split(' ').ToList().GetRange(0, LenthString-1);
            foreach(var el in DuplicateNumber)
            {
                new_string += el + " ";
            }
            new_string = new_string.Trim();
            ElementId rebarShapeId = new FilteredElementCollector(_doc)
                .OfClass(typeof(RebarShape))
                .Cast<RebarShape>()
                .Where(i => i.Name == new_string)
                .ToList()
                .First()
                .Id;

            return rebarShapeId;
        }

        private void CloseGroup(List<Group> GroupList)
        {
            var dictList = new Dictionary<string, List<Group>>();

            ///группируем группы
            foreach (Group group in GroupList)
            {

                if (!dictList.ContainsKey(group.Name) && !group.Name.Contains(DivideChar))
                {
                    var new_list = new List<Group>();
                    new_list.Add(group);
                    dictList.Add(group.Name, new_list);
                }
                else if (group.Name.Contains(DivideChar) && dictList.ContainsKey(group.Name.Split(DivideChar)[0]))
                {
                    var new_list = new List<Group>();
                    new_list.Add(group);
                    //string new_name = 
                    dictList[group.Name.Split(DivideChar)[0]].Add(group);
                }
                else
                {
                    dictList[group.Name].Add(group);
                }


            }

            foreach (var keyvaluePair in dictList)
            {
                if (CheckOpneGruop(keyvaluePair.Value))
                {
                    for (int i = 1; i < keyvaluePair.Value.Count; i++)
                    {

                        var parantType = (GroupType)_doc.GetElement(keyvaluePair.Value[0].GetTypeId());
                        var oldType = keyvaluePair.Value[i].GetTypeId();
                        ((Group)keyvaluePair.Value[i]).GroupType = parantType;
                        try
                        {
                            _doc.Delete(oldType);
                        }
                        catch
                        {

                        }

                    }
                }

            }
        }
        /// <summary>
        /// Проверка на то, была ли группа открытой
        /// </summary>
        /// <param name="listgruop"></param>
        /// <returns></returns>
        private bool CheckOpneGruop(List<Group> listgruop)
        {
            foreach (var element in listgruop)
            {
                if (element.Name.Contains('*'))
                {
                    return true;
                }
            }
            return false;
        }

        /// <summary>
        /// Выполнение команды
        /// </summary>
        /// <param name="userSetting"></param>
        public void ServiceCommand(UserSettings userSetting)
        {
            
            if(userSetting.ChangeRebarForm)
            {
                using(Transaction t = new Transaction(_doc, "Избавление от дубликатов системных форм"))
                {
                    t.Start();

                    OpenGroup(GetGruipWithDuplicateRebar());
                    
                    var gropList = new FilteredElementCollector(_doc).OfClass(typeof(Group)).Cast<Group>().Where(i=>i.IsValidObject).ToList();
                    SetRebarTrueForm();
                    CloseGroup(gropList);

                    t.Commit();
                }
                
            }
            else if(userSetting.OpenGruops)
            {
                using(Transaction t = new Transaction(_doc, "Открытие групп"))
                {
                    t.Start();

                    var GroupIdList = _uiApp.Selection.GetElementIds().ToList();
                    if (GroupIdList.Count > 0)
                    {
                        var GropList = GroupIdList.Select(i => (Group)_doc.GetElement(i)).ToList();
                        OpenGroup(GropList);
                    }
                    else
                    {
                        var gropList = new FilteredElementCollector(_doc)
                            .WhereElementIsNotElementType()
                            .OfClass(typeof(Group))
                            .Cast<Group>()
                            .ToList();
                        OpenGroup(gropList);
                    }

                    t.Commit();
                }
                
                
            }
            else if(userSetting.CloseGruops)
            {

                using (Transaction t = new Transaction(_doc, "Открытие групп"))
                {
                    t.Start();

                    var GroupIdList = _uiApp.Selection.GetElementIds().ToList();
                    if (GroupIdList.Count > 0)
                    {
                        var GropList = GroupIdList.Select(i => (Group)_doc.GetElement(i)).ToList();
                        CloseGroup(GropList);
                    }
                    else
                    {
                        var gropList = new FilteredElementCollector(_doc)
                            .WhereElementIsNotElementType()
                            .OfClass(typeof(Group))
                            .Cast<Group>()
                            .ToList();
                        CloseGroup(gropList);
                    }

                    t.Commit();
                }
                
            }
        }

    }
}
