using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;
using System.Text;

namespace MedGUI.Model.PatientModel
{
    [Table("Disabilities")]
    public class Disability
    {
        public int? Id { get; set; }
        public string Category { get; set; }

        public ICollection<Patient> Patients { get; set; }
    }
}
