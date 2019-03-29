using System;
using System.Collections.Generic;
using System.Text;

namespace Shared.Messages
{
    public class OrderBase : OrderHeaderBase, IOrderBase
    {
        
        public OrderItem[] Items { get; set; }

        public class OrderItem
        {
            public int Quantity { get; set; }
            public double Price { get; set; }
        }
    }
}
