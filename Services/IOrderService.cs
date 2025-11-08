using MiniCrmApi.Dtos;
using MiniCrmApi.Models;

namespace MiniCrmApi.Services
{
    public interface IOrderService
    {
        Task<List<Order>> GetAllAsync();
        Task<Order> GetByIdAsync(int id);
        Task AddAsync(CreateOrderDto order);
        Task UpdateAsync(int id, Order order);
        Task DeleteAsync(int id);
        Task UpdateOrderTotalPriceAsync(int orderId);
    }
}
