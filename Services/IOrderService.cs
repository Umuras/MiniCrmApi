using MiniCrmApi.Dtos;
using MiniCrmApi.Models;

namespace MiniCrmApi.Services
{
    public interface IOrderService
    {
        Task<List<Order>> GetAllAsync();
        Task<Order> GetByIdAsync(int id);
        Task<Order> AddAsync(CreateOrderDto order);
        Task UpdateAsync(int id, UpdateOrderRequestDto orderStatusRequest);
        Task DeleteAsync(int id);
        Task UpdateOrderTotalPriceAndQuantityAsync(int orderId);
        List<OrderResponseDto> ChangeOrdersResponse(List<Order> dbOrders);
        OrderResponseDto ChangeOrderResponse(Order order);
    }
}
