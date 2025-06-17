using Core.Model.Order;

namespace Core.Interfaces
{
    public interface IOrderService
    {
        Task<List<OrderModel>> GetOrdersAsync();
    }
}
