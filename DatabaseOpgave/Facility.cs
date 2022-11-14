using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DatabaseOpgave
{
    public class Facility
    {
     
        public int FacilityID { get; set; }

        public string Name { get; set; }

        public string Type { get; set; }

        public override string ToString()
        {
            return $"ID: {FacilityID}, Name: {Name}, Type: {Type}";
        }
    }
}
