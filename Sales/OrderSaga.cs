using NServiceBus;
using NServiceBus.Logging;
using Shared.Messages.Commands;
using Shared.Messages.Events;
using Shared.Messages.Models;
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
        IHandleMessages<ICalculatedShippingCosts>,
        IHandleMessages<IPreapprovedCredit>,
        IHandleMessages<OrderBilled>,
        IHandleMessages<OrderShipped>
    {
        static ILog log = LogManager.GetLogger<OrderSaga>();

        protected override void ConfigureHowToFindSaga(SagaPropertyMapper<OrderSagaData> mapper)
        {
            mapper.ConfigureMapping<ProcessSale>(message => message.OrderId)
             .ToSaga(sagaData => sagaData.OrderId);

            mapper.ConfigureMapping<ICalculatedShippingCosts>(message => message.OrderId)
                .ToSaga(sagaData => sagaData.OrderId);

            mapper.ConfigureMapping<IPreapprovedCredit>(message => message.OrderId)
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
                m.OrderId = Data.OrderId;
                m.OrderDate = Data.OrderDate;
                m.Items = Data.Items;
            });

        }

        public Task Handle(ICalculatedShippingCosts message, IMessageHandlerContext context)
        {
            Data.ShippingCosts = (double?)message.ShippingCost;

            return context.Send<PreapproveAvailableCredit>(m =>
            {
                m.OrderId = Data.OrderId;
                m.OrderDate = Data.OrderDate;
                m.Items = Data.Items;
                m.ShippingCost = Data.ShippingCosts.Value;
            });
        }

        public Task Handle(IPreapprovedCredit message, IMessageHandlerContext context)
        {
            log.Info("Credit Balance is Verified");

            Data.CreditBalanceChecked = true;

            return context.Publish<IRecievedNewOrder>(
                messageConstructor: m =>
                {
                    m.OrderId = Data.OrderId;
                    m.OrderDate = Data.OrderDate;
                    m.Items = Data.Items;
                    m.ShippingCost = Data.ShippingCosts != null ? Data.ShippingCosts.Value : 0.0;
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
