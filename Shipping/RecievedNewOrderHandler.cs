using NServiceBus;
using NServiceBus.Logging;
using Shared.Messages.Events;
using Shared.Messages.Commands;
using System;
using System.Threading.Tasks;
using System.Threading;

namespace Shipping
{
    public class RecievedNewOrderHandler : IHandleMessages<IRecievedNewOrder>
    {
        static ILog log = LogManager.GetLogger<RecievedNewOrderHandler>();

        public Task Handle(IRecievedNewOrder message, IMessageHandlerContext context)
        {
            // simulate work being done
            Thread.Sleep(700);

            log.Info("Order Shipped");

            return context.Send("Sales", new OrderShipped
            {
                OrderId = message.OrderId,
                OrderDate = message.OrderDate,
                ShippedDate = DateTime.UtcNow
            });
        }
    }
}
