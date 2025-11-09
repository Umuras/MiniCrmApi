using System.ComponentModel.DataAnnotations;

namespace MiniCrmApi.Dtos
{
    public record CategoryRequestDto
    {
        [Required(ErrorMessage = "CategoryName is required.")]
        public string CategoryName { get; set; } = string.Empty;
    }
}
