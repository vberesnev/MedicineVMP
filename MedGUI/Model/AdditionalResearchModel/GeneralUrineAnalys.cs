using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MedGUI.Model.AdditionalResearchModel
{
    [Table("GeneralUrineAnalyses")]
    public class GeneralUrineAnalys
    {
        public int? Id { get; set; }
        public DateTime? Date { get; set; }
        public string Color { get; set; }
        public string Density { get; set; }
        public string Reaction { get; set; }
        public string Protein { get; set; }
        public string ProteinWeight { get; set; }
        public string Glucose { get; set; }
        public string L_C { get; set; }
        public string EpithelialCells { get; set; }
        public string Slime { get; set; }

        public ICollection<AdditionalResearch> AdditionalResearches { get; set; }

        public void Copy(GeneralUrineAnalys g)
        {
            if (g == null) return;
            this.Date = g.Date;
            this.Color = g.Color;
            this.Density = g.Density;
            this.Reaction = g.Reaction;
            this.Protein = g.Protein;
            this.ProteinWeight = g.ProteinWeight;
            this.Glucose = g.Glucose;
            this.L_C = g.L_C;
            this.EpithelialCells = g.EpithelialCells;
            this.Slime = g.Slime;
        }

        public bool IsNull()
        {
            return Date == null & string.IsNullOrEmpty(Color) & string.IsNullOrEmpty(Density)
                                & string.IsNullOrEmpty(Reaction) & string.IsNullOrEmpty(Protein)
                                & string.IsNullOrEmpty(ProteinWeight)
                                & string.IsNullOrEmpty(Glucose) & string.IsNullOrEmpty(L_C)
                                & string.IsNullOrEmpty(EpithelialCells) & string.IsNullOrEmpty(Slime);
        }
        
    }
}
