using Shared.Messages.Models;
using System;

namespace Shared.Messages.Events
{
    public interface IBilledOrder : IOrderHeaderBase
    {
        DateTime BilledDate { get; set; }
    }
}
