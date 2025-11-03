using System.ComponentModel.DataAnnotations.Schema;

namespace MiniCrmApi.Models
{
    public class Order
    {
        public int Id { get; set; }
        public string Name { get; set; } = string.Empty;
        public string Description { get; set; } = string.Empty;

        //Foreign Key
        public int CustomerId { get; set; }

        //Navigation Property
        public Customer? Customer { get; set; }

        public List<OrderDetail> OrderDetails { get; set; } = new();

        //Order tablosunda Price kolonu oluşmasın diye yazıyoruz.
        [NotMapped]
        public decimal Price => OrderDetails.Sum(d => d.Price);
    }
}
