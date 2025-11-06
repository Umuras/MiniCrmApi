using System.ComponentModel.DataAnnotations;

namespace MiniCrmApi.Models
{
    public class OrderDetail
    {
        [Key]
        public int Id { get; set; }

        [Required(ErrorMessage = "Price is required.")]
        [MinLength(1, ErrorMessage = "Price cannot be 0.")]
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
