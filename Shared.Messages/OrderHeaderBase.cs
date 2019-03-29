using System;

namespace Shared.Messages
{
    public class OrderHeaderBase : IOrderHeaderBase
    {
        public string OrderId { get; set; }
        public DateTime OrderDate { get; set; }
    }
}
