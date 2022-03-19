using System;
using System.IO;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Azure.WebJobs;
using Microsoft.Azure.WebJobs.Extensions.Http;
using Microsoft.AspNetCore.Http;
using Microsoft.Extensions.Logging;
using Newtonsoft.Json;

namespace customer_function
{
    public class CreateCustomer
    {
        [FunctionName("CreateCustomer")]
        [return: Table("Customers")]
        public async static Task<Customer> TableOutput(
            [HttpTrigger(AuthorizationLevel.Function, "post", Route = null)] HttpRequest req,
            ILogger log)
        {
            string name = req.Query["name"];
            string address = req.Query["address"];
            string iban = req.Query["iban"];

            string requestBody = await new StreamReader(req.Body).ReadToEndAsync();
            dynamic data = JsonConvert.DeserializeObject(requestBody);
            name = name ?? data?.name;
            address = address ?? data?.address;
            iban = iban ?? data?.iban;
            
            log.LogInformation($"Creating Customer: name = {name}, address = {address}, iban = {iban}");

            var customer = new Customer
            {
                PartitionKey = "CustomerPartition",
                RowKey = Guid.NewGuid().ToString(),
                Name = name,
                Address = address,
                IBAN = iban
            };
            
            return customer;
        }
    }
}
