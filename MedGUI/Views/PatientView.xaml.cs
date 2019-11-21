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
using System.Data.Entity;

namespace MedGUI.Views
{
    /// <summary>
    /// Логика взаимодействия для PatientView.xaml
    /// </summary>
    public partial class PatientView : Page
    {
        private bool JustChecked;

        public PatientView()
        {
            InitializeComponent();
            SetDataContextAsync();
        }

        private async void SetDataContextAsync()
        {
            this.DataContext = await Task<object>.Factory.StartNew(() =>
            {
                return new PatientViewModel();
            });
        }

        private void TextBox_PreviewTextInput(object sender, TextCompositionEventArgs e)
        {
            if (!Char.IsDigit(e.Text, 0)) e.Handled = true;
        }

        private void Group_radioButton_Click(object sender, RoutedEventArgs e)
        {
            if (JustChecked)
            {
                JustChecked = false;
                e.Handled = true;
                return;
            }
            RadioButton s = sender as RadioButton;
            if (s.IsChecked == true)
                s.IsChecked = false;
        }

        private void Group_radioButton_Checked(object sender, RoutedEventArgs e)
        {
            RadioButton s = sender as RadioButton;
            JustChecked = true;
        }

        private void AddPoliceButton_Click(object sender, RoutedEventArgs e)
        {
            AddPoliceWindow APW = new AddPoliceWindow();
            APW.ShowDialog();
            PatientViewModel p = (PatientViewModel)this.DataContext;
            
            using (MedGUI.Model.MedicineContext db = new Model.MedicineContext())
            {
                db.Policies.Load();
                p.Policies = db.Policies.Local.ToBindingList();
            }

        }
    }
}
