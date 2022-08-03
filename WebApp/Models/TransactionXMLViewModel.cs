using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using System.Xml.Serialization;

namespace WebApp.Models
{
    public class TransactionXMLViewModel
    {
        [XmlElement(ElementName = "Transaction")]
        public List<TransactionDetailXMLViewModel> Transaction { get; set; }
    }
    public class TransactionDetailXMLViewModel
    {
        [XmlAttribute(AttributeName = "id")]
        public string id { get; set; }

        [XmlAttribute(AttributeName = "TransactionDate")]
        public string TransactionDate { get; set; }

        [XmlElement(ElementName = "PaymentDetails")]
        public PaymentDetailXMLViewModel PaymentDetails { get; set; }

        [XmlAttribute(AttributeName = "Status")]
        public string Status { get; set; }
    }
    public class PaymentDetailXMLViewModel
    {

        [XmlAttribute(AttributeName = "Amount")]
        public string Amount { get; set; }

        [XmlAttribute(AttributeName = "CurrencyCode")]
        public string CurrencyCode { get; set; }
    }
}
