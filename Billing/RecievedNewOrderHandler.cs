using NServiceBus;
using NServiceBus.Logging;
using Shared.Messages.Events;
using Shared.Messages.Commands;
using System;
using System.Threading.Tasks;
using System.Threading;

namespace Billing
{
    public class RecievedNewOrderHandler : IHandleMessages<IRecievedNewOrder>
    {
        static ILog log = LogManager.GetLogger<RecievedNewOrderHandler>();

        public Task Handle(IRecievedNewOrder message, IMessageHandlerContext context)
        {
            // simulate work being done
            Thread.Sleep(500);

            log.Info("Order Billed");

            return context.Send("Sales", new OrderBilled
            {
                OrderId = message.OrderId,
                OrderDate = message.OrderDate,
                BilledDate = DateTime.UtcNow
            });
        }
    }
}
