﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using NServiceBus;
using NServiceBus.Logging;
using Shared.Messages.Commands;

namespace Shipping
{
    public class CalculateShippingCostsHandler : IHandleMessages<CalculateShippingCost>
    {
        static ILog log = LogManager.GetLogger<RecievedNewOrderHandler>();

        public Task Handle(CalculateShippingCost message, IMessageHandlerContext context)
        {
            log.Info("Calculating Shipping cost");

            //throw new Exception("Force Exception for Demo");

            return context.Send("Sales", new ShippingCosts {
                OrderDate = message.OrderDate,
                OrderId = message.OrderId,
                Items = message.Items,
                ShippingCost = message.Items.Sum(x => x.Price) * 0.2 });
        }
    }
}