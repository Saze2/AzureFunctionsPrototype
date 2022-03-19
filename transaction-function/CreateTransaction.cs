using System;
using System.IO;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Azure.WebJobs;
using Microsoft.Azure.WebJobs.Extensions.Http;
using Microsoft.AspNetCore.Http;
using Microsoft.Extensions.Logging;
using Newtonsoft.Json;

namespace transaction_function
{
    public static class CreateTransaction
    {
        [FunctionName("CreateTransaction")]
        [return: Table("Transactions")]
        public async static Task<Transaction> TableOutput(
            [HttpTrigger(AuthorizationLevel.Function, "post", Route = null)] HttpRequest req,
            ILogger log)
        {
            string amount = req.Query["amount"];
            string senderIban = req.Query["senderIban"];
            string receiverIban = req.Query["receiverIban"];

            string requestBody = await new StreamReader(req.Body).ReadToEndAsync();
            dynamic data = JsonConvert.DeserializeObject(requestBody);
            amount = amount ?? data?.amount;
            senderIban = senderIban ?? data?.senderIban;
            receiverIban = receiverIban ?? data?.receiverIban;

            log.LogInformation($"Creating Transaction.");

            var transaction = new Transaction
            {
                PartitionKey = "TransactionPartition",
                RowKey = Guid.NewGuid().ToString(),
                executionDate = DateTime.Now,
                amount = System.Convert.ToInt32(amount),
                senderIBAN = senderIban,
                receiverIBAN = receiverIban
            };

            return transaction;
        }
    }
}
