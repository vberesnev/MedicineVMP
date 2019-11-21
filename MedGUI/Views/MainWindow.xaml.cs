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
using MedGUI.Views;
using MedGUI.ViewModel;
namespace MedGUI.Views
{

    public delegate void OpenPageDelegate();

    /// <summary>
    /// Логика взаимодействия для MainWindow.xaml
    /// </summary>
    public partial class MainWindow : Window
    {
        private bool isPatientForm = false;
        private bool isGeneralHistoryForm = false;
        private bool isObjectivelyForm = false;
        private bool isAdditionalResearchForm = false;
        private bool isSpecialistsInspectionForm = false;
        private bool isDiagnosisForm = false;

        public MainWindow()
        {
            InitializeComponent();
            DataContext = new MainViewModel();
            MainFrame.Navigate(new StartupView(this));
        }


        private void OpenPage()
        {
            patientBtn.Style = (Style)patientBtn.FindResource("ActiveButton");
            MainFrame.Navigate(new PatientView());
            isPatientForm = true;
        }

        private void MenuButton_Click(object sender, RoutedEventArgs e)
        {
            SetDefaultMenuStyle();
            Button btn = (Button)sender;
            btn.Style = (Style)btn.FindResource("ActiveButton");
        }

        private void SetDefaultMenuStyle()
        {
            SetStyleToItem("patientBtn", "DefaultButton");
            SetStyleToItem("generalHistoryBtn", "DefaultButton");
            SetStyleToItem("objectivelyBtn", "DefaultButton");
            SetStyleToItem("additionalResearchBtn", "DefaultButton");
            SetStyleToItem("specialistsInspectionBtn", "DefaultButton");
            SetStyleToItem("diagnosisBtn", "DefaultButton");
            isPatientForm = false;
            isGeneralHistoryForm = false;
            isObjectivelyForm = false;
            isAdditionalResearchForm = false;
            isSpecialistsInspectionForm = false;
            isDiagnosisForm = false;
        }

        private void SetStyleToItem(string elementName, string styleName)
        {
            var item = (Button)this.FindName(elementName);
            item.Style = (Style)item.FindResource(styleName);
        }

        private void FirstMenuButton_Click(object sender, RoutedEventArgs e)
        {
            if (!isPatientForm)
            {
                SetDefaultMenuStyle();
                patientBtn.Style = (Style)patientBtn.FindResource("ActiveButton");
                MainFrame.Navigate(new PatientView());
                isPatientForm = true;
            }
        }

        private void SecondMenuButton_Click(object sender, RoutedEventArgs e)
        {
            if (!isGeneralHistoryForm)
            {
                SetDefaultMenuStyle();
                generalHistoryBtn.Style = (Style)generalHistoryBtn.FindResource("ActiveButton");
                MainFrame.Navigate(new GeneralHistoryView());
                isGeneralHistoryForm = true;
            }
        }

        private void ThirdMenuButton_Click(object sender, RoutedEventArgs e)
        {
            if (!isObjectivelyForm)
            {
                SetDefaultMenuStyle();
                objectivelyBtn.Style = (Style)objectivelyBtn.FindResource("ActiveButton");
                MainFrame.Navigate(new ObjectivelyForm());
                isObjectivelyForm = true;
            }
        }

        private void FourthMenuButton_Click(object sender, RoutedEventArgs e)
        {
            if (!isAdditionalResearchForm)
            {
                SetDefaultMenuStyle();
                additionalResearchBtn.Style = (Style)additionalResearchBtn.FindResource("ActiveButton");
                MainFrame.Navigate(new AdditionalResearchView());
                isAdditionalResearchForm = true;
            }
        }

        private void FifthMenuButton_Click(object sender, RoutedEventArgs e)
        {
            if (!isSpecialistsInspectionForm)
            {
                SetDefaultMenuStyle();
                specialistsInspectionBtn.Style = (Style)specialistsInspectionBtn.FindResource("ActiveButton");
                MainFrame.Navigate(new SpecialistsInspectionView());
                isSpecialistsInspectionForm = true;
            }
        }

        private void SixthMenuButton_Click(object sender, RoutedEventArgs e)
        {
            if (!isDiagnosisForm)
            {
                SetDefaultMenuStyle();
                diagnosisBtn.Style = (Style)diagnosisBtn.FindResource("ActiveButton");
                MainFrame.Navigate(new DiagmosisView());
                isDiagnosisForm = true;
            }
        }

        private void RefreshButton_Click(object sender, RoutedEventArgs e)
        {
            MainFrame.Navigate(new StartupView(this));
            SetDefaultMenuStyle();
        }

        private void OpenVMPButton_Click(object sender, RoutedEventArgs e)
        {
            OpenPageDelegate deleg = OpenPage;
            MainFrame.Navigate(new OpenVMPView(deleg));
            SetDefaultMenuStyle();
        }
    }
}
