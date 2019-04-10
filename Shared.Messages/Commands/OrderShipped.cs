using Shared.Messages.Models;
using System;

namespace Shared.Messages.Commands
{
    public class OrderShipped : OrderHeaderBase
    {
        public DateTime ShippedDate { get; set; }
    }
}
