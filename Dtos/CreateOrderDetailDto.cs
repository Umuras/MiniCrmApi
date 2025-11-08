namespace MiniCrmApi.Dtos
{
    public record CreateOrderDetailDto
    {
        public int ProductId { get; set; }
        public int Quantity { get; set; }
    }
}
