using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MedGUI.Model
{
    public class EyeDescriptionMethod
    {
        public string Name { get; set; }
        public bool Both { get; set; }
        public string Value { get; set; }

        public EyeDescriptionMethod(string name, bool both, string value)
        {
            Name = name;
            Both = both;
            Value = value;
        }
    }
}
