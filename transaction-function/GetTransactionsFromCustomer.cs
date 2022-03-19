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

namespace transaction_function
{
    public static class TransactionFunctionExtensions
    {

        [FunctionName("GetTransactionForCustomer")]
        public static async Task<IActionResult> GetAll(
        [HttpTrigger(AuthorizationLevel.Function, "get", Route = null)] HttpRequest req,
        [Table("Transactions", Connection = "AzureWebJobsStorage")] CloudTable cloudTable,
        ILogger log)
        {
            string iban = req.Query["iban"];
            string requestBody = await new StreamReader(req.Body).ReadToEndAsync();
            dynamic requestData = JsonConvert.DeserializeObject(requestBody);
            iban = iban ?? requestData?.iban;

            log.LogInformation($"Getting Transactions for {iban} ...");

            TableQuery<Transaction> query = new TableQuery<Transaction>().Where(
                TableQuery.CombineFilters(
                TableQuery.GenerateFilterCondition("senderIBAN", QueryComparisons.Equal, iban),
                TableOperators.Or,
                TableQuery.GenerateFilterCondition("receiverIBAN", QueryComparisons.Equal, iban)));

            var segment = await cloudTable.ExecuteQuerySegmentedAsync(query, null);
            var data = segment.Select(TransactionExtension.GetTransaction);

            return new OkObjectResult(data);
        }
    }
}
