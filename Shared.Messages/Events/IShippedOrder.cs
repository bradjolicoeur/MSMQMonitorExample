using Shared.Messages.Models;
using System;

namespace Shared.Messages.Events
{
    public interface IShippedOrder : IOrderHeaderBase
    {
        DateTime ShippedDate { get; set; }
    }
}
