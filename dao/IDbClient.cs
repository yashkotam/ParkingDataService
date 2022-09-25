using System.Collections.Generic;

namespace ParkingDataServiceNameSpace
{
    public interface IdbClient
    {
        public ParkingUnit updateParkingUnit(ParkingUnit parkingUnit);
        public ParkingUnit getParkingUnit(string id);
        public List<ParkingVehicle> addParkingVehicles(List<ParkingVehicle> parkingVehicles);
        public List<string> deleteParkingVehicles(List<string> numbersList);
    }
}