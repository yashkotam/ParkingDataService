using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace ParkingDataServiceNameSpace
{
    public class ParkingUnit
    {
        public string id { get; set; }
        public string[] numbersList { get; set; }
        public string building { get; set; }
        public int floor { get; set; }
        public string start { get; set; }
        public string end { get; set; }
        public int count { get; set; }
        public int capacity { get; set; }

        public override string ToString()
        {
            return "id: " + id + " building:" + building + " floor:" + floor ;
        }
    }
}