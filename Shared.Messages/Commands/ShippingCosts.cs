using System;
using System.Collections.Generic;
using System.Text;

namespace Shared.Messages.Commands
{
    public class ShippingCosts : OrderBase
    {
        public double ShippingCost { get; set; }
    }
}
