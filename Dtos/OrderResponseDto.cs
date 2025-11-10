namespace MiniCrmApi.Dtos
{
    public record OrderResponseDto
    {
        public int Id { get; set; }
        public DateTime OrderDate { get; set; }
        public string Status { get; set; } = string.Empty;
        public int CustomerId { get; set; }
        public decimal TotalPrice { get; set; }
        public int TotalQuantity { get; set; }
        public List<OrderDetailResponse> OrderDetails { get; set; } = new();
    }
}
