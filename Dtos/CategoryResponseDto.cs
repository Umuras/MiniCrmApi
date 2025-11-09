namespace MiniCrmApi.Dtos
{
    public record CategoryResponseDto
    {
        public int CategoryId { get; set; }
        public string CategoryName { get; set; } = String.Empty;
    }
}
