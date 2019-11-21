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
        public double Creatinine { get; set; }
        public double Urea { get; set; }
        public double CommonXC { get; set; }
        public double Glucose { get; set; }
        public double TotalBilirubin { get; set; }
        public double TotalProtein { get; set; }
        public double Albumen { get; set; }
        public double ALT { get; set; }
        public double ACT { get; set; }
        public double TG { get; set; }
        public double LPVP { get; set; }
        public double LPNP { get; set; }

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
    }
}
