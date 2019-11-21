using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MedGUI.Model
{
    public class GeneralHistory
    {
        public int Id { get; set; }
        public string Complaints { get; set; }
        public string DiseaseHistory { get; set; }
        public string Operations { get; set; }
        public string ChronicDiseases { get; set; }
        public string AllergyHistory { get; set; }

        public virtual ICollection<VMP> VMPs { get; set; }

        public void Copy(GeneralHistory g)
        {
            Complaints = g.Complaints;
            DiseaseHistory = g.DiseaseHistory;
            Operations = g.Operations;
            ChronicDiseases = g.ChronicDiseases;
            AllergyHistory = g.AllergyHistory;
        }
    }
}
