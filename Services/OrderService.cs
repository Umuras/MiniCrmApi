using Microsoft.EntityFrameworkCore.Storage;
using MiniCrmApi.Data;
using MiniCrmApi.Dtos;
using MiniCrmApi.Models;
using MiniCrmApi.Repositories;

namespace MiniCrmApi.Services
{
    public class OrderService : IOrderService
    {
        private MiniCrmContext context;
        private IOrderRepository orderRepository;
        private ICustomerService customerService;
        private IProductService productService;

        public OrderService(IOrderRepository orderRepository, ICustomerService customerService, 
            IProductService productService, MiniCrmContext context)
        {
            this.orderRepository = orderRepository;
            this.customerService = customerService;
            this.productService = productService;
            this.context = context;
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

        public async Task AddAsync(CreateOrderDto orderDto)
        {
            Order order = new Order();
            
            Customer customer = await customerService.GetByIdAsync(orderDto.CustomerId);
            order.CustomerId = customer.Id;

            foreach (CreateOrderDetailDto detail in orderDto.OrderDetails)
            {
                
                Product product = await productService.GetProductByIdAsync(detail.ProductId);
                order.TotalPrice += detail.Quantity * product.Price;
                order.TotalQuantity += detail.Quantity;

                OrderDetail orderDetail = new OrderDetail()
                {
                    Product = product,
                    ProductId = detail.ProductId,
                    Price = detail.Quantity * product.Price,
                    Quantity = detail.Quantity
                };
  
                order.OrderDetails.Add(orderDetail);
            }

            // Transaction başlat
            using IDbContextTransaction transaction = await context.Database.BeginTransactionAsync();
            try
            {
                await orderRepository.AddAsync(order); // Order ve OrderDetails eklenir
                await context.SaveChangesAsync();

                await transaction.CommitAsync(); // Her şey başarılı ise commit edecek.
            }
            catch
            {
                await transaction.RollbackAsync(); // Hata olursa rollback yapacak.
                throw; // ExceptionMiddleware yakalayacak
            }
        }

        public async Task UpdateAsync(int id, Order order)
        {
            if (order == null)
            {
                throw new ArgumentNullException(nameof(order), "Order cannot be null.");
            }

            Order dbOrder = await GetByIdAsync(id);
            
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

            //Quantity propertysi eklenmeli bir orderda üründen kaç tane sipariş etmiş.
            order.TotalPrice = order.OrderDetails.Sum(o => o.Price * o.Order.TotalQuantity);

            await orderRepository.UpdateAsync(order);
        }
    }
}
