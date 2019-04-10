using Shared.Messages.Models;
using System;
using System.Collections.Generic;
using System.Text;

namespace Shared.Messages.Commands
{
    public class PreapproveAvailableCredit : OrderBase, IContainShippingCost
    {
        public double ShippingCost { get; set; }
    }
}
