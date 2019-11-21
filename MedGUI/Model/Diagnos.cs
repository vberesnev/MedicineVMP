using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MedGUI.Model
{
    [Table("Diagnoses")]
    public class Diagnos
    {
        public int? Id { get; set; }
        public string DiagnosisCode { get; set; }
        public string General { get; set; }
        public string Complications { get; set; }
        public string Companion { get; set; }
        public string VMPFor { get; set; }

        public virtual ICollection<VMP> VMPs { get; set; }

        public void Copy(Diagnos d)
        {
            DiagnosisCode = d.DiagnosisCode;
            General = d.General;
            Complications = d.Complications;
            Companion = d.Companion;
            VMPFor = d.VMPFor;
        }
    }
}
