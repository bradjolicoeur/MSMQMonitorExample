using System;
using System.Threading.Tasks;
using NServiceBus;
using NServiceBus.Logging;
using Shared.Messages.Commands;
using Shared.Messages.Events;

namespace Sales
{
    //Replaced by saga
    //public class ProcessSaleHandler : IHandleMessages<ProcessSale>
    //{
    //    static ILog log = LogManager.GetLogger<ProcessSaleHandler>();

    //    public  Task Handle(ProcessSale message, IMessageHandlerContext context)
    //    {
    //        log.Info("Hello from MyHandler");
    //        //GenerateException();

    //        return context.Publish<IRecievedNewOrder>(
    //            messageConstructor: m =>
    //            {
    //                m.OrderId = message.OrderId;
    //            });

    //    }

    //    public void GenerateException()
    //    {
    //        throw new Exception("forced exception for demo");
    //    }
    //}
}
