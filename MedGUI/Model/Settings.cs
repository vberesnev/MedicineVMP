using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Xml.Serialization;

namespace MedGUI.Model
{
    [Serializable]
    public class Settings
    {
        public string SavePath { get; set; }
        public string DoctorPosition { get; set; }
        public string DoctorName { get; set; }
        public string BossDoctorPosition { get; set; }
        public string BossDoctorName { get; set; }
        
        public Settings() { }

        public string Save()
        {
            try
            {
                XmlSerializer formatter = new XmlSerializer(typeof(Settings));

                using (FileStream fs = new FileStream("settings.xml", FileMode.OpenOrCreate))
                {
                    formatter.Serialize(fs, this);
                }
                return "Настройки успешно сохранены . . .";
            }
            catch(Exception ex)
            {
                return $"Ошибка сохранения: {ex.Message}";
            }
            
        }

        public static Settings Open()
        {
            XmlSerializer formatter = new XmlSerializer(typeof(Settings));
            try
            {
                using (FileStream fs = new FileStream("settings.xml", FileMode.OpenOrCreate))
                {
                    Settings settings = (Settings)formatter.Deserialize(fs);
                    return settings;
                }
            }
            catch (Exception ex)
            {
                return new Settings() { SavePath = @"C:\" };
            }
            
        }

    }
}
