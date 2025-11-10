using System.ComponentModel.DataAnnotations;

namespace MiniCrmApi.Dtos
{
    public record ProductRequestDto
    {
        [Required(ErrorMessage = "Name isn't empty.")]
        [StringLength(30, MinimumLength = 3, ErrorMessage = "Name length is between 3 to 30")]
        public string Name { get; set; } = string.Empty;
        [MaxLength(50, ErrorMessage = "Description cannot exceed 50 characters")]
        public string Description { get; set; } = string.Empty;
        [Required(ErrorMessage = "StockQuantity is required")]
        public int StockQuantity { get; set; }
        [Required(ErrorMessage = "Price is required")]
        public decimal Price { get; set; }
        [Range(1, int.MaxValue, ErrorMessage = "CategoryId must be greater than 0")]
        public int CategoryId { get; set; }
    }
}
