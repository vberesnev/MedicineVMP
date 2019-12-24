using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MedGUI.Model.AdditionalResearchModel
{
    [Table("ChemistryBloodAnalyses")]
    public class ChemistryBloodAnalys
    {
        public int? Id { get; set; }
        public DateTime? Date { get; set; }
        public string Creatinine { get; set; }
        public string Urea { get; set; }
        public string CommonXC { get; set; }
        public string Glucose { get; set; }
        public string TotalBilirubin { get; set; }
        public string TotalProtein { get; set; }
        public string Albumen { get; set; }
        public string ALT { get; set; }
        public string ACT { get; set; }
        public string TG { get; set; }
        public string LPVP { get; set; }
        public string LPNP { get; set; }

        public virtual ICollection<AdditionalResearch> AdditionalResearches { get; set; }

        public void Copy(ChemistryBloodAnalys c)
        {
            if (c == null) return;
            this.Date = c.Date;
            this.Creatinine = c.Creatinine;
            this.Urea = c.Urea;
            this.CommonXC = c.CommonXC;
            this.Glucose = c.Glucose;
            this.TotalBilirubin = c.TotalBilirubin;
            this.TotalProtein = c.TotalProtein;
            this.Albumen = c.Albumen;
            this.ALT = c.ALT;
            this.ACT = c.ACT;
            this.TG = c.TG;
            this.LPVP = c.LPVP;
            this.LPNP = c.LPNP;
        }

        public bool IsNull()
        {
            return Date == null & string.IsNullOrEmpty(Creatinine) & string.IsNullOrEmpty(Urea)
                                & string.IsNullOrEmpty(CommonXC) & string.IsNullOrEmpty(Glucose)
                                & string.IsNullOrEmpty(TotalBilirubin) & string.IsNullOrEmpty(TotalProtein)
                                & string.IsNullOrEmpty(Albumen) & string.IsNullOrEmpty(ALT)
                                & string.IsNullOrEmpty(ACT) & string.IsNullOrEmpty(TG) 
                                & string.IsNullOrEmpty(LPVP) & string.IsNullOrEmpty(LPNP);
        }
    }
}
