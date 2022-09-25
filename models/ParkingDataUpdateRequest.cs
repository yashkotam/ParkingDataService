using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace ParkingDataServiceNameSpace
{
    public class ParkingDataUpdateRequest
    {
        public string id { get; set; }
        public string[] numbersList { get; set; }
    }
}