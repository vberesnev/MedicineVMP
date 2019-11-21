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
    /// Логика взаимодействия для OpenVMPWindow.xaml
    /// </summary>
    public partial class OpenVMPView : Page
    {
        OpenPageDelegate deleg;
        public OpenVMPView(OpenPageDelegate sender)
        {
            InitializeComponent();
            deleg = sender;
            OpenVMPViewModel view = new OpenVMPViewModel();
            this.DataContext = view;
            if (view.CloseAction == null)
            {
                view.CloseAction = new Action(this.CloseAct);
            }
        }

        private void CloseAct()
        {
            deleg();
        }
    }
}
