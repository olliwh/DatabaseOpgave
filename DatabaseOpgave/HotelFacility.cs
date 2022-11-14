using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DatabaseOpgave
{
    public class HotelFacility
    {
        public int HotelID { get; set; }
        public int FacilityID { get; set; }
        public string Floor { get; set; }
        public override string ToString()
        {
            return $"Hotel ID:{HotelID}, Facility ID: {FacilityID}, Etage: {Floor}";
        }
    }
}
