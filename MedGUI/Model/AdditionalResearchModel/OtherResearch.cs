using System;
using System.Collections;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MedGUI.Model.AdditionalResearchModel
{
    public class OtherResearch: IReporteble
    {
        public int Id { get; set; }
        [Column("AdditionalResearchId")]
        public int AdditionalResearchId { get; set; }
        public AdditionalResearch AdditionalResearch { get; set; }
        public string Research { get; set; }
        public DateTime Date { get; set; }
        public string Result { get; set; }

        public OtherResearch() { }

        public string Report()
        {
            return $"{Research} от {Date.ToShortDateString()}: {Result}";
        }
    }
}
