using MedGUI.Model;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.ComponentModel;
using System.Linq;
using System.Runtime.CompilerServices;
using System.Text;
using System.Threading.Tasks;
using System.Data.Entity;

namespace MedGUI.ViewModel
{
    public class OpenVMPViewModel : INotifyPropertyChanged
    {
        private RelayCommand selectVMPCommand;
        public RelayCommand SelectVMPCommand
        {
            get
            {
                return selectVMPCommand ??
                  (selectVMPCommand = new RelayCommand(obj =>
                  {
                      VMP selectedVMP = obj as VMP;
                      if (selectedVMP != null)
                      {
                          try
                          {
                              SingletonVMP.Open(selectedVMP.Id);
                              CloseAction();
                          }
                          catch (Exception ex)
                          {
                              InfoText = ex.Message;
                          }
                          
                      }
                  },
                  (obj) => SelectedVMP != null));
            }
        }

        private RelayCommand cancelCommand;
        public RelayCommand CancelCommand
        {
            get
            {
                return cancelCommand ??
                  (cancelCommand = new RelayCommand(obj =>
                  {
                      CloseAction();
                  }));
            }
        }

        private RelayCommand textFilterChangedCommand;
        public RelayCommand TextFilterChangedCommand
        {
            get
            {
                return textFilterChangedCommand ??
                  (textFilterChangedCommand = new RelayCommand(obj =>
                  {
                      if (!string.IsNullOrWhiteSpace(TextFilter))
                      {
                          VMPList.Clear();
                          var temp = VMPAll.Where(x => x.Patient != null && x.Patient.Name != null && x.Patient.Name.ToLower().Contains(TextFilter.ToLower()));
                          foreach (var item in temp)
                          {
                              VMPList.Add(item);
                          }
                      }
                      else
                      {
                          VMPList.Clear();
                          foreach (var item in VMPAll)
                          {
                              VMPList.Add(item);
                          }
                      }
                  }));
            }
        }

        public Action CloseAction { get; set; }

        private List<VMP> VMPAll { get; set; }

        public ObservableCollection<VMP> VMPList { get; set; }

        private string textFilter;
        public string TextFilter
        {
            get { return textFilter; }
            set
            {
                textFilter = value;
                OnPropertyChanged("TextFilter");
            }
        }

        private string infoText;
        public string InfoText
        {
            get { return infoText; }
            set
            {
                infoText = value;
                OnPropertyChanged("InfoText");
            }
        }

        private VMP selectedVMP;
        public VMP SelectedVMP
        {
            get { return selectedVMP; }
            set
            {
                selectedVMP = value;
                OnPropertyChanged("SelectedVMP");
            }
        }

        public OpenVMPViewModel()
        {
            VMPList = new ObservableCollection<VMP>();
            InfoText = "Загрузка данных . . .";
            LoadData();
        }

        private async void LoadData()
        {
            await Task.Run(() =>
            {
                using (MedicineContext db = new MedicineContext())
                {
                    var data = db.VMPs.Include(x => x.Patient);

                    VMPAll = new List<VMP>();
                    foreach (var d in data)
                    {
                        VMPAll.Add(d);
                    }
                }
            });
            InfoText = string.Empty;
            foreach (var item in VMPAll)
            {
                VMPList.Add(item);
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
