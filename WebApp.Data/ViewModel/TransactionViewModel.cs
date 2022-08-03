using System;
using System.Collections.Generic;
using System.Text;

namespace WebApp.Data.ViewModel
{
    public class TransactionViewModel
    {
        public string TransactionId { get; set; }
        public string Payment { get; set; }
        public string Status { get; set; }
    }
    public class UploadTransactionViewModel
    {
        public string TransactionId { get; set; }
        public decimal Amount { get; set; }
        public string CurrencyCode { get; set; }
        public DateTime TransactionDate { get; set; }
        public string Status { get; set; }
    }
}
