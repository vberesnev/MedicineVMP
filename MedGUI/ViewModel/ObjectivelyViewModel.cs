using MedGUI.Model;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data.Entity;
using System.Linq;
using System.Runtime.CompilerServices;
using System.Text;
using System.Threading.Tasks;

namespace MedGUI.ViewModel
{
    public class ObjectivelyViewModel : INotifyPropertyChanged
    {

        private RelayCommand saveEyesDataCommand;
        public RelayCommand SaveEyesDataCommand
        {
            get
            {
                return saveEyesDataCommand ??
                  (saveEyesDataCommand = new RelayCommand(obj =>
                  {
                      if (SelectedDescription.Both)
                      {
                          OSEye.Equate(ODEye);
                      }
                  }));
            }
        }

        private Eye odEye;
        private Eye osEye;

        private EyeDescriptionMethod selectedDescription;

        private ICollection<EyeDescriptionMethod> eyeDescriptionMethods;
        private ICollection<Eye> eyes;
        private ICollection<EyeSide> eyeSides;

        public Eye ODEye
        {
            get { return odEye; }
            set
            {
                odEye = value;
                OnPropertyChanged("ODEye");
            }
        }

        public Eye OSEye
        {
            get { return osEye; }
            set
            {
                osEye = value;
                OnPropertyChanged("OSEye");
            }
        }

        public EyeDescriptionMethod SelectedDescription
        {
            get { return selectedDescription; }
            set
            {
                selectedDescription = value;
                OnPropertyChanged("SelectedDescription");
            }
        }

        public ICollection<EyeDescriptionMethod> EyeDescriptionMethods
        {
            get { return eyeDescriptionMethods; }
            set
            {
                eyeDescriptionMethods = value;
                OnPropertyChanged("EyeDescriptionMethods");
            }
        }

        public ICollection<Eye> Eyes
        {
            get { return eyes; }
            set
            {
                eyes = value;
                OnPropertyChanged("Eyes");
            }
        }

        public ICollection<EyeSide> EyeSides
        {
            get { return eyeSides; }
            set
            {
                eyeSides = value;
                OnPropertyChanged("EyeSides");
            }
        }


        public ObjectivelyViewModel()
        {
            SelectedDescription = new EyeDescriptionMethod("Оба сразу", true, "0");
            EyeDescriptionMethods = new List<EyeDescriptionMethod>()
            {
                SelectedDescription,
                new EyeDescriptionMethod("По отдельности", false, "2*")
            };

            using (MedicineContext db = new MedicineContext())
            {
                db.EyeSides.Load();
                EyeSides = db.EyeSides.Local.ToBindingList();
            }

            EyeSide ODSide = EyeSides.FirstOrDefault(x => x.Side == "OD");
            EyeSide OSSide = EyeSides.FirstOrDefault(x => x.Side == "OS");

            if (SingletonVMP.VMP.Eyes == null)
            {
                ODEye = new Eye(ODSide);
                OSEye = new Eye(OSSide);
                Eyes = new List<Eye>();
                Eyes.Add(ODEye);
                Eyes.Add(OSEye);
                SingletonVMP.VMP.Eyes = Eyes;
            }
            else
            {
                Eyes = SingletonVMP.VMP.Eyes;
                ODEye = Eyes.FirstOrDefault(s => s.EyeSideId == ODSide.Id);
                OSEye = Eyes.FirstOrDefault(s => s.EyeSideId == OSSide.Id);
                if (ODEye.CompareTo(OSEye) == -1)
                {
                    EyeDescriptionMethods.Clear();
                    SelectedDescription = new EyeDescriptionMethod("По отдельности", false, "2*");
                    EyeDescriptionMethods = new List<EyeDescriptionMethod>()
                    {
                        new EyeDescriptionMethod("Оба сразу", true, "0"),
                        SelectedDescription
                     };
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
