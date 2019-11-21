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
    /// Логика взаимодействия для AdditionalResearchView.xaml
    /// </summary>
    public partial class AdditionalResearchView : Page
    {
        public AdditionalResearchView()
        {
            InitializeComponent();
            DataContext = new AdditionalResearchViewModel();
        }

        private void ECG_addInfo_checkBox_Unchecked(object sender, RoutedEventArgs e)
        {
            CheckBox checkBox = sender as CheckBox;
            if (checkBox.Name == "ECG_addInfo_checkBox")
                ECG_addInfo_TextBox.Clear();
        }

        private void TextBox_PreviewTextInput(object sender, TextCompositionEventArgs e)
        {
            if (Char.IsDigit(e.Text, 0) || (e.Text == ".") || (e.Text == ",")) return;
            else e.Handled = true;

        }
    }
}
