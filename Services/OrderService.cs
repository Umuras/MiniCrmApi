using Microsoft.EntityFrameworkCore.Storage;
using MiniCrmApi.Data;
using MiniCrmApi.Dtos;
using MiniCrmApi.Enums;
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

        public async Task<Order> AddAsync(CreateOrderDto orderDto)
        {
            Order order = new Order();
            order.Status = OrderStatus.Pending.ToString();
            
            Customer customer = await customerService.GetByIdAsync(orderDto.CustomerId);
            order.CustomerId = customer.Id;

            foreach (CreateOrderDetailDto detail in orderDto.OrderDetails)
            {
                
                Product product = await productService.GetProductByIdAsync(detail.ProductId);
                if(product.StockQuantity < detail.Quantity)
                {
                    throw new Exception($"There isn't enough product {product.Name} stock quantity, so you don't order this product {product.Name} for this quantity");
                }

                product.StockQuantity -= detail.Quantity;

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
                return order;
            }
            catch
            {
                await transaction.RollbackAsync(); // Hata olursa rollback yapacak.
                throw; // ExceptionMiddleware yakalayacak
            }
        }

        public async Task UpdateAsync(int id, UpdateOrderRequestDto orderStatusRequest)
        {
            if (orderStatusRequest == null)
            {
                throw new ArgumentNullException(nameof(orderStatusRequest), "OrderStatus cannot be null.");
            }

            Order dbOrder = await GetByIdAsync(id);

            if(!Enum.IsDefined(typeof(OrderStatus), orderStatusRequest.OrderStatus))
            {
                throw new ArgumentNullException("Status cannot be empty or null");
            }
            else
            {
                dbOrder.Status = orderStatusRequest.OrderStatus.ToString();
            }

            using IDbContextTransaction transaction = await context.Database.BeginTransactionAsync();

            try
            {
                await orderRepository.UpdateAsync(dbOrder);
                await context.SaveChangesAsync();

                await transaction.CommitAsync();
            }
            catch (Exception)
            {
                await transaction.RollbackAsync();
                throw;
            }
            
        }

        public async Task DeleteAsync(int id)
        {
            Order order = await GetByIdAsync(id);

            using IDbContextTransaction transaction = await context.Database.BeginTransactionAsync();

            try
            {
                await orderRepository.DeleteAsync(order);
                await context.SaveChangesAsync();

                await transaction.CommitAsync();
            }
            catch (Exception)
            {
                await transaction.RollbackAsync();
                throw;
            }
            
        }

        public async Task UpdateOrderTotalPriceAndQuantityAsync(int orderId)
        {
            Order order = await orderRepository.GetOrderWithDetailsAsync(orderId);

            if(order == null)
            {
                throw new KeyNotFoundException($"There isn't order belong this id:{orderId}");
            }

            //Quantity propertysi eklenmeli bir orderda üründen kaç tane sipariş etmiş.
            order.TotalPrice = order.OrderDetails.Sum(o => o.Price);
            order.TotalQuantity = order.OrderDetails.Sum(o => o.Quantity);
            await orderRepository.UpdateAsync(order);
        }

        public List<OrderResponseDto> ChangeOrdersResponse(List<Order> dbOrders)
        {
            List<OrderResponseDto> orderResponseList = new List<OrderResponseDto>();

            foreach (Order order in dbOrders)
            {
                OrderResponseDto orderResponse = new OrderResponseDto();
                orderResponse.Id = order.Id;
                orderResponse.CustomerId = order.CustomerId;
                orderResponse.OrderDate = order.CreatedDate;
                orderResponse.Status = order.Status;
                orderResponse.TotalPrice = order.TotalPrice;
                orderResponse.TotalQuantity = order.TotalQuantity;
                List<OrderDetailResponse> orderDetails = order.OrderDetails.Select(od => new OrderDetailResponse
                {
                    ProductId = od.ProductId,
                    Quantity = od.Quantity,
                    Price = od.Price
                }).ToList();
                orderResponse.OrderDetails = orderDetails;

                orderResponseList.Add(orderResponse);
            }
            return orderResponseList;        
        }

        public OrderResponseDto ChangeOrderResponse(Order order)
        {
            OrderResponseDto orderResponse = new OrderResponseDto();
            orderResponse.Id = order.Id;
            orderResponse.CustomerId = order.CustomerId;
            orderResponse.OrderDate = order.CreatedDate;
            orderResponse.Status = order.Status;
            orderResponse.TotalPrice = order.TotalPrice;
            orderResponse.TotalQuantity = order.TotalQuantity;
            List<OrderDetailResponse> orderDetails = order.OrderDetails.Select(od => new OrderDetailResponse
            {
                ProductId = od.ProductId,
                Quantity = od.Quantity,
                Price = od.Price
            }).ToList();
            orderResponse.OrderDetails = orderDetails;

            return orderResponse;
        }
    }
}
