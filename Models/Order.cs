using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace MiniCrmApi.Models
{
    public class Order
    {
        [Key]
        public int Id { get; set; }

        [Required(ErrorMessage = "Name is required")]
        [StringLength(100, MinimumLength = 3, ErrorMessage = "Name must be between 3 and 100 characters.")]
        public string Name { get; set; } = string.Empty;

        [StringLength(100, MinimumLength = 10, ErrorMessage = "Description must be between 10 and 100 characters.")]
        public string? Description { get; set; }

        public decimal TotalPrice { get; set; }

        //Foreign Key
        public int CustomerId { get; set; }

        //Navigation Property
        public Customer? Customer { get; set; }

        public List<OrderDetail> OrderDetails { get; set; } = new();

    }
}
