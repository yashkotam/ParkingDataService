using Microsoft.Azure.Cosmos;
using Microsoft.Extensions.Logging;
using System.Threading.Tasks;
using System.Collections.Generic;
using System;

namespace ParkingDataServiceNameSpace
{
    public class CosmosDbClient : IdbClient
    {

        private static readonly string DB_NAME = "parkingdata";
        private static readonly string PARKING_UNITS_CONTAINER = "parkingunits";
        private static readonly string PARKING_VEHICLES_CONTAINER = "parkingvehicles";
        private readonly ILogger _logger;
        private CosmosClient _cosmosClient;
        private Database _database;
        private Container _parkingUnitContainer;
        private Container _parkingVehicleContainer;

        public CosmosDbClient(ILogger<CosmosDbClient> logger, CosmosClient cosmosClient){
            this._logger = logger;
            this._cosmosClient = cosmosClient;     
            this._database = _cosmosClient.GetDatabase(DB_NAME);
            this._parkingUnitContainer = _database.GetContainer(PARKING_UNITS_CONTAINER);
            this._parkingVehicleContainer = _database.GetContainer(PARKING_VEHICLES_CONTAINER);   
        }     

        public ParkingUnit updateParkingUnit(ParkingUnit parkingUnit){
            _logger.LogInformation("calling cosmos to update ParkingUnit : "+parkingUnit);
            Task<ItemResponse<ParkingUnit>> task =_parkingUnitContainer.ReplaceItemAsync<ParkingUnit>(parkingUnit, parkingUnit.id, new PartitionKey(parkingUnit.id));
            ParkingUnit parkingUnitUpdated = task.Result.Resource;
            _logger.LogInformation("completed cosmos to update ParkingUnit : "+parkingUnit);
            return parkingUnitUpdated;
        }

        public ParkingUnit  getParkingUnit(string id){
            _logger.LogInformation("calling cosmos to get ParkingUnit for id : "+id);
            Task<ParkingUnit> task = getParkingUnitAsync(id);
            ParkingUnit p = task.Result;
            _logger.LogInformation("got response from cosmos for id : "+id);
            return p;
        }
        public async Task<ParkingUnit> getParkingUnitAsync(string id){
            ItemResponse<ParkingUnit> response = await _parkingUnitContainer.ReadItemAsync<ParkingUnit>(id, new PartitionKey(id)); 
            return response.Resource;
        }
        public List<ParkingVehicle> addParkingVehicles(List<ParkingVehicle> parkingVehiclesList){
            List<ParkingVehicle> resList = new List<ParkingVehicle>();
            List<Task<ItemResponse<ParkingVehicle>>> taskList = new List<Task<ItemResponse<ParkingVehicle>>>();
            foreach(ParkingVehicle parkingVehicle in parkingVehiclesList){
                _logger.LogInformation("calling cosmos to add ParkingVehicle : "+parkingVehicle);
                Task<ItemResponse<ParkingVehicle>> task = _parkingVehicleContainer.CreateItemAsync<ParkingVehicle>(parkingVehicle, new PartitionKey(parkingVehicle.id));
                taskList.Add(task);
            }
            foreach(Task<ItemResponse<ParkingVehicle>> task in taskList){
                ParkingVehicle parkingVehicle = task.Result.Resource;
                _logger.LogInformation("completed cosmos to add ParkingVehicle : "+parkingVehicle);
                resList.Add(parkingVehicle);
            }
            return resList;
        }

        public List<string> deleteParkingVehicles(List<string> numbersList){
            List<string> resList = new List<string>();
            List<Task<ItemResponse<ParkingVehicle>>> taskList = new List<Task<ItemResponse<ParkingVehicle>>>();
            foreach(string numberId in numbersList){
                _logger.LogInformation("calling cosmos to delete ParkingVehicle Id: "+numberId);
                Task<ItemResponse<ParkingVehicle>> task = _parkingVehicleContainer.DeleteItemAsync<ParkingVehicle>(numberId, new PartitionKey(numberId));
                taskList.Add(task);
                resList.Add(numberId);
            }
            int i = 0;
            foreach(Task<ItemResponse<ParkingVehicle>> task in taskList){
                try{
                    ParkingVehicle parkingVehicle = task.Result.Resource;
                    string delId = numbersList[i++];
                    _logger.LogInformation("completed cosmos to delete ParkingVehicle Id: "+delId);       
                }catch(Exception e){
                    _logger.LogError(e.StackTrace);        
                }
            }
            return resList;
        }
    }
}