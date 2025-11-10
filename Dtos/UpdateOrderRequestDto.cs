using MiniCrmApi.Enums;

namespace MiniCrmApi.Dtos
{
    public record UpdateOrderRequestDto
    {
        public OrderStatus OrderStatus {  get; set; }
    }
}
