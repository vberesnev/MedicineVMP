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
using MedGUI.ViewModel;

namespace MedGUI.Views
{
    /// <summary>
    /// Логика взаимодействия для GeneralHistoryView.xaml
    /// </summary>
    public partial class GeneralHistoryView : Page
    {
        public GeneralHistoryView()
        {
            InitializeComponent();
            DataContext = new GeneralHistoryViewModel();
        }

        private void ClearTextBoxesByCheckBoxes(object sender, RoutedEventArgs e)
        {
            CheckBox checkBox = sender as CheckBox;
            if (checkBox.Name == "operationsCheckBox")
                operations_textBox.Clear();
            if (checkBox.Name == "chronicDiseasesCheckBox")
                chronicDiseases_textBox.Clear();
            if (checkBox.Name == "allergyHistoryCheckBox")
                allergyHistory_textBox.Clear();
        }
    }
}
