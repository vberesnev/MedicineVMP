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
using System.Windows.Forms;

namespace MedGUI.Views
{
    /// <summary>
    /// Логика взаимодействия для StartupView.xaml
    /// </summary>
    public partial class StartupView : Page
    {
        private FolderBrowserDialog folderBrowserDialog;
        private MainWindow Main;

        public StartupView(MainWindow mainWindow)
        {
            InitializeComponent();
            DataContext = new StartupViewModel();
            Main = mainWindow;
        }

        private void PathButton_Click(object sender, RoutedEventArgs e)
        {
            folderBrowserDialog = new FolderBrowserDialog();

            if (folderBrowserDialog.ShowDialog() == DialogResult.OK)
            {
                path_textBox.Text = folderBrowserDialog.SelectedPath;
                path_textBox.Focus();
            }
        }
    }
}
