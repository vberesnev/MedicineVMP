using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MedGUI.Model.PatientModel
{
    public class Month
    {
        public int Id { get; set; }
        public string MonthName { get; set; }

        public Month(int id, string monthName)
        {
            Id = id;
            MonthName = monthName;
        }

        public override string ToString()
        {
            return MonthName;
        }
    }
}
