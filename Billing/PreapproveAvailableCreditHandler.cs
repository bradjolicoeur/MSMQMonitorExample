using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using NServiceBus;
using NServiceBus.Logging;
using Shared.Messages.Commands;
using Shared.Messages.Events;

namespace Billing
{
    public class PreapproveAvailableCreditHandler : IHandleMessages<PreapproveAvailableCredit>
    {
        static ILog log = LogManager.GetLogger<RecievedNewOrderHandler>();

        public Task Handle(PreapproveAvailableCredit message, IMessageHandlerContext context)
        {
            log.Info("Preapprove Credit Balance");

            // simulate work being done
            //Thread.Sleep(5000);

            return context.Publish<IPreapprovedCredit>(m =>
            {
                m.OrderId = message.OrderId;
                m.OrderDate = message.OrderDate;
                m.Items = message.Items;
            });
        }
    }
}
