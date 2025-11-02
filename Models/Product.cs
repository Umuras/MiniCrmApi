namespace MiniCrmApi.Models
{
    public class Product
    {
        public int Id { get; set; }
        public string Name { get; set; } = string.Empty;
        public string Description { get; set; } = string.Empty;
        public int StockQuantity { get; set; }
        public decimal Price { get; set; }

        //Foreign Key
        public int CategoryId { get; set; }
        
        //Navigation Property
        public Category? Category { get; set; }

        public List<OrderDetail> OrderDetails { get; set; } = new();
    }
}
