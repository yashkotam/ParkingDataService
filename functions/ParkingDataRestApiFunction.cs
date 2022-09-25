using System;
using System.IO;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Azure.WebJobs;
using Microsoft.Azure.WebJobs.Extensions.Http;
using Microsoft.AspNetCore.Http;
using Microsoft.Extensions.Logging;
using Newtonsoft.Json;

namespace ParkingDataServiceNameSpace
{
    public class ParkingDataRestApiFunction
    {
        private readonly ILogger _logger;
        private ParkingService _parkingService;

        public  ParkingDataRestApiFunction(ILogger<ParkingDataRestApiFunction> logger, ParkingService parkingService){
            this._logger = logger;
            this._parkingService = parkingService;        
        } 

        [FunctionName("UpdateParkingUnitData")]
        public async Task<IActionResult> Run(
            [HttpTrigger(AuthorizationLevel.Function, "post", Route = "parkingData")] HttpRequest req)
        {
            string requestBody = await new StreamReader(req.Body).ReadToEndAsync();
            ParkingDataUpdateRequest  parkingDataUpdateRequest = JsonConvert.DeserializeObject<ParkingDataUpdateRequest>(requestBody);
            _logger.LogInformation("parkingDataUpdateRequest : id : "+parkingDataUpdateRequest.id);
            _logger.LogInformation("parkingDataUpdateRequest : numbers list size : "+parkingDataUpdateRequest.numbersList.Length);

            _parkingService.updateParkingUnit(parkingDataUpdateRequest.id, parkingDataUpdateRequest.numbersList);

            string responseMessage = "OK";

            return new OkObjectResult(responseMessage);
        }
    }
}
