namespace MiniCrmApi.Models
{
    public class OrderDetail
    {
        public int Id { get; set; }
        public decimal Price { get; set; }

        //Foreign Key
        public int OrderId { get; set; }
        //Navigation Property
        public Order? Order { get; set; }

        //Foreign Key
        public int ProductId { get; set; }
        //Navigation Property
        public Product? Product { get; set; }
    }
}
