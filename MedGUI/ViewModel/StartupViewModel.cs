using MedGUI.Model;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Runtime.CompilerServices;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Input;

namespace MedGUI.ViewModel
{
    public class StartupViewModel : INotifyPropertyChanged
    {
        
        public RelayCommand SaveCommand { get; set; }

        private string info;
        public string Info
        {
            get { return info; }
            set
            {
                info = value;
                OnPropertyChanged("Info");
            }
        }

        private Settings settings;
        public Settings Settings
        {
            get { return settings; }
            set
            {
                settings = value;
                OnPropertyChanged("Settings");
            }
        }

        public StartupViewModel()
        {
            this.SaveCommand = new RelayCommand((x) =>
            {
                if (Settings != null)
                {
                    Info = Settings.Save();
                }
            });
            Settings = Settings.Open();
            Info = "";
        }

        public event PropertyChangedEventHandler PropertyChanged;

        public void OnPropertyChanged([CallerMemberName]string prop = "")
        {
            if (PropertyChanged != null)
                PropertyChanged(this, new PropertyChangedEventArgs(prop));
        }
    }
}
