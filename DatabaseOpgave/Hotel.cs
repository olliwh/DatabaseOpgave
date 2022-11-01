using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DatabaseOpgave
{
    public class Hotel
    {
        public int HotelID { get; set; }
        public string Name { get; set; }

        public string Address { get; set; }

        public override string ToString()
        {
            return $"ID: {HotelID}, Name: {Name}, Address: {Address}";
        }
    }
}
