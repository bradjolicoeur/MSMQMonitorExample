using Shared.Messages.Models;
using System;

namespace Shared.Messages.Commands
{
    public class OrderBilled : OrderHeaderBase
    {
        public DateTime BilledDate { get; set; }
    }
}
