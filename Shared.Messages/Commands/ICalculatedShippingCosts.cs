using System;
using System.Collections.Generic;
using System.Text;
using Shared.Messages.Models;

namespace Shared.Messages.Events
{
    public interface ICalculatedShippingCosts : IOrderBase, IContainShippingCost
    {
        
    }
}
