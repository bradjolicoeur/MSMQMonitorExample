
namespace Shared.Messages
{
    public interface IOrderBase : IOrderHeaderBase
    {
        OrderBase.OrderItem[] Items { get; set; }
    }
}