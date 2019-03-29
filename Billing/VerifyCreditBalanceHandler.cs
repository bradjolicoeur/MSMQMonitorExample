using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using NServiceBus;
using NServiceBus.Logging;
using Shared.Messages.Commands;

namespace Billing
{
    public class VerifyCreditBalanceHandler : IHandleMessages<VerifyCreditBalance>
    {
        static ILog log = LogManager.GetLogger<RecievedNewOrderHandler>();

        public Task Handle(VerifyCreditBalance message, IMessageHandlerContext context)
        {
            log.Info("Verify Credit Balance");

            // simulate work being done
            //Thread.Sleep(5000);

            return context.Send("Sales", new CreditBalanceVerified
            {
                OrderId = message.OrderId,
                OrderDate = message.OrderDate,
                Items = message.Items
            });
        }
    }
}
