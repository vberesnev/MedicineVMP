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
using System.Windows.Shapes;
using MedGUI.Model.PatientModel;
using MedGUI.Model;

namespace MedGUI.Views
{
    /// <summary>
    /// Логика взаимодействия для AddPoliceWindow.xaml
    /// </summary>
    public partial class AddPoliceWindow : Window
    {
        public AddPoliceWindow()
        {
            InitializeComponent();
        }

        private void CancelButton_Click(object sender, RoutedEventArgs e)
        {
            this.Close();
        }

        private void AddPolice_button_Click(object sender, RoutedEventArgs e)
        {
            if (!string.IsNullOrWhiteSpace(police_textBox.Text))
            {
                using (MedicineContext db = new MedicineContext())
                {
                    db.Policies.Add(new Policy() { Name = police_textBox.Text });
                    db.SaveChanges();
                }
                this.Close();
            }
        }
    }
}
