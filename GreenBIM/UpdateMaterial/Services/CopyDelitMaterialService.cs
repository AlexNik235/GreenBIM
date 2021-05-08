using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace GreenBIM.UpdateMaterial.Services
{
    using Autodesk.Revit.DB;
    using Autodesk.Revit.UI;
    using GreenBIM.ServiceClass;
    using GreenBIM.UpdateMaterial.Models;
    using System.Collections.ObjectModel;

    public class CopyDelitMaterialService
    {
        private Loger loger;
        public CopyDelitMaterialService()
        {
            loger = new Loger(ConstantClass.LogFilePath);
        }
        public void CopyMaterial(ObservableCollection<DocModel> DocumentList, ObservableCollection<MaterialModel> curentMaterialList)
        {
            CopyPasteOptions copyPasteOptions = new CopyPasteOptions();
            foreach (var docModel in DocumentList)
            {
                if(docModel.ChangeMaterial)
                {
                    using(Transaction t = new Transaction(docModel.doc, "Копирование материалов"))
                    {
                        t.Start();

                        foreach (var KeyValuePair in GetElementIdCollection(curentMaterialList))
                        {
                            try
                            {
                                if (KeyValuePair.Value.Count > 0)
                                {
                                    
                                    ElementTransformUtils.CopyElements(KeyValuePair.Key, KeyValuePair.Value, docModel.doc, Transform.Identity, copyPasteOptions);
                                    foreach (var el in KeyValuePair.Value)
                                    {
                                        loger.AddLog("Скопирован материал " + KeyValuePair.Key.GetElement(el).Name + " из файла " + KeyValuePair.Key.Title + " в " + docModel.doc.Title);
                                    }

                                }
                            }
                            catch
                            {
                                loger.AddLog("Не удалось скопировать материалы из документа ", KeyValuePair.Key.Title);
                            }

                        }

                        t.Commit();
                    }
                    

                }
            }
            loger.ShowTxtLog();
        }

        /// <summary>
        /// Отсортированный словарь с айдишниками
        /// </summary>
        /// <param name="curentMaterialList"></param>
        /// <returns></returns>
        private Dictionary<Document, ICollection<ElementId>> GetElementIdCollection(ObservableCollection<MaterialModel> curentMaterialList)
        {
            var dictMaterial = new Dictionary<Document, ICollection<ElementId>>();

            foreach(var el in curentMaterialList)
            {
                if(el.IsChoosen)
                {
                    if (!dictMaterial.ContainsKey(el.doc))
                    {
                        ICollection<ElementId> elementIdCollection = new Collection<ElementId>();
                        elementIdCollection.Add(el.material.Id);
                        dictMaterial.Add(el.doc, elementIdCollection);
                    }
                    else
                    {
                        dictMaterial[el.doc].Add(el.material.Id);
                    }
                }
                
            }
            return dictMaterial;
        }

        public void DelitMaterial(ObservableCollection<MaterialModel> curentMaterialList)
        {
            
            foreach(var keyValuePair in GetElementIdCollection(curentMaterialList))
            {
                using(Transaction t = new Transaction(keyValuePair.Key, "Удаление материалов"))                   
                {
                    t.Start();
                    foreach(var elId in keyValuePair.Value)
                    {
                        loger.AddLog("удален материал " + keyValuePair.Key.GetElement(elId).Name + " в файле " + keyValuePair.Key.Title);
                        keyValuePair.Key.Delete(elId);
                    }
                    t.Commit();

                }    
            }
            loger.ShowTxtLog();
        }
    }
}
