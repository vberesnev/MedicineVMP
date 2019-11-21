using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MedGUI.Model
{
    [Table("SpecialistInspections")]
    public class SpecialistInspection: IReporteble
    {
        public int Id { get; set; }
        [Column("VMPId"), ForeignKey("VMP")]
        public int VMPId { get; set; }
        public virtual VMP VMP { get; set; }
        public string Specialist { get; set; }
        public DateTime Date { get; set; }
        public string Result { get; set; }

        public SpecialistInspection() { }

        public string Report()
        {
            return $"{Specialist} от {Date.ToShortDateString()}: {Result}";
        }
    }
}
