using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;
using System.Text;

namespace MedGUI.Model.PatientModel
{
    [Table("Policies")]
    public class Policy
    {
        public int? Id { get; set; }
        public string Name { get; set; }

        public ICollection<Patient> Patients { get; set; }

        public override string ToString()
        {
            return Name;
        }
    }
}
