
namespace Shared.Messages.Models
{
    public interface IOrderBase : IOrderHeaderBase
    {
        OrderBase.OrderItem[] Items { get; set; }
    }
}