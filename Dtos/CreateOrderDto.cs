using MiniCrmApi.Models;

namespace MiniCrmApi.Dtos
{
    public record CreateOrderDto
    {
        public int CustomerId { get; set; }
        public List<CreateOrderDetailDto> OrderDetails { get; set; } = new List<CreateOrderDetailDto>();
    }
}
