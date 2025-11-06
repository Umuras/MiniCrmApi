using MiniCrmApi.Models;

namespace MiniCrmApi.Repositories
{
    public interface IOrderRepository
    {
        Task<List<Order>> GetAllAsync();
        Task<Order> GetByIdAsync(int id);
        Task AddAsync(Order order);
        Task UpdateAsync(Order order);
        Task DeleteAsync(Order order);
        Task<Order> GetOrderWithDetailsAsync(int orderId);
    }
}
