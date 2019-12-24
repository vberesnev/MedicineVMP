using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using MedGUI.Model.PatientModel;
using MedGUI.Model.AdditionalResearchModel;
using System.ComponentModel.DataAnnotations.Schema;
using System.Data.Entity;
using System.ComponentModel.DataAnnotations;

namespace MedGUI.Model
{
    public class VMP
    {
        //Класс ВМП
        public int Id { get; set; }
        public DateTime CreationDate { get; set; }
        [ForeignKey("Patient")]
        public int? PatientId { get; set; }
        public virtual Patient Patient { get; set; }
        public int? GeneralHistoryId { get; set; }
        public GeneralHistory GeneralHistory { get; set; }
        public ICollection<Eye> Eyes { get; set; }
        public int? AdditionalResearchId { get; set; }
        public AdditionalResearch AdditionalResearch { get; set; }
        public ICollection<SpecialistInspection> SpecialistInspections { get; set; }
        [ForeignKey("Diagnos")]
        public int? DiagnosId { get; set; }
        public virtual Diagnos Diagnos { get; set; }

        public VMP() { }

        internal string Save()
        {
            try
            {
                using (MedicineContext db = new MedicineContext())
                {
                    if (this.Id == 0)
                    {
                        //Если у направления нет ID, значит оно новое - создаем новое направление
                        this.CreationDate = DateTime.Now.Date;
                        db.VMPs.Add(this);
                    }
                    else
                    {
                        //Если ID есть, загружаем информацию о направлении из базы для ее изменения
                        var vmp = db.VMPs.Include(x => x.Patient)
                                         .Include(x => x.GeneralHistory)
                                         .Include(x => x.Eyes)
                                         .Include(x => x.AdditionalResearch)
                                         .Include(x => x.SpecialistInspections)
                                         .Include(x => x.Diagnos)
                                         .FirstOrDefault(x => x.Id == this.Id);

                        if (this.Patient != null)
                        {
                            if (vmp.PatientId == null)
                            {
                                db.Patients.Add(this.Patient);
                                db.SaveChanges();
                                vmp.PatientId = this.Patient.Id;
                                this.PatientId = this.Patient.Id;
                            }
                            else
                            {
                                var patient = db.Patients.Find(vmp.PatientId);
                                patient.Copy(this.Patient);
                            }
                        }

                        if (this.GeneralHistory != null)
                        {
                            if (vmp.GeneralHistoryId == null)
                            {
                                db.GeneralHistories.Add(this.GeneralHistory);
                                db.SaveChanges();
                                vmp.GeneralHistoryId = this.GeneralHistory.Id;
                                this.GeneralHistoryId = this.GeneralHistory.Id;
                            }
                            else
                            {
                                var generalHistory = db.GeneralHistories.Find(vmp.GeneralHistoryId);
                                generalHistory.Copy(this.GeneralHistory);
                            }
                        }

                        if(this.Eyes != null)
                        {
                            EyeSide OD = db.EyeSides.FirstOrDefault(x => x.Side == "OD");
                            EyeSide OS = db.EyeSides.FirstOrDefault(x => x.Side == "OS");
                            if (vmp.Eyes.Count == 0)
                            {
                                foreach (var eye in this.Eyes)
                                {
                                    eye.VMPId = this.Id;
                                }
                                db.Eyes.Add(this.Eyes.FirstOrDefault(x => x.EyeSideId == OD.Id));
                                db.Eyes.Add(this.Eyes.FirstOrDefault(x => x.EyeSideId == OS.Id));
                                db.SaveChanges();
                            }
                            else
                            {
                                Eye eyeOD = db.Eyes.FirstOrDefault(x => x.VMPId == this.Id && x.EyeSideId == OD.Id);
                                Eye eyeOS = db.Eyes.FirstOrDefault(x => x.VMPId == this.Id && x.EyeSideId == OS.Id);

                                eyeOD.Copy(this.Eyes.FirstOrDefault(x => x.EyeSideId == OD.Id));
                                eyeOS.Copy(this.Eyes.FirstOrDefault(x => x.EyeSideId == OS.Id));
                            }
                        }

                        if (this.AdditionalResearch != null)
                        {
                            if (vmp.AdditionalResearchId == null)
                            {
                                db.AdditionalResearches.Add(this.AdditionalResearch);
                                db.SaveChanges();
                                vmp.AdditionalResearchId = this.AdditionalResearch.Id;
                                this.AdditionalResearchId = this.AdditionalResearch.Id;
                            }
                            else
                            {
                                var generalBloodAnalys = db.GeneralBloodAnalyses.Find(vmp.AdditionalResearch.GeneralBloodAnalysisId);
                                generalBloodAnalys.Copy(this.AdditionalResearch.GeneralBloodAnalysis);
                                var chemistryBloodAnalys = db.ChemistryBloodAnalyses.Find(vmp.AdditionalResearch.ChemistryBloodAnalysisId);
                                chemistryBloodAnalys.Copy(this.AdditionalResearch.ChemistryBloodAnalysis);
                                var mp = db.MPs.Find(vmp.AdditionalResearch.MPId);
                                mp.Copy(this.AdditionalResearch.MP);
                                var generalUrineAnalys = db.GeneralUrineAnalyses.Find(vmp.AdditionalResearch.GeneralUrineAnalysisId);
                                generalUrineAnalys.Copy(this.AdditionalResearch.GeneralUrineAnalysis);
                                var feces = db.Feceses.Find(vmp.AdditionalResearch.FecesId);
                                feces.Copy(this.AdditionalResearch.Feces);
                                var ecg = db.ECGs.Find(vmp.AdditionalResearch.ECGId);
                                ecg.Copy(this.AdditionalResearch.ECG);

                                if (this.AdditionalResearch.OtherResearches != null)
                                {
                                    db.Database.ExecuteSqlCommand($"DELETE FROM OtherResearches WHERE AdditionalResearchId = {vmp.AdditionalResearch.Id}");

                                    foreach (var or in this.AdditionalResearch.OtherResearches)
                                    {
                                        db.Database.ExecuteSqlCommand($"INSERT INTO OtherResearches (AdditionalResearchId, Research, Date, Result) " +
                                                                      $"VALUES ({vmp.AdditionalResearch.Id}, '{or.Research}', '{or.Date?.ToString("yyyy-MM-dd 00:00:00")}', '{or.Result}')");
                                    }
                                }
                            }
                        }

                        if (this.SpecialistInspections != null)
                        {
                            db.Database.ExecuteSqlCommand($"DELETE FROM SpecialistInspections WHERE VMPId = {this.Id}");
                            foreach(var si in this.SpecialistInspections)
                            {
                                db.Database.ExecuteSqlCommand($"INSERT INTO SpecialistInspections (VMPId, Specialist, Date, Result) " +
                                                              $"VALUES ({this.Id}, '{si.Specialist}', '{si.Date.ToString("yyyy-MM-dd 00:00:00")}', '{si.Result}')");
                            }
                        }

                        if (this.Diagnos != null)
                        {
                            if (vmp.DiagnosId == null)
                            {
                                db.Diagnoses.Add(this.Diagnos);
                                db.SaveChanges();
                                vmp.DiagnosId = this.Diagnos.Id;
                                this.DiagnosId = this.Diagnos.Id;
                            }
                            else
                            {
                                var diagnos = db.Diagnoses.Find(vmp.DiagnosId);
                                diagnos.Copy(this.Diagnos);
                            }
                        }
                    }

                    db.SaveChanges();
                }
                return "Направление успешно сохранено!";
            }
            catch(Exception ex)
            {
                return $"Ошибка сохранения: {ex.Message}";
            }
            
        }

        
    }
}
