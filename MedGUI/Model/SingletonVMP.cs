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

                if (VMP.AdditionalResearch.GeneralBloodAnalysis.IsNull())
                {
                    ClearText(doc, "GBA");
                }
                else
                {
                    ReplaceText(app, "#GeneralBloodDate", VMP.AdditionalResearch.GeneralBloodAnalysis.Date == null ? ": " : $" от {VMP.AdditionalResearch.GeneralBloodAnalysis.Date?.ToShortDateString()}: ");
                    ReplaceText(app, "#Hb", string.IsNullOrEmpty(VMP.AdditionalResearch.GeneralBloodAnalysis.Hb) ? "" : $"Hb {VMP.AdditionalResearch.GeneralBloodAnalysis.Hb}; ");
                    ReplaceText(app, "#E_C", string.IsNullOrEmpty(VMP.AdditionalResearch.GeneralBloodAnalysis.E_C) ? "" : $"э/ц {VMP.AdditionalResearch.GeneralBloodAnalysis.E_C}х10^12/л; ");
                    ReplaceText(app, "#CP", string.IsNullOrEmpty(VMP.AdditionalResearch.GeneralBloodAnalysis.CP) ? "" : $"ЦП {VMP.AdditionalResearch.GeneralBloodAnalysis.CP}; ");
                    ReplaceText(app, "#L_C", string.IsNullOrEmpty(VMP.AdditionalResearch.GeneralBloodAnalysis.L_C) ? "" : $"л/ц {VMP.AdditionalResearch.GeneralBloodAnalysis.L_C}х10^9/л; ");
                    ReplaceText(app, "#T_C", string.IsNullOrEmpty(VMP.AdditionalResearch.GeneralBloodAnalysis.T_C) ? "" : $"т/ц {VMP.AdditionalResearch.GeneralBloodAnalysis.T_C}х10^9/л; ");
                    ReplaceText(app, "#P_YA", string.IsNullOrEmpty(VMP.AdditionalResearch.GeneralBloodAnalysis.P_YA) ? "" : $"п/я {VMP.AdditionalResearch.GeneralBloodAnalysis.P_YA}%; ");
                    ReplaceText(app, "#C_YA", string.IsNullOrEmpty(VMP.AdditionalResearch.GeneralBloodAnalysis.C_YA) ? "" : $"с/я {VMP.AdditionalResearch.GeneralBloodAnalysis.C_YA}%; ");
                    ReplaceText(app, "#E_O", string.IsNullOrEmpty(VMP.AdditionalResearch.GeneralBloodAnalysis.E_O) ? "" : $"э/о {VMP.AdditionalResearch.GeneralBloodAnalysis.E_O}%; ");
                    ReplaceText(app, "#L_F", string.IsNullOrEmpty(VMP.AdditionalResearch.GeneralBloodAnalysis.L_F) ? "" : $"л/ф {VMP.AdditionalResearch.GeneralBloodAnalysis.L_F}%; ");
                    ReplaceText(app, "#MO", string.IsNullOrEmpty(VMP.AdditionalResearch.GeneralBloodAnalysis.MO) ? "" : $"мо {VMP.AdditionalResearch.GeneralBloodAnalysis.MO}%; ");
                    ReplaceText(app, "#COE", string.IsNullOrEmpty(VMP.AdditionalResearch.GeneralBloodAnalysis.COE) ? "" : $"СОЭ {VMP.AdditionalResearch.GeneralBloodAnalysis.COE}мм/ч; ");
                }


                //Биохимический анализ крови
                if (VMP.AdditionalResearch.ChemistryBloodAnalysis.IsNull())
                {
                    ClearText(doc, "CBA");
                }
                else
                {
                    ReplaceText(app, "#ChemistryBloodDate", VMP.AdditionalResearch.ChemistryBloodAnalysis.Date == null ? ": " : $" от {VMP.AdditionalResearch.ChemistryBloodAnalysis.Date?.ToShortDateString()}: ");
                    ReplaceText(app, "#Creatinine", string.IsNullOrEmpty(VMP.AdditionalResearch.ChemistryBloodAnalysis.Creatinine) ? "" : $"креатинин {VMP.AdditionalResearch.ChemistryBloodAnalysis.Creatinine} ммоль/л; ");
                    ReplaceText(app, "#ChemistryBloodUrea", string.IsNullOrEmpty(VMP.AdditionalResearch.ChemistryBloodAnalysis.Urea) ? "" : $"мочевина {VMP.AdditionalResearch.ChemistryBloodAnalysis.Urea} ммоль/л; ");
                    ReplaceText(app, "#CommonXC", string.IsNullOrEmpty(VMP.AdditionalResearch.ChemistryBloodAnalysis.CommonXC) ? "" : $"общих ХС {VMP.AdditionalResearch.ChemistryBloodAnalysis.CommonXC} ммоль/л; ");
                    ReplaceText(app, "#ChemistryBloodGlucose", string.IsNullOrEmpty(VMP.AdditionalResearch.ChemistryBloodAnalysis.Glucose) ? "" : $"глюкоза {VMP.AdditionalResearch.ChemistryBloodAnalysis.Glucose} ммоль/л; ");
                    ReplaceText(app, "#TotalBilirubin", string.IsNullOrEmpty(VMP.AdditionalResearch.ChemistryBloodAnalysis.TotalBilirubin) ? "" : $"общий билирубин {VMP.AdditionalResearch.ChemistryBloodAnalysis.TotalBilirubin} ммоль/л; ");
                    ReplaceText(app, "#TotalProtein", string.IsNullOrEmpty(VMP.AdditionalResearch.ChemistryBloodAnalysis.TotalProtein) ? "" : $"общий белок {VMP.AdditionalResearch.ChemistryBloodAnalysis.TotalProtein} г/л; ");
                    ReplaceText(app, "#Albumen", string.IsNullOrEmpty(VMP.AdditionalResearch.ChemistryBloodAnalysis.Albumen) ? "" : $"альбумин {VMP.AdditionalResearch.ChemistryBloodAnalysis.Albumen} г/л; ");
                    ReplaceText(app, "#ALT", string.IsNullOrEmpty(VMP.AdditionalResearch.ChemistryBloodAnalysis.ALT) ? "" : $"АЛТ {VMP.AdditionalResearch.ChemistryBloodAnalysis.ALT} Ед; ");
                    ReplaceText(app, "#ACT", string.IsNullOrEmpty(VMP.AdditionalResearch.ChemistryBloodAnalysis.ACT) ? "" : $"АСТ {VMP.AdditionalResearch.ChemistryBloodAnalysis.ACT} Ед; ");
                    ReplaceText(app, "#TG", string.IsNullOrEmpty(VMP.AdditionalResearch.ChemistryBloodAnalysis.TG) ? "" : $"ТГ {VMP.AdditionalResearch.ChemistryBloodAnalysis.TG} мкмоль/л; ");
                    ReplaceText(app, "#LPVP", string.IsNullOrEmpty(VMP.AdditionalResearch.ChemistryBloodAnalysis.LPVP) ? "" : $"ЛПВП {VMP.AdditionalResearch.ChemistryBloodAnalysis.LPVP} ммоль/л; ");
                    ReplaceText(app, "#LPNP", string.IsNullOrEmpty(VMP.AdditionalResearch.ChemistryBloodAnalysis.LPNP) ? "" : $"ЛПНП {VMP.AdditionalResearch.ChemistryBloodAnalysis.LPNP} ммоль/л; ");
                }

                //MP
                if (VMP.AdditionalResearch.MP.IsNull())
                {
                    ClearText(doc, "MP");
                    
                }
                else
                {
                    ReplaceText(app, "#MPDate", VMP.AdditionalResearch.MP.Date == null ? ": " : $" от {VMP.AdditionalResearch.MP.Date?.ToShortDateString()}: ");
                    ReplaceText(app, "#MPResult", string.IsNullOrEmpty(VMP.AdditionalResearch.MP.Result) ? "" : $" {VMP.AdditionalResearch.MP.Result} результат; ");
                }

                //Общий анализ мочи
                if (VMP.AdditionalResearch.GeneralUrineAnalysis.IsNull())
                {
                    ClearText(doc, "GUA");
                }
                else
                {
                    ReplaceText(app, "#GeneralUrineDate", VMP.AdditionalResearch.GeneralUrineAnalysis.Date == null ? ": " : $" от {VMP.AdditionalResearch.GeneralUrineAnalysis.Date?.ToShortDateString()}: ");
                    ReplaceText(app, "#UrineColor", string.IsNullOrEmpty(VMP.AdditionalResearch.GeneralUrineAnalysis.Color) ? "" : $"цвет {VMP.AdditionalResearch.GeneralUrineAnalysis.Color.ToLower()}, ");
                    ReplaceText(app, "#UrineDensity", string.IsNullOrEmpty(VMP.AdditionalResearch.GeneralUrineAnalysis.Density) ? "" : $"плотность {VMP.AdditionalResearch.GeneralUrineAnalysis.Density.ToLower()}, ");
                    ReplaceText(app, "#UrineReaction", string.IsNullOrEmpty(VMP.AdditionalResearch.GeneralUrineAnalysis.Reaction) ? "" : $"реакция {VMP.AdditionalResearch.GeneralUrineAnalysis.Reaction.ToLower()}, ");
                    ReplaceText(app, "#UrineProtein", (string.IsNullOrEmpty(VMP.AdditionalResearch.GeneralUrineAnalysis.Protein) && string.IsNullOrEmpty(VMP.AdditionalResearch.GeneralUrineAnalysis.ProteinWeight)) ? "" : $"{GetUrineProtein()}, ");
                    ReplaceText(app, "#UrineGlucose", string.IsNullOrEmpty(VMP.AdditionalResearch.GeneralUrineAnalysis.Glucose) ? "" : $"глюкоза {VMP.AdditionalResearch.GeneralUrineAnalysis.Glucose.ToLower()}, ");
                    ReplaceText(app, "#Urine_L_C", string.IsNullOrEmpty(VMP.AdditionalResearch.GeneralUrineAnalysis.L_C) ? "" : $"л/ц {VMP.AdditionalResearch.GeneralUrineAnalysis.L_C} в п.зр., ");
                    ReplaceText(app, "#UrineEpithelialCells", string.IsNullOrEmpty(VMP.AdditionalResearch.GeneralUrineAnalysis.EpithelialCells) ? "" : $"эпителиальные клетки {VMP.AdditionalResearch.GeneralUrineAnalysis.EpithelialCells}, ");
                    ReplaceText(app, "#UrineSlime", string.IsNullOrEmpty(VMP.AdditionalResearch.GeneralUrineAnalysis.Slime) ? "" : $"слизь {VMP.AdditionalResearch.GeneralUrineAnalysis.Slime.ToLower()}");
                }

                //Кал на я/г
                if (VMP.AdditionalResearch.Feces.IsNull())
                {
                    ClearText(doc, "FA");
                }
                else
                {
                    ReplaceText(app, "#FecesDate", VMP.AdditionalResearch.Feces.Date == null ? ": " : $" от {VMP.AdditionalResearch.Feces.Date?.ToShortDateString()}: ");
                    ReplaceText(app, "#FecesResult", string.IsNullOrEmpty(VMP.AdditionalResearch.Feces.Result) ? "" : $"{VMP.AdditionalResearch.GeneralUrineAnalysis.Color}.");
                }

                //ЭКГ
                if (VMP.AdditionalResearch.ECG.IsNull())
                {
                    ClearText(doc, "ECG");
                }
                else
                {
                    ReplaceText(app, "#ECGDate", VMP.AdditionalResearch.ECG.Date == null ? ": " : $" от {VMP.AdditionalResearch.ECG.Date?.ToShortDateString()}: ");
                    ReplaceText(app, "#ECGRhythm", string.IsNullOrEmpty(VMP.AdditionalResearch.ECG.Rhythm) ? "" : $"ритм {VMP.AdditionalResearch.ECG.Rhythm.ToLower()}, ");
                    ReplaceText(app, "#ECGRate", VMP.AdditionalResearch.ECG.Rate == 0 ? "" : $"ЧСС {VMP.AdditionalResearch.ECG.Rate} уд/ мин. ");
                    ReplaceText(app, "#ECGAdditionalInfo", string.IsNullOrEmpty(VMP.AdditionalResearch.ECG.AdditionalInfo) ? "" : $"{VMP.AdditionalResearch.ECG.AdditionalInfo}");
                }
                    

                //Дополнительные исследования
                if (VMP.AdditionalResearch.OtherResearches != null)
                    ReplaceList(app, "#AdditionalResearches", VMP.AdditionalResearch.OtherResearches);


                //Осмотр специалистов
                if (VMP.SpecialistInspections != null)
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
                $"белок отрицательный" : $"белок {VMP.AdditionalResearch.GeneralUrineAnalysis.ProteinWeight}г.";
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
