namespace MiniCrmApi.Models
{
    public class Customer
    {
        public int Id { get; set; }
        public string Name { get; set; } = string.Empty;
        public string Address { get; set; } = string.Empty;
        public string City { get; set; } = string.Empty;
        public string PhoneNumber { get; set; } = string.Empty;

        //Bir müşteri birden fazla sipariş verebilir. OneToMany
        //Many tarafta foreign key, One tarafta liste.
        public List<Order> Orders { get; set; } = new List<Order>();
    }
}
