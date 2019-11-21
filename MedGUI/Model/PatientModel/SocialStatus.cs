using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.ComponentModel.DataAnnotations.Schema;
using System.Runtime.CompilerServices;
using System.Text;

namespace MedGUI.Model.PatientModel
{
    [Table("SocialStatuses")]
    public class SocialStatus: INotifyPropertyChanged
    {
        private string status;

        public int? Id { get; set; }
        public string Status
        {
            get { return status; }
            set
            {
                status = value;
                OnPropertyChanged("Status");
            }
        }

        public ICollection<Patient> Patients { get; set; }

        public SocialStatus() { }

        public event PropertyChangedEventHandler PropertyChanged;
        public void OnPropertyChanged([CallerMemberName]string prop = "")
        {
            if (PropertyChanged != null)
                PropertyChanged(this, new PropertyChangedEventArgs(prop));
        }

        public override string ToString()
        {
            return Status;
        }
    }
}
