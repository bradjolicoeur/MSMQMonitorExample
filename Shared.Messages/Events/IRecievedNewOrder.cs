using System;
using System.Collections.Generic;
using System.Text;

namespace Shared.Messages.Events
{
    public interface IRecievedNewOrder : IOrderBase
    {

        decimal ShippingCost { get; set; }
    }
}
