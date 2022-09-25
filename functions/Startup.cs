using Microsoft.Azure.Cosmos.Fluent;
using Microsoft.Azure.Functions.Extensions.DependencyInjection;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Logging;

[assembly: FunctionsStartup(typeof(ParkingDataServiceNameSpace.Startup))]

namespace ParkingDataServiceNameSpace
{
    public class Startup : FunctionsStartup
    {
        public override void Configure(IFunctionsHostBuilder builder)
        {
            builder.Services.AddLogging(loggingBuilder =>
            {
                loggingBuilder.AddFilter(level => true);
            });

            builder.Services.AddSingleton((s) =>
            {  
                var connString = System.Environment.GetEnvironmentVariable("CosmosDbConnectionString");

                CosmosClientBuilder cosmosClientBuilder = new CosmosClientBuilder(connString);
                return cosmosClientBuilder.WithConnectionModeDirect()
                    .WithBulkExecution(true)
                    .Build();
            });
            builder.Services.AddTransient<IdbClient, CosmosDbClient>();
            builder.Services.AddTransient<ParkingService>();
        }
    }
}