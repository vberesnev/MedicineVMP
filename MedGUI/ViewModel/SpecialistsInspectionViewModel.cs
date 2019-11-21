using MedGUI.Model;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.ComponentModel;
using System.Linq;
using System.Runtime.CompilerServices;
using System.Text;
using System.Threading.Tasks;

namespace MedGUI.ViewModel
{
    public class SpecialistsInspectionViewModel : INotifyPropertyChanged
    {
        private RelayCommand addSpecialistsInspectionCommand;
        public RelayCommand AddSpecialistsInspectionCommand
        {
            get
            {
                return addSpecialistsInspectionCommand ??
                  (addSpecialistsInspectionCommand = new RelayCommand(obj =>
                  {
                      SpecialistInspection inspection = obj as SpecialistInspection;
                      if (inspection != null)
                      {
                          SpecialistInspections.Add(inspection);
                          SingletonVMP.VMP.SpecialistInspections.Add(inspection);
                         SelectedSpecialistInspection = new SpecialistInspection() { Date = DateTime.Now.Date };
                      }
                  }));
            }
        }

        private RelayCommand deleteSpecialistsInspectionCommand;
        public RelayCommand DeleteSpecialistsInspectionCommand
        {
            get
            {
                return deleteSpecialistsInspectionCommand ??
                  (deleteSpecialistsInspectionCommand = new RelayCommand(obj =>
                  {
                      SpecialistInspection inspection = obj as SpecialistInspection;
                      if (inspection != null)
                      {
                          SpecialistInspections.Remove(inspection);
                          SingletonVMP.VMP.SpecialistInspections.Remove(inspection);
                      }
                  },
                  (obj) => SpecialistInspections.Count > 0));
            }
        }

        private SpecialistInspection selectedSpecialistInspection;
        public ObservableCollection<SpecialistInspection> SpecialistInspections { get; set; }

        public SpecialistInspection SelectedSpecialistInspection
        {
            get { return selectedSpecialistInspection; }
            set
            {
                selectedSpecialistInspection = value;
                OnPropertyChanged("SelectedSpecialistInspection");
            }
        }


        public SpecialistsInspectionViewModel()
        {
            SelectedSpecialistInspection = new SpecialistInspection() { Date = DateTime.Now.Date };
            
            if (SingletonVMP.VMP.SpecialistInspections == null)
            {
                SingletonVMP.VMP.SpecialistInspections = new List<SpecialistInspection>();
                SpecialistInspections = new ObservableCollection<SpecialistInspection>();
            }
            else
            {
                SpecialistInspections = new ObservableCollection<SpecialistInspection>(SingletonVMP.VMP.SpecialistInspections);
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
