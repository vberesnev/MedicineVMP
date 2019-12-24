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
        public string Hb { get; set; }
        public string E_C { get; set; }
        public string CP { get; set; }
        public string L_C { get; set; }
        public string T_C { get; set; }
        public string P_YA { get; set; }
        public string C_YA { get; set; }
        public string E_O { get; set; }
        public string L_F { get; set; }
        public string MO { get; set; }
        public string COE { get; set; }

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

        public bool IsNull()
        {
            return Date == null & string.IsNullOrEmpty(Hb) & string.IsNullOrEmpty(E_C)
                                & string.IsNullOrEmpty(CP) & string.IsNullOrEmpty(L_C)
                                & string.IsNullOrEmpty(T_C) & string.IsNullOrEmpty(P_YA)
                                & string.IsNullOrEmpty(C_YA) & string.IsNullOrEmpty(E_O)
                                & string.IsNullOrEmpty(L_F) & string.IsNullOrEmpty(MO) & string.IsNullOrEmpty(COE);
        }
    }
}
