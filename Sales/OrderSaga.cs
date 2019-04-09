using NServiceBus;
using NServiceBus.Logging;
using Shared.Messages.Commands;
using Shared.Messages.Events;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Sales
{
    public class OrderSaga :
        Saga<OrderSagaData>,
        IAmStartedByMessages<ProcessSale>,
        IHandleMessages<ShippingCosts>,
        IHandleMessages<CreditBalanceVerified>,
        IHandleMessages<OrderBilled>,
        IHandleMessages<OrderShipped>
    {
        static ILog log = LogManager.GetLogger<OrderSaga>();

        protected override void ConfigureHowToFindSaga(SagaPropertyMapper<OrderSagaData> mapper)
        {
            mapper.ConfigureMapping<ProcessSale>(message => message.OrderId)
             .ToSaga(sagaData => sagaData.OrderId);

            mapper.ConfigureMapping<ShippingCosts>(message => message.OrderId)
                .ToSaga(sagaData => sagaData.OrderId);

            mapper.ConfigureMapping<CreditBalanceVerified>(message => message.OrderId)
                .ToSaga(sagaData => sagaData.OrderId);

            mapper.ConfigureMapping<OrderShipped>(message => message.OrderId)
                .ToSaga(sagaData => sagaData.OrderId);

            mapper.ConfigureMapping<OrderBilled>(message => message.OrderId)
                .ToSaga(sagaData => sagaData.OrderId);
        }

        public Task Handle(ProcessSale message, IMessageHandlerContext context)
        {
            log.Info("Initialize Saga");

            Data.Items = message.Items;
            Data.OrderDate = message.OrderDate;

            return context.Send<CalculateShippingCost>(m =>
            {
                m.OrderId = message.OrderId;
                m.OrderDate = message.OrderDate;
                m.Items = message.Items;
            });

        }

        public Task Handle(ShippingCosts message, IMessageHandlerContext context)
        {
            Data.ShippingCosts = (decimal?)message.ShippingCost;

            return context.Send<VerifyCreditBalance>(m =>
            {
                m.OrderId = message.OrderId;
                m.OrderDate = message.OrderDate;
                m.Items = message.Items;
                m.ShippingCost = message.ShippingCost;
            });
        }

        public Task Handle(CreditBalanceVerified message, IMessageHandlerContext context)
        {
            log.Info("Credit Balance is Verified");

            Data.CreditBalanceChecked = true;

            return context.Publish<IRecievedNewOrder>(
                messageConstructor: m =>
                {
                    m.OrderId = message.OrderId;
                    m.OrderDate = message.OrderDate;
                    m.Items = message.Items;
                    m.ShippingCost = Data.ShippingCosts.Value;
                });

        }

        public Task Handle(OrderBilled message, IMessageHandlerContext context)
        {
            log.Info("Order Billed");

            Data.Shipped = true;

            return Task.CompletedTask;
        }

        public Task Handle(OrderShipped message, IMessageHandlerContext context)
        {
            log.Info("Order Shipped");

            Data.Billed = true;

            return Task.CompletedTask;
        }
    }
}
