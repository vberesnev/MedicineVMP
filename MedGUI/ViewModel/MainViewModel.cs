using MedGUI.Model;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Runtime.CompilerServices;
using System.Text;
using System.Threading;
using System.Threading.Tasks;

namespace MedGUI.ViewModel
{
    public class MainViewModel : INotifyPropertyChanged
    {
        public RelayCommand PrintCommand { get; set; }

        public RelayCommand SaveCommand { get; set; }

        public RelayCommand SetInfoTextCommand { get; set; }

        public RelayCommand RefreshCommand { get; set; }

        private SingletonVMP singletonVMP;

        public SingletonVMP SingletonVMP
        {
            get { return singletonVMP; }
            set
            {
                singletonVMP = value;
                OnPropertyChanged("SingletonVMP");
            }
        }

        private string infoMessage;

        public string InfoMessage
        {
            get { return infoMessage; }
            set
            {
                infoMessage = value;
                OnPropertyChanged("InfoMessage");
            }
        }

        public MainViewModel()
        {
            this.PrintCommand = new RelayCommand((x) =>
            {
                List<string> checkList = CheckVMP();
                checkList = null;
                if (checkList == null)
                {
                    InfoMessage = SingletonVMP.Print(Settings.Open());
                }
                else
                {
                    InfoMessage = GetErrorText(checkList);
                }
            });

            this.SaveCommand = new RelayCommand((x) =>
            {
                InfoMessage = SingletonVMP.Save();
            });

            this.SetInfoTextCommand = new RelayCommand((x) =>
            {
                if (x != null)
                {
                    InfoMessage = x.ToString();
                }
            });

            this.RefreshCommand = new RelayCommand((x) =>
            {
                SingletonVMP.Clear();
            });

            SingletonVMP = SingletonVMP.GetInstance();
            InfoMessage = "Сформируйте ВМП. Удачи...";
        }

        private string GetErrorText(List<string> checkList)
        {
            string text = "ОШИБКА!\r\nЗаполните следующую информацию:\r\n";
            for (int i = 0; i < checkList.Count; i++)
            {
                text += checkList[i];
                if (i < checkList.Count - 1)
                {
                    text += "\r\n";
                }
            }
            return text;
        }

        private List<string> CheckVMP()
        {
            List<string> list = new List<string>();
            if (SingletonVMP.VMP.Patient == null)
                list.Add("Персональную информацию о пациенте;");
            if (SingletonVMP.VMP.GeneralHistory == null)
                list.Add("Основной анамнез;");
            if (SingletonVMP.VMP.Eyes == null)
                list.Add("Объективно;");
            if (SingletonVMP.VMP.AdditionalResearch == null)
                list.Add("Дополнительные исследования;");
            if (SingletonVMP.VMP.SpecialistInspections == null)
                list.Add("Осмотр специалистов;");
            if (SingletonVMP.VMP.Diagnos == null)
                list.Add("Диагноз;");
            if (list.Count > 0)
                return list;
            else
                return null;
        }

        public event PropertyChangedEventHandler PropertyChanged;

        public void OnPropertyChanged([CallerMemberName]string prop = "")
        {
            if (PropertyChanged != null)
                PropertyChanged(this, new PropertyChangedEventArgs(prop));
        }
    }
}
