using MedGUI.Model.PatientModel;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Runtime.CompilerServices;
using System.Data.Entity;
using System.Text;
using System.Threading.Tasks;
using MedGUI.Model;
using System.Threading;

namespace MedGUI.ViewModel
{
    class PatientViewModel : INotifyPropertyChanged
    {
        public RelayCommand SavePatientDataCommand { get;  set; }

        private Patient patient;
        private int bDay;
        private int bMonth;
        private int bYear;

        private Policy selectedPolicy;
        private SocialStatus selectedSocialStatus;


        private bool disabilityI = false;
        private bool disabilityII = false;
        private bool disabilityIII = false;

        IEnumerable<SocialStatus> socialStatuses;
        IEnumerable<Policy> policies;
        IEnumerable<Month> months;
        IEnumerable<Disability> disabilities;

        #region Variables
        public Patient Patient
        {
            get { return patient; }
            set
            {
                patient = value;
                OnPropertyChanged("Patient");
            }
        }

        public int BDay
        {
            get { return bDay; }
            set
            {
                bDay = value;
                OnPropertyChanged("BDay");
            }
        }

        public int BMonth
        {
            get { return bMonth; }
            set
            {
                bMonth = value;
                OnPropertyChanged("BMonth");
            }
        }

        public int BYear
        {
            get { return bYear; }
            set
            {
                bYear = value;
                OnPropertyChanged("BYear");
            }
        }

        public Policy SelectedPolicy
        {
            get
            {
                if (selectedPolicy == null) return null;
                return Policies.FirstOrDefault(x => x.Id == selectedPolicy.Id);
            }
            set
            {
                selectedPolicy = value;
                OnPropertyChanged("SelectedPolicy");
            }
        }

        public SocialStatus SelectedSocialStatus
        {
            get
            {
                if (selectedSocialStatus == null) return null;
                return SocialStatuses.FirstOrDefault(x => x.Id == selectedSocialStatus.Id);
            }
            set
            {
                selectedSocialStatus = value;
                OnPropertyChanged("SelectedSocialStatus");
            }
        }

        public  bool DisabilityI
        {
            get { return disabilityI; }
            set
            {
                disabilityI = value;
                if (disabilityI)
                {
                    this.DisabilityII = false;
                    this.DisabilityIII = false;
                }
                OnPropertyChanged("DisabiltyI");
            }
        }

        public bool DisabilityII
        {
            get { return disabilityII; }
            set
            {
                disabilityII = value;
                if (disabilityII)
                {
                    this.DisabilityI = false;
                    this.DisabilityIII = false;
                }
                OnPropertyChanged("DisabiltyII");
            }
        }

        public bool DisabilityIII
        {
            get { return disabilityIII; }
            set
            {
                disabilityIII = value;
                if (disabilityIII)
                {
                    this.DisabilityI = false;
                    this.DisabilityII = false;
                }
                OnPropertyChanged("DisabiltyIII");
            }
        }

        public IEnumerable<Month> Months
        {
            get { return months; }
            set
            {
                months = value;
                OnPropertyChanged("Months");
            }
        }

        public IEnumerable<SocialStatus> SocialStatuses
        {
            get { return socialStatuses; }
            set
            {
                socialStatuses = value;
                OnPropertyChanged("SocialStatuses");
            }
        }

        public IEnumerable<Policy> Policies
        {
            get { return policies; }
            set
            {
                policies = value;
                OnPropertyChanged("Policies");
            }
        }

        public IEnumerable<Disability> Disabilities
        {
            get { return disabilities; }
            set
            {
                disabilities = value;
                OnPropertyChanged("Disabilities");
            }
        }
        #endregion

        public PatientViewModel()
        {
            this.SavePatientDataCommand = new RelayCommand(this.SavePatientData);
            Months = GetMonths();

            using (MedicineContext db = new MedicineContext())
            {
                db.SocialStatuses.Load();
                db.Policies.Load();
                db.Disabilities.Load();
                SocialStatuses = db.SocialStatuses.Local.ToBindingList();
                Policies = db.Policies.Local.ToBindingList();
                Disabilities = db.Disabilities.Local.ToBindingList();
            }
            if (SingletonVMP.VMP.Patient == null)
            {
                Patient = new Patient();
                SingletonVMP.VMP.Patient = Patient;
            }
            else
            {
                Patient = SingletonVMP.VMP.Patient;
                GetBirthday();
                GetDisability();
                SelectedSocialStatus = SocialStatuses.FirstOrDefault(x => x.Id == Patient.SocialStatusId);
                SelectedPolicy = Policies.FirstOrDefault(x => x.Id == Patient.PoliceId);
            }
        }


        private void SavePatientData(object obj)
        {
            if (SingletonVMP.VMP.Patient == null)
                return;
            if (BYear != 0 && BMonth != 0 && BDay != 0)
                SingletonVMP.VMP.Patient.Birthday = SetBirthday();
            if (SelectedSocialStatus != null)
            {
                SingletonVMP.VMP.Patient.SocialStatusId = SelectedSocialStatus.Id;
            }
            if (SelectedPolicy != null)
            {
                SingletonVMP.VMP.Patient.PoliceId = SelectedPolicy.Id;
            }
            if (Patient != null)
            {
                SetDisability();
            }
            
        }

        private void GetBirthday()
        {
            if (Patient.Birthday != DateTime.MinValue)
            {
                BDay = Patient.Birthday.Day;
                BMonth = Patient.Birthday.Month;
                BYear = Patient.Birthday.Year;
            }
            else
            {
                BDay = 0;
                BMonth = 0;
                BYear = 0;
            }
        }

        private DateTime SetBirthday()
        {
            return new DateTime(BYear, BMonth, BDay);
        }

        private  void GetDisability()
        {
            if (Patient.DisabilityId == null)
            {
                return;
            }
            else if (Patient.DisabilityId == Disabilities.FirstOrDefault(x => x.Category == "1 группа инвалидности").Id)
            {
                DisabilityI = true;
            }
            else if (Patient.DisabilityId == Disabilities.FirstOrDefault(x => x.Category == "2 группа инвалидности").Id)
            {
                DisabilityII = true;
            }
            else
            {
                DisabilityIII = true;
            }
           
        }

        private void SetDisability()
        {
            if (DisabilityI)
            {
                SingletonVMP.VMP.Patient.DisabilityId = Disabilities.FirstOrDefault(d => d.Category == "1 группа инвалидности").Id;
            }
            else if (DisabilityII)
            {
                SingletonVMP.VMP.Patient.DisabilityId = Disabilities.FirstOrDefault(d => d.Category == "2 группа инвалидности").Id;
            }
            else if (DisabilityIII)
            {
                SingletonVMP.VMP.Patient.DisabilityId = Disabilities.FirstOrDefault(d => d.Category == "3 группа инвалидности").Id;
            }
            else
            {
                SingletonVMP.VMP.Patient.DisabilityId = null;
            }
        }

        private IEnumerable<Month> GetMonths()
        {
            return new List<Month>()
            {
                new Month(1, "Январь"),
                new Month(2, "Февраль"),
                new Month(3, "Март"),
                new Month(4, "Апрель"),
                new Month(5, "Май"),
                new Month(6, "Июнь"),
                new Month(7, "Июль"),
                new Month(8, "Август"),
                new Month(9, "Сентябрь"),
                new Month(10, "Октябрь"),
                new Month(11, "Ноябрь"),
                new Month(12, "Декабрь"),
            };
        }

        public event PropertyChangedEventHandler PropertyChanged;

        public void OnPropertyChanged([CallerMemberName]string prop = "")
        {
            if (PropertyChanged != null)
                PropertyChanged(this, new PropertyChangedEventArgs(prop));
        }
    }
}
