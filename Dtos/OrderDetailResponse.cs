namespace MiniCrmApi.Dtos
{
    public class OrderDetailResponse
    {
        public int ProductId { get; set; }
        public int Quantity { get; set; }
        public decimal Price { get; set; }
    }
}
