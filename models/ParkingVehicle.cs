using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace ParkingDataServiceNameSpace
{
    public class ParkingVehicle
    {
        public string id { get; set; }
        public string building { get; set; }
        public int floor { get; set; }
        public string start { get; set; }
        public string end { get; set; }

        public override string ToString()
        {
            return "id: " + id + " building:" + building + " floor:" + floor ;
        }
    }
}