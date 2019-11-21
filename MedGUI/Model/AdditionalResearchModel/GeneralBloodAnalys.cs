using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MedGUI.Model.AdditionalResearchModel
{
    [Table("GeneralBloodAnalyses")]
    public class GeneralBloodAnalys
    {
        public int? Id { get; set; }
        public DateTime? Date { get; set; }
        public double Hb { get; set; }
        public double E_C { get; set; }
        public double CP { get; set; }
        public double L_C { get; set; }
        public double T_C { get; set; }
        public double P_YA { get; set; }
        public double C_YA { get; set; }
        public double E_O { get; set; }
        public double L_F { get; set; }
        public double MO { get; set; }
        public double COE { get; set; }

        public ICollection<AdditionalResearch> AdditionalResearches { get; set; }

        public void Copy(GeneralBloodAnalys g)
        {
            if (g == null) return;
            this.Date = g.Date;
            this.Hb = g.Hb;
            this.E_C = g.E_C;
            this.CP = g.CP;
            this.L_C = g.L_C;
            this.T_C = g.T_C;
            this.P_YA = g.P_YA;
            this.C_YA = g.C_YA;
            this.E_O = g.E_O;
            this.L_F = g.L_F;
            this.MO = g.MO;
            this.COE = g.COE;
        }
    }
}
