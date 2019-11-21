using MedGUI.ViewModel;
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

namespace MedGUI.Views
{
    /// <summary>
    /// Логика взаимодействия для DiagmosisView.xaml
    /// </summary>
    public partial class DiagmosisView : Page
    {
        public DiagmosisView()
        {
            InitializeComponent();
            DataContext = new DiagnosisViewModel();
        }

        private void ClearTextBoxesByCheckBoxes(object sender, RoutedEventArgs e)
        {
            CheckBox checkBox = sender as CheckBox;
            if (checkBox.Name == "complicationsCheckBox")
                complications_TextBox.Clear();
        }
    }
}
