using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MedGUI.Model.AdditionalResearchModel
{
    [Table("ECGs")]
    public class ECG
    {
        public int? Id { get; set; }
        public DateTime? Date { get; set; }
        public string Rhythm { get; set; }
        public int Rate { get; set; }
        public string AdditionalInfo { get; set; }

        public ICollection<AdditionalResearch> AdditionalResearches { get; set; }

        public void Copy(ECG e)
        {
            if (e == null) return;
            this.Date = e.Date;
            this.Rhythm = e.Rhythm;
            this.Rate = e.Rate;
            this.AdditionalInfo = e.AdditionalInfo;
        }
    }
}
