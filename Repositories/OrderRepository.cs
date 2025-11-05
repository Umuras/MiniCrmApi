using Microsoft.EntityFrameworkCore;
using MiniCrmApi.Data;
using MiniCrmApi.Models;

namespace MiniCrmApi.Repositories
{
    public class OrderRepository : IOrderRepository
    {
        private readonly MiniCrmContext context;

        public OrderRepository(MiniCrmContext miniCrmContext)
        {
            this.context = miniCrmContext;
        }

        public async Task<List<Order>> GetAllAsync()
        {
            List<Order> orderList = await context.Orders.ToListAsync<Order>();
            return orderList;
        }

        public async Task<Order> GetByIdAsync(int id)
        {
            Order? order = await context.Orders.FindAsync(id);
            return order;
        }

        public async Task AddAsync(Order order)
        {
            context.Orders.Add(order);
            await context.SaveChangesAsync();
        }

        public async Task UpdateAsync(Order order)
        {
            context.Orders.Update(order);
            await context.SaveChangesAsync();
        }

        public async Task DeleteAsync(Order order)
        {
            context.Orders.Remove(order);
            await context.SaveChangesAsync();
        }
    }
}
