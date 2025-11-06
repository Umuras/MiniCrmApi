using Microsoft.EntityFrameworkCore;
using MiniCrmApi.Data;
using MiniCrmApi.Models;

namespace MiniCrmApi.Repositories
{
    public class OrderDetailRepository : IOrderDetailRepository
    {
        private readonly MiniCrmContext _context;

        public OrderDetailRepository(MiniCrmContext context)
        {
            _context = context;
        }

        public async Task<List<OrderDetail>> GetAllOrderDetailsAsync()
        {
            List<OrderDetail> orderDetails = await _context.OrderDetails.Include(o => o.Order).Include(p => p.Product).ToListAsync();
            return orderDetails;
        }

        public async Task<OrderDetail> GetOrderDetailAsync(int id)
        {
            OrderDetail? orderDetail = await _context.OrderDetails.Include(o => o.Order).Include(p => p.Product).FirstOrDefaultAsync(x => x.Id == id);
            return orderDetail;
        }

        public async Task AddOrderDetailAsync(OrderDetail orderDetail)
        {
            _context.OrderDetails.Add(orderDetail);
            await _context.SaveChangesAsync();

        }

        public async Task UpdateOrderDetailAsync(OrderDetail orderDetail)
        {
            _context.Entry(orderDetail).State = EntityState.Modified;
            await _context.SaveChangesAsync();
        }

        public async Task DeleteOrderDetailAsync(OrderDetail orderDetail)
        {
            _context.OrderDetails.Remove(orderDetail);
            await _context.SaveChangesAsync();
        }
    }
}
