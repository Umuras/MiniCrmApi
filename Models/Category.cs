using System.ComponentModel.DataAnnotations;

namespace MiniCrmApi.Models
{
    public class Category
    {
        [Key]
        public int Id { get; set; }
        [Required(ErrorMessage = "Category name is required.")]
        [StringLength(50,MinimumLength = 10, ErrorMessage = "Category name length should be between 10 to 50.")]
        public string Name { get; set; } = string.Empty;

        //Navigation Property
        public List<Product> Products { get; set; } = new List<Product>();
    }
}
