using MedGUI.Model;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Runtime.CompilerServices;
using System.Text;
using System.Threading.Tasks;

namespace MedGUI.ViewModel
{
    public class DiagnosisViewModel : INotifyPropertyChanged
    {
        public RelayCommand ComplicationsUncheckedCommand { get; set; }

        private Diagnos diagnos;

        private bool isComplications;

        public Diagnos Diagnos
        {
            get { return diagnos; }
            set
            {
                diagnos = value;
                OnPropertyChanged("Diagnos");
            }
        }

        public bool IsComplications
        {
            get { return isComplications; }
            set
            {
                isComplications = value;
                OnPropertyChanged("IsComplications");
            }
        }

        public DiagnosisViewModel()
        {
            this.ComplicationsUncheckedCommand = new RelayCommand((x) =>
            {
                IsComplications = false;
                Diagnos.Complications = null;
            });

            if (SingletonVMP.VMP.Diagnos == null)
            {
                Diagnos = new Diagnos();
                SingletonVMP.VMP.Diagnos = Diagnos;
            }
            else
            {
                Diagnos = SingletonVMP.VMP.Diagnos;
                isComplications = !(string.IsNullOrEmpty(Diagnos.Complications) || string.IsNullOrWhiteSpace(Diagnos.Complications));
            }
        }

        public event PropertyChangedEventHandler PropertyChanged;

        public void OnPropertyChanged([CallerMemberName]string prop = "")
        {
            if (PropertyChanged != null)
                PropertyChanged(this, new PropertyChangedEventArgs(prop));
        }
    }
}
