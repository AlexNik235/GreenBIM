using GreenBIM.ServiceClass;
using GreenBIM.UpdateMaterial.ViewModels;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Navigation;
using System.Windows.Shapes;

namespace GreenBIM.UpdateMaterial.View
{

    using Autodesk.Revit.DB;
    using Autodesk.Revit.UI;
    /// <summary>
    /// Логика взаимодействия для SelecteWindow.xaml
    /// </summary>
    public partial class SelectWindow : Window, IClosable
    {
        public SelectWindow()
        {
            InitializeComponent();  
            
        }



        private void updateMaterialList_Click(object sender, RoutedEventArgs e)
        {
            CheckBox radioButton = (CheckBox)sender;
            (this.DataContext as MainContext).updateMaterialList();
            
        }

        private void UpdateDocList_Click(object sender, RoutedEventArgs e)
        {
            RadioButton radioButton = (RadioButton)sender;
            (this.DataContext as MainContext).updateDocumentlList();
        }

        private void CheckBox_Click(object sender, RoutedEventArgs e)
        {
            CheckBox radioButton = (CheckBox)sender;
            (this.DataContext as MainContext).updateMaterialList();
        }
    }
}
