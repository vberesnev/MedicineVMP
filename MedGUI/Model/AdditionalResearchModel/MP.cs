using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MedGUI.Model.AdditionalResearchModel
{
    [Table("MPs")]
    public class MP
    {
        public int? Id { get; set; }
        public DateTime? Date { get; set; }
        public string Result { get; set; }

        public virtual ICollection<AdditionalResearch> AdditionalResearches { get; set; }

        public void Copy(MP m)
        {
            if (m == null) return;
            this.Date = m.Date;
            this.Result = m.Result;
        }

        public bool IsNull()
        {
            return Date == null & string.IsNullOrEmpty(Result);
        }
    }
}
