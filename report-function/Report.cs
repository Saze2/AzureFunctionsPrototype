using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace report_function
{
    public class Report
    {
        public string Name { get; set; }
        public string Address { get; set; }
        public string IBAN { get; set; }

        public List<Transaction> Transactions { get; set; }
    }

    public class CustomerReceived
    {
        public string name { get; set; }
        public string address { get; set; }
        public string iban { get; set; }

        public string partitionKey { get; set; }

        public string rowKey { get; set; }
        public string timestamp { get; set; }
        public string eTag { get; set; }
    }

    public class Customer
    {
        public string Name { get; set; }
        public string Address { get; set; }
        public string IBAN { get; set; }

    }

    public class Transaction
    {
        public DateTime executionDate { get; set; }
        public int amount { get; set; }
        public string senderIBAN { get; set; }
        public string receiverIBAN { get; set; }

    }
}
