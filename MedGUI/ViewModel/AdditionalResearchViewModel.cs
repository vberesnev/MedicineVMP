using MedGUI.Model;
using MedGUI.Model.AdditionalResearchModel;
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
    public class AdditionalResearchViewModel : INotifyPropertyChanged
    {
        private AdditionalResearch additionalResearch;

        private RelayCommand addOtherResearchCommand;
        public RelayCommand AddOtherResearchCommand
        {
            get
            {
                return addOtherResearchCommand ??
                  (addOtherResearchCommand = new RelayCommand(obj =>
                  {
                      OtherResearch research = obj as OtherResearch;
                      if (research != null)
                      {
                          OtherResearches.Add(research);
                          SingletonVMP.VMP.AdditionalResearch.OtherResearches.Add(research);
                          SelectedOtherResearch = new OtherResearch() { Date = DateTime.Now.Date };
                      }
                  }));
            }
        }

        private RelayCommand deleteOtherResearchCommand;
        public RelayCommand DeleteOtherResearchCommand
        {
            get
            {
                return deleteOtherResearchCommand ??
                  (deleteOtherResearchCommand = new RelayCommand(obj =>
                  {
                      OtherResearch research = obj as OtherResearch;
                      if (research != null)
                      {
                          OtherResearches.Remove(research);
                          SingletonVMP.VMP.AdditionalResearch.OtherResearches.Remove(research);
                      }
                  },
                  (obj) => OtherResearches.Count > 0));
            }
        }

        private OtherResearch selectedOtherResearch;
        public ObservableCollection<OtherResearch> OtherResearches { get; set; }

        public OtherResearch SelectedOtherResearch
        {
            get { return selectedOtherResearch; }
            set
            {
                selectedOtherResearch = value;
                OnPropertyChanged("SelectedOtherResearch");
            }
        }

        public AdditionalResearch AdditionalResearch
        {
            get { return additionalResearch; }
            set
            {
                additionalResearch = value;
                OnPropertyChanged("AdditionalResearch");
            }
        }

        public AdditionalResearchViewModel()
        {
            SelectedOtherResearch = new OtherResearch() { Date = DateTime.Now.Date };
            OtherResearches = new ObservableCollection<OtherResearch>();
            if (SingletonVMP.VMP.AdditionalResearch == null)
            {
                AdditionalResearch = new AdditionalResearch();
                AdditionalResearch.GeneralBloodAnalysis = new GeneralBloodAnalys() {};
                AdditionalResearch.ChemistryBloodAnalysis = new ChemistryBloodAnalys() {};
                AdditionalResearch.MP = new MP() {};
                AdditionalResearch.GeneralUrineAnalysis = new GeneralUrineAnalys() {};
                AdditionalResearch.Feces = new Feces() {};
                AdditionalResearch.ECG = new ECG() {};

                SingletonVMP.VMP.AdditionalResearch = AdditionalResearch;

                SingletonVMP.VMP.AdditionalResearch.OtherResearches = new List<OtherResearch>();
                OtherResearches = new ObservableCollection<OtherResearch>();

                //AdditionalResearch.OtherResearches = OtherResearches;
            }
            else
            {
                AdditionalResearch = SingletonVMP.VMP.AdditionalResearch;
                if (additionalResearch.OtherResearches != null)
                {
                    OtherResearches = new ObservableCollection<OtherResearch>(AdditionalResearch.OtherResearches);
                }
                else
                {
                    OtherResearches = new ObservableCollection<OtherResearch>();
                }
                
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
