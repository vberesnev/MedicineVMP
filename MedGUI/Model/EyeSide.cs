using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MedGUI.Model
{
    public class  EyeSide
    {
        public int Id { get; set; }
        public string Side { get; set; }

        public virtual ICollection<Eye> Eyes { get; set; }
    }
}
