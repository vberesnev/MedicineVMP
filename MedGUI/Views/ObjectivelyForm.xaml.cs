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
    /// Логика взаимодействия для ObjectivelyForm.xaml
    /// </summary>
    public partial class ObjectivelyForm : Page
    {
        public ObjectivelyForm()
        {
            InitializeComponent();
            DataContext = new ObjectivelyViewModel();
        }

        private void TextBox_PreviewTextInput(object sender, TextCompositionEventArgs e)
        {
            if (!Char.IsDigit(e.Text, 0)) e.Handled = true;
        }
    }


}
