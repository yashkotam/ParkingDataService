using Microsoft.Extensions.Logging;
using System.Collections.Generic;

namespace ParkingDataServiceNameSpace
{
    public class ParkingService
    {

        private readonly ILogger _logger;
        private IdbClient _dbClient;

        public ParkingService(ILogger<ParkingService> logger, IdbClient dbClient){
            this._logger = logger;
            this._dbClient = dbClient;        
        }     

        public void updateParkingUnit(string id, string[] numbersList){
            
            _logger.LogInformation("started updating parkingUnit : ");
            
            ParkingUnit parkingUnit = _dbClient.getParkingUnit(id);
            _logger.LogInformation("current parkingUnit : " + parkingUnit);

            string[] oldVehicles = parkingUnit.numbersList;
            List<string> oldVehiclesList = new List<string>();
            List<string> addVehiclesList = new List<string>();
            List<string> newVehiclesList = new List<string>(numbersList);
            if(oldVehicles!=null){
                oldVehiclesList = new List<string>(oldVehicles);
            }

            foreach(string num in newVehiclesList){
                if(oldVehiclesList.Contains(num)){
                    oldVehiclesList.Remove(num);
                }else{
                    addVehiclesList.Add(num);
                }
            }            

            int count = numbersList.Length;
            parkingUnit.count = count;
            parkingUnit.numbersList = numbersList;
        
            List<ParkingVehicle> parkingVehiclesList = new List<ParkingVehicle>();

            foreach(string numberId in addVehiclesList){
                ParkingVehicle parkingVehicle = new ParkingVehicle();
                parkingVehicle.id = numberId;
                parkingVehicle.building = parkingUnit.building;
                parkingVehicle.floor = parkingUnit.floor;
                parkingVehicle.start = parkingUnit.start;
                parkingVehicle.end = parkingUnit.end;
                parkingVehiclesList.Add(parkingVehicle);
            }

            _dbClient.updateParkingUnit(parkingUnit);
            _dbClient.deleteParkingVehicles(oldVehiclesList);
            _dbClient.addParkingVehicles(parkingVehiclesList);

            _logger.LogInformation("completed updating parkingUnit : ");
        }
    }
}