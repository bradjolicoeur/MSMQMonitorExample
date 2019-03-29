using System;

namespace Shared.Messages
{
    public interface IOrderHeaderBase
    {
        DateTime OrderDate { get; set; }
        string OrderId { get; set; }
    }
}