using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;
using System.Text;

namespace MedGUI.Model.PatientModel
{
    [Table("Patients")]
    public class Patient
    {
        public int? Id { get; set; }
        public string Name { get; set; }
        public DateTime Birthday { get; set; }
        [Column("PoliceId"), ForeignKey("Policy")]
        public int? PoliceId { get; set; }
        public virtual Policy Policy { get; set; }
        public string PoliceNumber { get; set; }
        public string Address { get; set; }
        public string Phone { get; set; }
        public int? SocialStatusId { get; set; }
        public virtual SocialStatus SocialStatus { get; set; }
        public string JobAddress { get; set; }
        public string StudyAddress { get; set; }
        public bool Concession { get; set; }
        public int? DisabilityId { get; set; }
        public virtual Disability Disability { get; set; }
        public bool VeteranWWII { get; set; }

        public virtual ICollection<VMP> VMPs { get; set; }

        public void Copy(Patient p)
        {
            Name = p.Name;
            Birthday = p.Birthday;
            PoliceId = p.PoliceId;
            PoliceNumber = p.PoliceNumber;
            Address = p.Address;
            Phone = p.Phone;
            SocialStatusId = p.SocialStatusId;
            JobAddress = p.JobAddress;
            StudyAddress = p.StudyAddress;
            Concession = p.Concession;
            DisabilityId = p.DisabilityId;
            VeteranWWII = p.VeteranWWII;
        }

        
    }
}
