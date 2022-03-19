using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Microsoft.Azure.Cosmos.Table;


namespace transaction_function
{
    public class Transaction : TableEntity
    {
        public DateTime executionDate { get; set; }
        public int amount { get; set; }
        public string senderIBAN { get; set; }
        public string receiverIBAN { get; set; }
    }

    public static class TransactionExtension
    {
        public static Transaction GetTransaction(this Transaction transaction)
        {
            return new Transaction
            {
                executionDate = transaction.executionDate,
                amount = transaction.amount,
                senderIBAN = transaction.senderIBAN,
                receiverIBAN = transaction.receiverIBAN
            };
        }
    }
}