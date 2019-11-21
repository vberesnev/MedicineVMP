using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Runtime.CompilerServices;
using System.Text;
using System.Threading.Tasks;
using MedGUI.Model;

namespace MedGUI.ViewModel
{
    class GeneralHistoryViewModel : INotifyPropertyChanged
    {
        public RelayCommand OperationsUncheckedCommand { get; set; }
        public RelayCommand ChronicDiseasesUncheckedCommand { get; set; }
        public RelayCommand AllergyHistoryUncheckedCommand { get; set; }

        private GeneralHistory generalHistory;

        private bool isOperations;
        private bool isChronicDiseases;
        private bool isAllergyHistory;

        public GeneralHistory GeneralHistory
        {
            get { return generalHistory; }
            set
            {
                generalHistory = value;
                OnPropertyChanged("GeneralHistory");
            }
        }

        public bool IsOperations
        {
            get { return isOperations; }
            set
            {
                isOperations = value;
                OnPropertyChanged("IsOperations");
            }
        }

        public bool IsChronicDiseases
        {
            get { return isChronicDiseases; }
            set
            {
                isChronicDiseases = value;
                OnPropertyChanged("IsChronicDiseases");
            }
        }

        public bool IsAllergyHistory
        {
            get { return isAllergyHistory; }
            set
            {
                isAllergyHistory = value;
                OnPropertyChanged("IsAllergyHistory");
            }
        }

        public GeneralHistoryViewModel()
        {
            this.OperationsUncheckedCommand = new RelayCommand((x) => 
            {
                IsOperations = false;
                GeneralHistory.Operations = null;
            });
            this.ChronicDiseasesUncheckedCommand = new RelayCommand((x) => 
            {
                IsChronicDiseases = false;
                GeneralHistory.ChronicDiseases = null;
            });
            this.AllergyHistoryUncheckedCommand = new RelayCommand((x) => 
            {
                IsAllergyHistory = false;
                GeneralHistory.AllergyHistory = null;
            });

            if (SingletonVMP.VMP.GeneralHistory == null)
            {
                GeneralHistory = new GeneralHistory();
                SingletonVMP.VMP.GeneralHistory = GeneralHistory;
            }
            else
            {
                GeneralHistory = SingletonVMP.VMP.GeneralHistory;
                SetCheckBoxes();
            }
        }

        private void SetCheckBoxes()
        {
            IsOperations = !(string.IsNullOrEmpty(GeneralHistory.Operations) || string.IsNullOrWhiteSpace(GeneralHistory.Operations));
            IsChronicDiseases = !(string.IsNullOrEmpty(GeneralHistory.ChronicDiseases) || string.IsNullOrWhiteSpace(GeneralHistory.ChronicDiseases));
            IsAllergyHistory = !(string.IsNullOrEmpty(GeneralHistory.AllergyHistory) || string.IsNullOrWhiteSpace(GeneralHistory.AllergyHistory));
        }

        public event PropertyChangedEventHandler PropertyChanged;

        public void OnPropertyChanged([CallerMemberName]string prop = "")
        {
            if (PropertyChanged != null)
                PropertyChanged(this, new PropertyChangedEventArgs(prop));
        }


    }
}
