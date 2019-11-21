using System;
using System.Collections.Generic;
using System.Data.Entity;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using MedGUI.Model.PatientModel;
using MedGUI.Model.AdditionalResearchModel;
using MedGUI.Model;

namespace MedGUI.Model
{
    public class MedicineContext: DbContext
    {
        public MedicineContext() : base("MedConnection") { }

        public DbSet<VMP> VMPs { get; set; }

        public DbSet<Patient> Patients { get; set; }
        public DbSet<Disability> Disabilities { get; set; }
        public DbSet<Policy> Policies { get; set; }
        public DbSet<SocialStatus> SocialStatuses { get; set; }

        public DbSet<GeneralHistory> GeneralHistories { get; set; }

        public DbSet<Eye> Eyes { get; set; }
        public DbSet<EyeSide> EyeSides { get; set; }

        public DbSet<AdditionalResearch> AdditionalResearches { get; set; }
        public DbSet<GeneralBloodAnalys> GeneralBloodAnalyses { get; set; }
        public DbSet<ChemistryBloodAnalys> ChemistryBloodAnalyses { get; set; }
        public DbSet<MP> MPs { get; set; }
        public DbSet<GeneralUrineAnalys> GeneralUrineAnalyses { get; set; }
        public DbSet<Feces> Feceses { get; set; }
        public DbSet<ECG> ECGs { get; set; }
        public DbSet<OtherResearch> OtherResearches { get; set; }

        public DbSet<SpecialistInspection> SpecialistInspections { get; set; }

        public DbSet<Diagnos> Diagnoses { get; set; }


    }
}
