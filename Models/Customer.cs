using System.ComponentModel.DataAnnotations;

namespace MiniCrmApi.Models
{
    public class Customer
    {
        [Key]
        public int Id { get; set; }

        [Required(ErrorMessage = "Name is required.")]
        [StringLength(100, MinimumLength =3, ErrorMessage = "Name must be between 3 and 100 characters.")]
        public string Name { get; set; } = string.Empty;

        [Required(ErrorMessage = "Address is required.")]
        [StringLength(100, MinimumLength = 10, ErrorMessage = "Address must be between 10 and 100 characters.")]
        public string Address { get; set; } = string.Empty;

        [Required(ErrorMessage = "City is required.")]
        [StringLength(50, MinimumLength = 5, ErrorMessage = "City must be between 5 and 100 characters.")]
        public string City { get; set; } = string.Empty;

        [Required(ErrorMessage = "PhoneNumber is required.")]
        [StringLength(15, MinimumLength = 10, ErrorMessage = "PhoneNumber must be 15 characters.")]
        [Phone(ErrorMessage = "Invalid phone number format.")]
        public string PhoneNumber { get; set; } = string.Empty;

        //Bir müşteri birden fazla sipariş verebilir. OneToMany
        //Many tarafta foreign key, One tarafta liste.
        public List<Order> Orders { get; set; } = new List<Order>();
    }
}
