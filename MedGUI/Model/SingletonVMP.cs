using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Reflection;
using System.Text;
using System.Threading.Tasks;
using MedGUI.Model.AdditionalResearchModel;
using MedGUI.Model.PatientModel;
using Word = Microsoft.Office.Interop.Word;
using System.Data.Entity;
using Microsoft.Office.Interop.Word;

namespace MedGUI.Model
{
    public class SingletonVMP
    {
        private static SingletonVMP instance;

        #region Для печати в Word

        private static string Template { get { return Environment.CurrentDirectory + "\\template.dotx"; } }
        private static Object missingObj = Missing.Value;
        private static Object trueObj = true;
        private static Object falseObj = false;
        private static Object saveChanges = Word.WdSaveOptions.wdDoNotSaveChanges;

        #endregion

        public static VMP VMP { get; private set; }
        public static Settings Settings { get; set; }

        public SingletonVMP()
        {
            VMP = new VMP();
        }

        public void Clear()
        {
            VMP = null;
            VMP = new VMP();
        }

        public static SingletonVMP GetInstance()
        {
            if (instance == null)
               instance = new SingletonVMP();
            return instance;
        }

        public static void Open(int id)
        {
            using (MedicineContext db = new MedicineContext())
            {
                VMP = db.VMPs.Include(x => x.Patient)
                            .Include(x => x.Patient.Disability)
                            .Include(x => x.GeneralHistory)
                            .Include(x => x.Eyes)
                            .Include(x => x.AdditionalResearch)
                            .Include(x => x.AdditionalResearch.GeneralBloodAnalysis)
                            .Include(x => x.AdditionalResearch.ChemistryBloodAnalysis)
                            .Include(x => x.AdditionalResearch.MP)
                            .Include(x => x.AdditionalResearch.GeneralUrineAnalysis)
                            .Include(x => x.AdditionalResearch.Feces)
                            .Include(x => x.AdditionalResearch.ECG)
                            .Include(x => x.AdditionalResearch.OtherResearches)
                            .Include(x => x.SpecialistInspections)
                            .Include(x => x.Diagnos)
                            .FirstOrDefault(x => x.Id == id);
                if (VMP.Eyes.Count == 0)
                    VMP.Eyes = null;
                if (VMP.AdditionalResearch != null && VMP.AdditionalResearch.OtherResearches.Count == 0)
                    VMP.AdditionalResearch.OtherResearches = null;
                if (VMP.SpecialistInspections.Count == 0)
                    VMP.SpecialistInspections = null;
            }
        }

        public static string Save()
        {
            return VMP.Save();
        }

        public static string Print(Settings settings)
        {

            List<SocialStatus> SocialStatuses;
            List<Policy> Policies;
            List<Disability> Disabilities;
            List<EyeSide> EyeSides;
 
            Directory.CreateDirectory(settings.SavePath);
            Object template = Template;
            Word.Application app = new Word.Application();
            app.Visible = true;
            try
            {
                Word.Document doc = app.Documents.Add(ref template, ref missingObj, ref missingObj, ref missingObj);

                using (MedicineContext db = new MedicineContext())
                {
                    db.SocialStatuses.Load();
                    db.Policies.Load();
                    db.Disabilities.Load();
                    db.EyeSides.Load();
                    SocialStatuses = db.SocialStatuses.Local.ToList();
                    Policies = db.Policies.Local.ToList();
                    Disabilities = db.Disabilities.Local.ToList();
                    EyeSides = db.EyeSides.Local.ToList();
                }
                
                //Общая информация о пациенте
                ReplaceText(app, "#Name", VMP.Patient.Name);
                ReplaceText(app, "#Birthday", VMP.Patient.Birthday.ToShortDateString());
                ReplaceText(app, "#Policy", Policies.FirstOrDefault(x => x.Id == VMP.Patient.PoliceId).Name);
                ReplaceText(app, "#PoliceNumber", VMP.Patient.PoliceNumber);
                ReplaceText(app, "#Address", VMP.Patient.Address);
                ReplaceText(app, "#Phone", VMP.Patient.Phone);
                ReplaceText(app, "#lgota", GetLgota(Disabilities));
                ReplaceText(app, "#SocialStatus", GetSocialStatus(SocialStatuses));

                //Основной анамнез
                ReplaceText(app, "#Complaints", VMP.GeneralHistory.Complaints);
                ReplaceText(app, "#DiseaseHistory", VMP.GeneralHistory.DiseaseHistory);
                ReplaceText(app, "#Operations ", string.IsNullOrEmpty(VMP.GeneralHistory.Operations) ? "отрицает" : VMP.GeneralHistory.Operations);
                ReplaceText(app, "#ChronicDiseases", string.IsNullOrEmpty(VMP.GeneralHistory.ChronicDiseases) ? "отрицает" : VMP.GeneralHistory.ChronicDiseases);
                ReplaceText(app, "#AllergyHistory", string.IsNullOrEmpty(VMP.GeneralHistory.AllergyHistory) ? "отрицает" : VMP.GeneralHistory.AllergyHistory);
            
                //Объективно
                GetObjectively(app, EyeSides);

                //Данные дополнительных исследований
                //Общий анализ крови

                if (VMP.AdditionalResearch.GeneralBloodAnalysis.Date != null)
                {
                    ReplaceText(app, "#GeneralBloodDate", VMP.AdditionalResearch.GeneralBloodAnalysis.Date?.ToShortDateString());
                    ReplaceText(app, "#Hb", VMP.AdditionalResearch.GeneralBloodAnalysis.Hb.ToString());
                    ReplaceText(app, "#E_C", VMP.AdditionalResearch.GeneralBloodAnalysis.E_C.ToString());
                    ReplaceText(app, "#CP", VMP.AdditionalResearch.GeneralBloodAnalysis.CP.ToString());
                    ReplaceText(app, "#L_C", VMP.AdditionalResearch.GeneralBloodAnalysis.L_C.ToString());
                    ReplaceText(app, "#T_C", VMP.AdditionalResearch.GeneralBloodAnalysis.T_C.ToString());
                    ReplaceText(app, "#P_YA", VMP.AdditionalResearch.GeneralBloodAnalysis.P_YA.ToString());
                    ReplaceText(app, "#C_YA", VMP.AdditionalResearch.GeneralBloodAnalysis.C_YA.ToString());
                    ReplaceText(app, "#E_O", VMP.AdditionalResearch.GeneralBloodAnalysis.E_O.ToString());
                    ReplaceText(app, "#L_F", VMP.AdditionalResearch.GeneralBloodAnalysis.L_F.ToString());
                    ReplaceText(app, "#MO", VMP.AdditionalResearch.GeneralBloodAnalysis.MO.ToString());
                    ReplaceText(app, "#COE ", VMP.AdditionalResearch.GeneralBloodAnalysis.COE.ToString());
                }
                else
                {
                    ClearText(doc, "GBA");
                }


                //Биохимический анализ крови
                if (VMP.AdditionalResearch.ChemistryBloodAnalysis.Date != null)
                {
                    ReplaceText(app, "#ChemistryBloodDate", VMP.AdditionalResearch.ChemistryBloodAnalysis.Date?.ToShortDateString());
                    ReplaceText(app, "#Creatinine", VMP.AdditionalResearch.ChemistryBloodAnalysis.Creatinine.ToString());
                    ReplaceText(app, "#ChemistryBloodUrea ", VMP.AdditionalResearch.ChemistryBloodAnalysis.Urea.ToString());
                    ReplaceText(app, "#CommonXC", VMP.AdditionalResearch.ChemistryBloodAnalysis.CommonXC.ToString());
                    ReplaceText(app, "#ChemistryBloodGlucose", VMP.AdditionalResearch.ChemistryBloodAnalysis.Glucose.ToString());
                    ReplaceText(app, "#TotalBilirubin", VMP.AdditionalResearch.ChemistryBloodAnalysis.TotalBilirubin.ToString());
                    ReplaceText(app, "#TotalProtein", VMP.AdditionalResearch.ChemistryBloodAnalysis.TotalProtein.ToString());
                    ReplaceText(app, "#Albumen", VMP.AdditionalResearch.ChemistryBloodAnalysis.Albumen.ToString());
                    ReplaceText(app, "#ALT", VMP.AdditionalResearch.ChemistryBloodAnalysis.ALT.ToString());
                    ReplaceText(app, "#ACT", VMP.AdditionalResearch.ChemistryBloodAnalysis.ACT.ToString());
                    ReplaceText(app, "#TG", VMP.AdditionalResearch.ChemistryBloodAnalysis.TG.ToString());
                    ReplaceText(app, "#LPVP", VMP.AdditionalResearch.ChemistryBloodAnalysis.LPVP.ToString());
                    ReplaceText(app, "#LPNP", VMP.AdditionalResearch.ChemistryBloodAnalysis.LPNP.ToString());
                }
                else
                {
                    ClearText(doc, "CBA");
                }

                //MP
                if (VMP.AdditionalResearch.MP.Date != null)
                {
                    ReplaceText(app, "#MPDate", VMP.AdditionalResearch.MP.Date?.ToShortDateString());
                    ReplaceText(app, "#MPResult", VMP.AdditionalResearch.MP.Result);
                }
                else
                {
                    ClearText(doc, "MP");
                }

                //Общий анализ мочи
                if (VMP.AdditionalResearch.GeneralUrineAnalysis.Date != null)
                {
                    ReplaceText(app, "#GeneralUrineDate", VMP.AdditionalResearch.GeneralUrineAnalysis.Date?.ToShortDateString());
                    ReplaceText(app, "#UrineColor", VMP.AdditionalResearch.GeneralUrineAnalysis.Color);
                    ReplaceText(app, "#UrineDensity", VMP.AdditionalResearch.GeneralUrineAnalysis.Density.ToString());
                    ReplaceText(app, "#UrineReaction", VMP.AdditionalResearch.GeneralUrineAnalysis.Reaction.ToLower());
                    ReplaceText(app, "#UrineProtein", GetUrineProtein());
                    ReplaceText(app, "#UrineGlucose", VMP.AdditionalResearch.GeneralUrineAnalysis.Glucose.ToLower());
                    ReplaceText(app, "#Urine_L_C", VMP.AdditionalResearch.GeneralUrineAnalysis.L_C.ToString());
                    ReplaceText(app, "#UrineEpithelialCells", VMP.AdditionalResearch.GeneralUrineAnalysis.EpithelialCells.ToString());
                    ReplaceText(app, "#UrineSlime", VMP.AdditionalResearch.GeneralUrineAnalysis.Slime.ToString());
                }
                else
                {
                    ClearText(doc, "GUA");
                }

                //Кал на я/г
                if (VMP.AdditionalResearch.Feces.Date != null)
                {
                    ReplaceText(app, "#FecesDate", VMP.AdditionalResearch.Feces.Date?.ToShortDateString());
                    ReplaceText(app, "#FecesResult", VMP.AdditionalResearch.Feces.Result);
                }
                else
                {
                    ClearText(doc, "FA");
                }

                //ЭКГ
                if (VMP.AdditionalResearch.ECG.Date != null)
                {
                    ReplaceText(app, "#ECGDate", VMP.AdditionalResearch.ECG.Date?.ToShortDateString());
                    ReplaceText(app, "#ECGRhythm", VMP.AdditionalResearch.ECG.Rhythm);
                    ReplaceText(app, "#ECGRate", VMP.AdditionalResearch.ECG.Rate.ToString());
                    ReplaceText(app, "#ECGAdditionalInfo", GetECGAdditionalInfo());
                }
                else
                {
                    ClearText(doc, "ECG");
                }
                    

                //Дополнительные исследования
                ReplaceList(app, "#AdditionalResearches", VMP.AdditionalResearch.OtherResearches);


                //Осмотр специалистов
                ReplaceList(app, "#SpecialistInspections", VMP.SpecialistInspections);

                //Диагноз
                ReplaceText(app, "#MKB", VMP.Diagnos.DiagnosisCode);
                ReplaceText(app, "#GeneralDiagnosis", GetDiagnos());
                ReplaceText(app, "#CompanionDiagnosis", VMP.Diagnos.Companion);
                ReplaceText(app, "#VMPFor", VMP.Diagnos.VMPFor);

                //Подписи
                ReplaceText(app, "#DoctorPosition", settings.DoctorPosition);
                ReplaceText(app, "#DoctorName", settings.DoctorName);
                ReplaceText(app, "#BossDoctorPosition", settings.BossDoctorPosition);
                ReplaceText(app, "#BossDoctorName", settings.BossDoctorName);

                string name = String.Format($"{VMP.Patient.Name}.docx");

                Object fileFormat = Word.WdSaveFormat.wdFormatDocumentDefault;
                Object fileName = settings.SavePath + @"\" + name;

            
                doc.SaveAs(fileName, fileFormat, falseObj, missingObj, falseObj, missingObj, falseObj, falseObj, falseObj, falseObj, missingObj, missingObj, missingObj, missingObj, missingObj, missingObj);
                ((Word._Document)doc).Close(ref saveChanges);
                app.Quit();
                return "ВМП сформирован";
            }
            catch (Exception ex)
            {
                app.Quit();
                return "Ошибка формирования ВМП: " + ex.Message;
            }
        }

        private static void ClearText(Word.Document doc, string bookmark)
        {
            Bookmark bm = doc.Bookmarks[bookmark];
            Range range = bm.Range;
            range.Text = "";
        }

        private static string GetDiagnos()
        {
            if (VMP.Diagnos.Complications != null)
                return $"{VMP.Diagnos.General}.\r\n{VMP.Diagnos.Complications}.";
            else
                return $"{VMP.Diagnos.General}.";
        }

       

        private static string GetECGAdditionalInfo()
        {
            return VMP.AdditionalResearch.ECG.AdditionalInfo == null ?
                string.Empty : $"{VMP.AdditionalResearch.ECG.AdditionalInfo}.";
        }

        private static string GetUrineProtein()
        {
            return VMP.AdditionalResearch.GeneralUrineAnalysis.Protein == "Отрицательный" ?
                VMP.AdditionalResearch.GeneralUrineAnalysis.Protein.ToLower() : $"{VMP.AdditionalResearch.GeneralUrineAnalysis.ProteinDetail}г.";
        }

        private static void GetObjectively(Word.Application app, List<EyeSide> eyeSides)
        {
            var eyeSideOD = eyeSides.FirstOrDefault(x => x.Side == "OD");
            var eyeSideOS = eyeSides.FirstOrDefault(x => x.Side == "OS");

            Eye eyeOD = VMP.Eyes.FirstOrDefault(x => x.EyeSideId == eyeSideOD.Id);
            Eye eyeOS = VMP.Eyes.FirstOrDefault(x => x.EyeSideId == eyeSideOS.Id);

            ReplaceText(app, "#VisusOD", $"={eyeOD.Visus}");
            ReplaceText(app, "#VisusOS", $"={eyeOS.Visus}");

            Word.Range range = app.ActiveDocument.Content;
            range.Find.Execute("#OU");

            if (eyeOD.CompareTo(eyeOS) == 0)
            {
                range.Text = GetOU(eyeOD);
            }
            else
            {
                range.Text = GetBoth(eyeOD, eyeOS);
            }

            ReplaceText(app, "#OcularFundusOD", eyeOD.OcularFundus);
            ReplaceText(app, "#OcularFundusOS", eyeOS.OcularFundus);
            ReplaceText(app, "#PressureOD", eyeOD.Pressure.ToString());
            ReplaceText(app, "#PressureOS", eyeOS.Pressure.ToString());
        }

        private static string GetBoth(Eye eyeOD, Eye eyeOS)
        {
            return $"OD: {GetEyeDescription(eyeOD)}\r\nOS: {GetEyeDescription(eyeOS)}";
        }

        private static string GetOU(Eye eye)
        {
            return $"OU: " + GetEyeDescription(eye);
        }

        
        
        private static string GetEyeDescription(Eye eye)
        {
            string result = "";

            result += "положение глазных яблок ";
            result += eye.EyeBallPosition == "Правильное" ? $"{eye.EyeBallPosition.ToLower()}, " : eye.EyeBallPositionDetail != null ? $"{eye.EyeBallPositionDetail.ToLower()}, " : " ";
            result += "движение ";
            result += eye.Motion == "В полном объеме" ? $"{eye.Motion.ToLower()}. " : eye.MotionDetail != null ? $"{eye.MotionDetail.ToLower()}. " : " ";
            result += "Глаза ";
            result += eye.Character == "Спокойны" ? $"{eye.Character.ToLower()}. " : eye.CharacterDetail != null ? $"{eye.CharacterDetail.ToLower()}. " : " ";
            result += "Веки ";
            result += eye.EyeLids == "Не изменены" ? $"{eye.EyeLids.ToLower()}. " : eye.EyeLidsDetail != null ? $"{eye.EyeLidsDetail.ToLower()}. " : " ";
            result += "Рост ресниц ";
            result += eye.EyeLashes == "Правильный" ? $"{eye.EyeLashes.ToLower()}. " : eye.EyeLashesDetail != null ? $"{eye.EyeLashesDetail.ToLower()}. " : " ";
            result += "При пальпации в проекции слезных мешочков ";
            result += eye.Palpation == "Отделяемого нет" ? $"патологического {eye.Palpation.ToLower()}. " : eye.PalpationDetail != null ? $"имеется {eye.PalpationDetail.ToLower()}. " : " ";
            result += eye.Conjunctiva == "Нет" ? "Конъюнктива не изменена. " : eye.ConjunctivaDetail != null ? $"имеется {eye.ConjunctivaDetail.ToLower()}. " : " ";
            result += "Роговица ";
            result += eye.Cornea == "Прозрачная" ? $"{eye.Cornea.ToLower()}. " : eye.CorneaDetail != null ? $"{eye.CorneaDetail.ToLower()}. " : " ";
            result += eye.FrontCamera != null ? $"Передняя камера {eye.FrontCamera.ToLower()}, " : " ";
            result += "влага ";
            result += eye.Moisture == "Чистая" ? $"{eye.Moisture.ToLower()}. " : eye.MoistureDetail != null ? $"{eye.MoistureDetail.ToLower()}. " : " ";
            result += eye.FrontCameraAngle != null ? $"Угол передней камеры {eye.FrontCameraAngle.ToLower()}. " : " ";
            result += "Радужка ";
            result += eye.Iris == "Прозрачная" ? $"{eye.Iris.ToLower()}. " : eye.IrisDetail != null ? $"{eye.IrisDetail.ToLower()}. " : " ";
            result += eye.Pupil != null ? eye.Pupil.ToLower().Contains("правильный") ? $"Зрачок {eye.Pupil.ToLower()}. " : $"{eye.Pupil}. " : " ";
            result += "Хрусталик ";
            result += eye.Chrystal == "Прозрачный" ? $"{eye.Chrystal.ToLower()}. " : eye.ChrystalDetail != null ? $"{eye.ChrystalDetail.ToLower()}. " : "";

            return result;
        }

        private static string GetSocialStatus(List<SocialStatus> socialStatuses)
        {
            string status = socialStatuses.FirstOrDefault(x => x.Id == VMP.Patient.SocialStatusId).Status;

            if (status == "Работающий")
            {
                return $"{status.ToLower()}, {VMP.Patient.JobAddress}."; 
            }
            else if (status == "Студент" || status == "Школьник")
            {
                return $"{status.ToLower()}, место учёбы {VMP.Patient.StudyAddress}.";
            }
            else
            {
                return status.ToLower();
            }
        }

        private static string GetLgota(List<Disability> disabilities)
        {
            string str = "";
            if (VMP.Patient.DisabilityId != null)
            {
                str = $"{disabilities.FirstOrDefault(x => x.Id == VMP.Patient.DisabilityId).Category}.";
            }
            if (VMP.Patient.VeteranWWII)
            {
                str += " Ветеран Великой Отечественной войны.";
            }
            if (string.IsNullOrEmpty(str))
                return "нет";
            else
                return str.TrimStart(' ');
        }

        private static void ReplaceList<T>(Word.Application app, string textToFind, ICollection<T> list) where T: IReporteble
        {
            Word.Range range = app.ActiveDocument.Content;
            range.Find.Execute(textToFind);

            string result = "";
            if (list.Count > 0)
            {
                foreach (var l in list)
                {
                    result += $"{l.Report()}\r\n";
                }
            }
            range.Text = result;
        }

        /// <summary>
        /// Метод для замены текста в шаблоне Word
        /// </summary>
        /// <param name="app">Ссылка на приложение Word</param>
        /// <param name="textToFind">Текст для поиска места вставки (якорь)</param>
        /// <param name="textToReplace">Текст для вставки</param>
        private static void ReplaceText(Word.Application app, string textToFind, string textToReplace)
        {
            Word.Range range = app.ActiveDocument.Content;
            range.Find.Execute(textToFind);
            range.Text = textToReplace;

            


            //Word.Find find = app.Selection.Find;
            //find.ClearFormatting();
            //find.Text = textToFind;
            //find.Replacement.ClearFormatting();
            //find.Replacement.Text = textToReplace;
            //Object wrap = Word.WdFindWrap.wdFindContinue;
            //Object replace = Word.WdReplace.wdReplaceAll;
            //find.Execute(missingObj, falseObj, falseObj, falseObj, missingObj, falseObj, trueObj, wrap, falseObj, missingObj, replace);
        }

        /*public static void SetPatient(Patient patient)
        {
            VMP.Patient = patient;
        }*/

    }
}
