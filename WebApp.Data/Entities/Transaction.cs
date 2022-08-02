using System;
using System.Collections.Generic;
using System.Text;

namespace WebApp.Data.Entities
{
    public class Transaction : BaseEntity
    {
        public string TransactionId { get; set; }
        public decimal Amount { get; set; }
        public string CurrencyCode { get; set; }
        public DateTime TransactionDate { get; set; }
        public string Status { get; set; }
    }
}
