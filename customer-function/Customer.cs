using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Microsoft.Azure.WebJobs.Extensions.Storage;
using Microsoft.Azure.Cosmos.Table;

namespace customer_function
{
    public class Customer : TableEntity
    {
        public string Name { get; set; }
        public string Address { get; set; }
        public string IBAN { get; set; }

    }
    public static class CustomerExtensions
    {
        public static Customer GetCustomer(this Customer cust)
        {
            return new Customer
            {
                Name = cust.Name,
                Address = cust.Address,
                IBAN = cust.IBAN,
            };
        }
    }
}
