namespace MiniCrmApi.Models
{
    public class Order
    {
        public int Id { get; set; }
        public string Name { get; set; } = string.Empty;
        public string Description { get; set; } = string.Empty;
        public decimal Price { get; set; }

        //Foreign Key
        public int CustomerId { get; set; }

        //Navigation Property
        public Customer? Customer { get; set; }

        public List<OrderDetail> OrderDetails { get; set; } = new();
    }
}
