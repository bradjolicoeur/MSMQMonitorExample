using System;

namespace Shared.Messages.Models
{
    public interface IOrderHeaderBase
    {
        DateTime OrderDate { get; set; }
        string OrderId { get; set; }
    }
}