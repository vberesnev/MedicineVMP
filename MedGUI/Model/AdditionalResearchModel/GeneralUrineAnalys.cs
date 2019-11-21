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
        public double Density { get; set; }
        public string Reaction { get; set; }
        public string Protein { get; set; }
        public string ProteinDetail { get; set; }
        public double ProteinWeight { get; set; }
        public string Glucose { get; set; }
        public double L_C { get; set; }
        public double EpithelialCells { get; set; }
        public double Slime { get; set; }

        public ICollection<AdditionalResearch> AdditionalResearches { get; set; }

        public void Copy(GeneralUrineAnalys g)
        {
            if (g == null) return;
            this.Date = g.Date;
            this.Color = g.Color;
            this.Density = g.Density;
            this.Reaction = g.Reaction;
            this.Protein = g.Protein;
            this.ProteinDetail = g.ProteinDetail;
            this.ProteinWeight = g.ProteinWeight;
            this.Glucose = g.Glucose;
            this.L_C = g.L_C;
            this.EpithelialCells = g.EpithelialCells;
            this.Slime = g.Slime;
        }
    }
}
