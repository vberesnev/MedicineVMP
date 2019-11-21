using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MedGUI.Model.AdditionalResearchModel
{
    [Table("AdditionalResearches")]
    public class AdditionalResearch
    {
        public int? Id { get; set; }
        public int? GeneralBloodAnalysisId { get; set; }
        [ForeignKey("GeneralBloodAnalysisId")]
        public GeneralBloodAnalys GeneralBloodAnalysis { get; set; }
        public int? ChemistryBloodAnalysisId { get; set; }
        [ForeignKey("ChemistryBloodAnalysisId")]
        public ChemistryBloodAnalys ChemistryBloodAnalysis { get; set; }
        public int? MPId { get; set; }
        [ForeignKey("MPId")]
        public MP MP { get; set; }
        public int? GeneralUrineAnalysisId { get; set; }
        [ForeignKey("GeneralUrineAnalysisId")]
        public GeneralUrineAnalys GeneralUrineAnalysis { get; set; }
        public int? FecesId { get; set; }
        [ForeignKey("FecesId")]
        public Feces Feces { get; set; }
        public int? ECGId { get; set; }
        [ForeignKey("ECGId")]
        public ECG ECG { get; set; }
        public virtual ICollection<OtherResearch> OtherResearches { get; set; }

        public virtual ICollection<VMP> VMPs { get; set; }

    }
}
