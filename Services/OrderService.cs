using MiniCrmApi.Models;
using MiniCrmApi.Repositories;

namespace MiniCrmApi.Services
{
    public class OrderService : IOrderService
    {
        private IOrderRepository orderRepository;

        public OrderService(IOrderRepository orderRepository)
        {
            this.orderRepository = orderRepository;
        }

        public async Task<List<Order>> GetAllAsync()
        {
           List<Order> orderList = await orderRepository.GetAllAsync();
            return orderList;
        }

        public async Task<Order> GetByIdAsync(int id)
        {
            Order? order = await orderRepository.GetByIdAsync(id);

            if (order == null)
            {
                throw new KeyNotFoundException($"There isn't order belong with this id:{id}");
            }

            return order;
        }

        public async Task AddAsync(Order order)
        {
            if(order == null)
            {
                throw new ArgumentNullException(nameof(order), "Order cannot be null");
            }

            await orderRepository.AddAsync(order);
        }

        public async Task UpdateAsync(int id, Order order)
        {
            if (order == null)
            {
                throw new ArgumentNullException(nameof(order), "Order cannot be null.");
            }

            Order dbOrder = await GetByIdAsync(id);

            if (order.Name != null)
            {
                dbOrder.Name = order.Name;
            }
            if (order.Description != null)
            {
                dbOrder.Description = order.Description;
            }
            
            await orderRepository.UpdateAsync(dbOrder);
        }

        public async Task DeleteAsync(int id)
        {
            Order order = await GetByIdAsync(id);
            await orderRepository.DeleteAsync(order);
        }

        public async Task UpdateOrderTotalPriceAsync(int orderId)
        {
            Order order = await orderRepository.GetOrderWithDetailsAsync(orderId);

            if(order == null)
            {
                throw new KeyNotFoundException($"There isn't order belong this id:{orderId}");
            }

            order.TotalPrice = order.OrderDetails.Sum(o => o.Price);

            await orderRepository.UpdateAsync(order);
        }
    }
}
