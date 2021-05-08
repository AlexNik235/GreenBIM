using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.IO;

namespace GreenBIM.UpdateMaterial.Services
{
    using Autodesk.Revit.DB;
    using Autodesk.Revit.UI;
    using GreenBIM.ServiceClass;
    using GreenBIM.UpdateMaterial.Models;
    using System.Collections.ObjectModel;

    public class UpdateMaterialService
    {
        private Loger loger { get; }
        public UpdateMaterialService()
        {
            loger = new Loger(ConstantClass.LogFilePath);
        }
        /// <summary>
        /// Получаем словарь типов в которых имеются сам параметр и материал
        /// </summary>
        private Dictionary<FamilyType, Dictionary<FamilyParameter, Material>> GetParamValueDict(Document doc)
        {
            var DictTypes = new Dictionary<FamilyType, Dictionary<FamilyParameter, Material>>();
            var DistParams = new Dictionary<FamilyParameter, Material>();

            FamilyManager FM = doc.FamilyManager;
            FamilyTypeSet familyTypes = FM.Types;

            var familyParameters = FM.Parameters;

            foreach (FamilyType type in familyTypes)
            {
                var new_dict = new Dictionary<FamilyParameter, Material>();
                DictTypes.Add(type, new_dict);
                foreach(FamilyParameter param in familyParameters)
                {
                    if(param.Definition.ParameterType == ParameterType.Material)
                    {
                        //////////////////Обработать исключения если материала нет
                        ///
                        ElementId elid = type.AsElementId(param);
                        if(elid != ElementId.InvalidElementId)
                        {
                            Material material = (Material)doc.GetElement(elid);
                            new_dict.Add(param, material);
                        }
                        
                    }                    
                }
                
            }

            return DictTypes;


        }

        /// <summary>
        /// Получаем список моделей
        /// </summary>
        /// <param name="doc"></param>
        /// <returns></returns>
        private List<ChangeMaterialModel> CreateModellList(Document doc)
        {
            var resultList = new List<ChangeMaterialModel>();

            foreach(var keyValuePair in GetParamValueDict(doc))
            {
                foreach(var keyValuePair1 in keyValuePair.Value)
                {
                    resultList.Add(new ChangeMaterialModel()
                    {
                        doc = doc,
                        familyType = keyValuePair.Key,
                        fmParam = keyValuePair1.Key,
                        old_material = keyValuePair1.Value
                    });
                }
            }

            return resultList;
        }

        /// <summary>
        /// Проверка наличия материала в файле сопоставления
        /// </summary>
        /// <param name="materialModel"></param>
        /// <param name="matchingList"></param>
        /// <returns></returns>
        private bool CheckMaterialChangeFile(ChangeMaterialModel materialModel,  Dictionary<string, List<string>> matchingList)
        {
            foreach(var keyValuePair in matchingList)
            {
                if(materialModel.old_material != null)
                {
                    if (keyValuePair.Value.Contains(materialModel.old_material_Name))
                    {
                        materialModel.IdList = keyValuePair.Key;
                        materialModel.matchingList.AddRange(keyValuePair.Value);
                        if(materialModel.matchingList.Contains("НЕ УДАЛЯТЬ"))
                        {
                            materialModel.Ignoreble = true;
                        }
                        else
                        {
                            materialModel.Ignoreble = false;
                        }
                        return true;
                    }
                }
                
            }

            return false;
        }

        private bool CheckMaterialInResurseFile(Document doc, ChangeMaterialModel materialModel)
        {
            var materials = new FilteredElementCollector(doc).OfClass(typeof(Material)).Cast<Material>().ToList();
            foreach(var material in materials)
            {
                if(materialModel.matchingList != null)
                {
                    if (materialModel.matchingList.Contains(material.Name))
                    {
                        materialModel.new_material = material;
                        return true;
                    }
                }              
            }
            return false;
        }

        public void CheckMaterialInFiles(ObservableCollection<DocModel> DocumentList, Dictionary<string, List<string>> matchingList)
        {
            Document mainFile = DocumentList.Where(i => i.IsMaterialFile).ToList().First().doc;

            ICollection<ElementId> listToCopy = new Collection<ElementId>();

            // Проходимся по выбранным файлам в которых активно свойство изменить материал
            foreach(DocModel changeFile in DocumentList.Where(i=>i.ChangeMaterial).ToList())
            {
                // Создаем лист моделей с материалами в документах в которых нужно изменить материалы
                var ListMaterialModel = CreateModellList(changeFile.doc);

                // Проходимся по списку с моделями материалов
                foreach(var materialModel in ListMaterialModel)
                {
                    // Проверяем наличие материала в файле сопоставления
                    if(CheckMaterialChangeFile(materialModel, matchingList))
                    {

                    }
                    else
                    {
                        loger.AddLog(changeFile.doc, materialModel.old_material);
                    }

                    // Проверяем материал в файле из которого будем скачивать материал
                    if(CheckMaterialInResurseFile(mainFile, materialModel))
                    {
                        // Записываем материал в материал модели в случае удачного сопоставления
                        materialModel.new_material = new FilteredElementCollector(mainFile)
                            .WhereElementIsNotElementType()
                            .OfClass(typeof(Material))
                            .Cast<Material>()
                            .Where(i => materialModel.matchingList.Contains(i.Name))
                            .ToList()
                            .First();

                        listToCopy.Add(materialModel.new_material.Id);
                    }
                    else
                    {
                        loger.AddLog(mainFile, materialModel.old_material);
                    }
                }


                using(Transaction t = new Transaction(changeFile.doc, "Обновление материалов"))
                {

                    t.Start();
                    
                    CopyProjectStandart(changeFile.doc, mainFile, listToCopy);
                    GetNewMaterialAfterCopy(changeFile.doc, ListMaterialModel);
                    SetNewValueParam(changeFile.doc, ListMaterialModel);
                    DelitOldMaterial(changeFile.doc, ListMaterialModel);
                    t.Commit();
                }    
               
                loger.ShowTxtLog();
            }
        }

        private void SetNewValueParam(Document doc, List<ChangeMaterialModel> listModel )
        {
            FamilyManager FM = doc.FamilyManager;
            foreach (var model in listModel)
            {
                if (model.new_material != null)
                {
                    try
                    {
                        FM.CurrentType = model.familyType;
                        FM.Set(model.fmParam, model.new_material.Id);
                    }
                    catch
                    {

                    }
                }
            }           
        }
        /// <summary>
        /// Получить материалы после копирования
        /// </summary>
        /// <param name="doc"></param>
        /// <param name="listModel"></param>
        private void GetNewMaterialAfterCopy(Document doc, List<ChangeMaterialModel> listModel)
        {
            var materiaLlist = new FilteredElementCollector(doc)
                .WhereElementIsNotElementType()
                .OfClass(typeof(Material))
                .Cast<Material>()
                .ToList();

            foreach(Material material in materiaLlist)
            {
                foreach(var model in listModel)
                {
                    if(model.matchingList != null)
                    {
                        if (model.matchingList.Contains(material.Name))
                        {
                            model.new_material = material;
                        }
                    }  
                }
            }
        }

        /// <summary>
        /// Удалить старые найденные материалы
        /// </summary>
        /// <param name="doc"></param>
        /// <param name="listModel"></param>
        private void DelitOldMaterial(Document doc, List<ChangeMaterialModel> listModel)
        {
            foreach (var model in listModel)
            {
                if(!model.Ignoreble)
                {
                    try
                    {
                        doc.Delete(model.old_material.Id);
                    }
                    catch
                    {
                        //loger.AddLog(doc, model.old_material, "При удалении материала возникла ошибка");
                    }
                }                                    
            } 
        }

        /// <summary>
        /// Копируем материалы из файла ресурса
        /// </summary>
        /// <param name="Doc"></param>
        /// <param name="ResurseDoc"></param>
        /// <param name="elementIds"></param>
        private void CopyProjectStandart(Document Doc, Document ResurseDoc, ICollection<ElementId> elementIds)
        {
            
            CopyPasteOptions copyPasteOptions = new CopyPasteOptions();
            if(elementIds.Count>0)
            {
                ElementTransformUtils.CopyElements(ResurseDoc, elementIds, Doc, Transform.Identity, copyPasteOptions);
            }                  
            
        }

    }
    
}
