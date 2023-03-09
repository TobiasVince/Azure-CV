using System;
using System.IO;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Azure.WebJobs;
using Microsoft.Azure.WebJobs.Extensions.Http;
using Microsoft.AspNetCore.Http;
using Microsoft.Extensions.Logging;
using Newtonsoft.Json;
using System.Diagnostics.Metrics;
using System.Net.Http;
using System.Text;
using Company.Function;

namespace GetResumeCounter
{
    public static class GetResumeCounter
    {
        [FunctionName("GetResumeCounter")]
        public static HttpResponseMessage Run(
            [HttpTrigger(AuthorizationLevel.Function, "get", "post", Route = null)] HttpRequest req,
            [CosmosDB(databaseName:"azurecvus", collectionName: "Counter", ConnectionStringSetting = "AzureResumeConnectionString", Id = "1", PartitionKey ="!")] Counter<int> Counter,
           [CosmosDB(databaseName:"azurecvus", collectionName: "Counter", ConnectionStringSetting = "AzureResumeConnectionString", Id = "1", PartitionKey ="1")] out Counter<int> updatedCounter,
            ILogger log)
        {
            log.LogInformation("C# HTTP trigger function processed a request.");

            updatedCounter = Counter;
            updatedCounter.Count += 1;
            
            var jsonToReturn =JsonConvert.SerializeObject(Counter);
        


           return new HttpResponseMessage(System.Net.HttpStatusCode.OK)
{
    Content = new StringContent(jsonToReturn, Encoding.UTF8, "application/json")
};

        }
    }
}
