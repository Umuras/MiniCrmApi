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
            List<Order> orderList = await context.Orders.Include(o => o.OrderDetails).ToListAsync<Order>();
            return orderList;
        }

        public async Task<Order> GetByIdAsync(int id)
        {
            Order? order = await context.Orders.Include(o => o.OrderDetails).FirstOrDefaultAsync(x => x.Id == id);
            return order;
        }

        public async Task AddAsync(Order order)
        {
            await context.Orders.AddAsync(order);
        }

        public Task UpdateAsync(Order order)
        {
            context.Orders.Update(order);
            return Task.CompletedTask;
        }

        public Task DeleteAsync(Order order)
        {
            context.Orders.Remove(order);
            return Task.CompletedTask;
        }

        public async Task<Order> GetOrderWithDetailsAsync(int orderId)
        {
            Order? order = await context.Orders.Include(o => o.OrderDetails).FirstOrDefaultAsync(x => x.Id == orderId);
            return order;
        }
    }
}
