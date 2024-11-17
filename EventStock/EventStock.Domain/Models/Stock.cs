namespace EventStock.Domain.Models
{
    public class Stock
    {
        public int Id { get; set; }
        public required string Name { get; set; }

        public Address? Address { get; set; }
        public ICollection<User> Users { get; set; } = new List<User>();
        public ICollection<UserStockRole> UserStockRoles { get; set; } = new List<UserStockRole>();
        public ICollection<Equipment> Equipments { get; set; } = new List<Equipment>();
    }
}
