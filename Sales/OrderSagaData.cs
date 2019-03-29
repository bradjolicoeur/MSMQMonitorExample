using NServiceBus;
using Shared.Messages;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Sales
{
    public class OrderSagaData : OrderBase, IContainSagaData
    {
        public bool Shipped { get; set; }
        public bool Billed { get; set; }
        public decimal? ShippingCosts { get; set; }
        public bool CreditBalanceChecked { get; set; }


        public Guid Id { get; set; }
        public string Originator { get; set; }
        public string OriginalMessageId { get; set; }
    }

}
