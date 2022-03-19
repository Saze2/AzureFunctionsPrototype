using System;
using System.IO;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Azure.WebJobs;
using Microsoft.Azure.WebJobs.Extensions.Http;
using Microsoft.AspNetCore.Http;
using Microsoft.Extensions.Logging;
using Newtonsoft.Json;
using System.Linq;
using Microsoft.Azure.Cosmos.Table;

namespace customer_function
{
    public static class GetSingleCustomer
    {
        [FunctionName("GetSingleCustomer")]
        public static async Task<IActionResult> GetAll(
        [HttpTrigger(AuthorizationLevel.Function, "get", Route = null)] HttpRequest req,
        [Table("Customers", Connection = "AzureWebJobsStorage")] CloudTable cloudTable,
        ILogger log)
        {

            string iban = req.Query["iban"];
            string requestBody = await new StreamReader(req.Body).ReadToEndAsync();
            dynamic requestData = JsonConvert.DeserializeObject(requestBody);
            iban = iban ?? requestData?.iban;

            log.LogInformation($"Getting Customer Data for {iban} ...");

            TableQuery<Customer> query = new TableQuery<Customer>()
                .Where(TableQuery.GenerateFilterCondition("IBAN", QueryComparisons.Equal, iban));
            
            var segment = await cloudTable.ExecuteQuerySegmentedAsync(query, null);
            var data = segment.Select(CustomerExtensions.GetCustomer);

            return new OkObjectResult(data);
        }
    }
}
