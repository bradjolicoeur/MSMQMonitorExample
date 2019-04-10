using System;

namespace Shared.Messages.Models
{
    public class OrderHeaderBase : IOrderHeaderBase
    {
        public string OrderId { get; set; }
        public DateTime OrderDate { get; set; }
    }
}
