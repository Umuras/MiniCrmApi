using MiniCrmApi.Enums;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace MiniCrmApi.Models
{
    public class Order
    {
        [Key]
        public int Id { get; set; }

        public decimal TotalPrice { get; set; }

        public int TotalQuantity { get; set; }

        public DateTime CreatedDate { get; set; } = DateTime.Now;

        public string Status { get; set; } = OrderStatus.Pending.ToString();

        //Foreign Key
        public int CustomerId { get; set; }

        //Navigation Property
        public Customer? Customer { get; set; }

        public List<OrderDetail> OrderDetails { get; set; } = new();

    }
}
