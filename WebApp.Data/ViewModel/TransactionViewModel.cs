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
        public string Amount { get; set; }
        public string CurrencyCode { get; set; }
        public string TransactionDate { get; set; }
        public string Status { get; set; }
    }
}
