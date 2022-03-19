using System;
using System.IO;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Azure.WebJobs;
using Microsoft.Azure.WebJobs.Extensions.Http;
using Microsoft.AspNetCore.Http;
using Microsoft.Extensions.Logging;
using Newtonsoft.Json;
using System.Net.Http;
using System.Net.Http.Headers;
using System.Net;
using Newtonsoft.Json.Linq;
using System.Collections.Generic;

namespace report_function
{
    public static class DisplayReport
    {
        private static readonly HttpClient client = new HttpClient();

        [FunctionName("GetReport")]
        public static async Task<IActionResult> Run(
            [HttpTrigger(AuthorizationLevel.Function, "get", Route = null)] HttpRequest req,
            ILogger log)
        {
            string iban = req.Query["iban"];
            string requestBody = await new StreamReader(req.Body).ReadToEndAsync();
            dynamic data = JsonConvert.DeserializeObject(requestBody);
            iban = iban ?? data?.iban;
            log.LogInformation($"Request Report for {iban}.");

            HttpResponseMessage response = await client
                .GetAsync($"http://localhost:7071/api/GetSingleCustomer?iban={iban}");
            response.EnsureSuccessStatusCode();
            string customerResponseBody = await response.Content.ReadAsStringAsync();
            
            HttpResponseMessage response2 = await client
                .GetAsync($"http://localhost:7072/api/GetTransactionForCustomer?iban={iban}");
            response.EnsureSuccessStatusCode();
            string transactionResponseBody = await response2.Content.ReadAsStringAsync();

            var settings = new JsonSerializerSettings
            {
                NullValueHandling = NullValueHandling.Ignore,
                MissingMemberHandling = MissingMemberHandling.Ignore
            };

            List<Customer> deserializedCustomer = JsonConvert.DeserializeObject<List<Customer>>(customerResponseBody, settings);
            List<Transaction> deserializedTransactions = JsonConvert.DeserializeObject<List<Transaction>>(transactionResponseBody);

            var report = new Report
            {
                Name = deserializedCustomer[0].Name,
                Address = deserializedCustomer[0].Address,
                IBAN = deserializedCustomer[0].IBAN,
                Transactions = deserializedTransactions
            };


            string jsonString = JsonConvert.SerializeObject(report, Formatting.Indented);
            return new OkObjectResult(jsonString);
        }
    }
}
