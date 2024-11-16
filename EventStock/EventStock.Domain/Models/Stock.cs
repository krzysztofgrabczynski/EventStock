namespace EventStock.Domain.Models
{
    public class Stock
    {
        public int Id { get; set; }
        public required string Name { get; set; }

        public int? AddressId { get; set; }
        public Address? Address { get; set; }

        public ICollection<User>? Users { get; set; }
        public ICollection<UserStockRole>? UserStockRoles { get; set; }
        public required ICollection<Equipment>? Equipments { get; set; }
    }
}
