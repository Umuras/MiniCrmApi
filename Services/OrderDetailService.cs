using MiniCrmApi.Models;
using MiniCrmApi.Repositories;

namespace MiniCrmApi.Services
{
    public class OrderDetailService : IOrderDetailService
    {
        private readonly IOrderDetailRepository _orderDetailRepository;
        private readonly IProductService _productService;
        private readonly IOrderService _orderService;

        public OrderDetailService(IOrderDetailRepository orderDetailRepository, IProductService productService, IOrderService orderService)
        {
            _orderDetailRepository = orderDetailRepository;
            _productService = productService;
            _orderService = orderService;
        }

        public async Task<List<OrderDetail>> GetAllOrderDetailsAsync()
        {
            List<OrderDetail> orderDetails = await _orderDetailRepository.GetAllOrderDetailsAsync();
            return orderDetails;
        }

        public async Task<OrderDetail> GetOrderDetailByIdAsync(int id)
        {
            OrderDetail orderDetail = await _orderDetailRepository.GetOrderDetailAsync(id);
            
            if(orderDetail == null)
            {
                throw new KeyNotFoundException($"There isn't orderdetail belong this id:{id}");
            }
            
            return orderDetail;
        }

        public async Task AddOrderDetailAsync(OrderDetail orderDetail)
        {
            if (orderDetail == null)
            {
                throw new ArgumentNullException(nameof(orderDetail), "OrderDetail cannot be null");
            }

            Product product = await _productService.GetProductByIdAsync(orderDetail.ProductId);
            orderDetail.Price = product.Price;

            await _orderDetailRepository.AddOrderDetailAsync(orderDetail);
            await _orderService.UpdateOrderTotalPriceAsync(orderDetail.OrderId);
        }

        public async Task UpdateOrderDetailAsync(int id, OrderDetail orderDetail)
        {
            if (orderDetail == null)
            {
                throw new ArgumentNullException(nameof(orderDetail), "OrderDetail cannot be null");
            }

            OrderDetail dbOrderDetail = await GetOrderDetailByIdAsync(id);

            if(orderDetail.Price > 0)
            {
                dbOrderDetail.Price = orderDetail.Price;
            }

            if(orderDetail.OrderId != dbOrderDetail.OrderId)
            {
                await _orderService.GetByIdAsync(orderDetail.OrderId);
                dbOrderDetail.OrderId = orderDetail.OrderId;
            }

            if(orderDetail.ProductId != dbOrderDetail.ProductId)
            {
                await _productService.GetProductByIdAsync(orderDetail.ProductId);
                dbOrderDetail.ProductId = orderDetail.ProductId;
            }

            await _orderDetailRepository.UpdateOrderDetailAsync(dbOrderDetail);
            await _orderService.UpdateOrderTotalPriceAsync(dbOrderDetail.OrderId);
        }

        public async Task DeleteOrderDetailAsync(int id)
        {
            OrderDetail orderDetail = await GetOrderDetailByIdAsync(id);
            await _orderDetailRepository.DeleteOrderDetailAsync(orderDetail);

            await _orderService.UpdateOrderTotalPriceAsync(orderDetail.OrderId);
        }
    }
}
