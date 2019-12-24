using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MedGUI.Model.AdditionalResearchModel
{
    [Table("Feceses")]
    public class Feces
    {
        public int? Id { get; set; }
        public DateTime? Date { get; set; }
        public string Result { get; set; }

        public ICollection<AdditionalResearch> AdditionalResearches { get; set; }

        public void Copy(Feces f)
        {
            if (f == null) return;
            this.Date = f.Date;
            this.Result = f.Result;
        }

        public bool IsNull()
        {
            return Date == null && string.IsNullOrEmpty(Result);
        }
    }
}
