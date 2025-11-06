using MiniCrmApi.Models;

namespace MiniCrmApi.Services
{
    public interface IOrderDetailService
    {
        Task<List<OrderDetail>> GetAllOrderDetailsAsync();
        Task<OrderDetail> GetOrderDetailByIdAsync(int id);
        Task AddOrderDetailAsync(OrderDetail orderDetail);
        Task UpdateOrderDetailAsync(int id, OrderDetail orderDetail);
        Task DeleteOrderDetailAsync(int id);
    }
}
