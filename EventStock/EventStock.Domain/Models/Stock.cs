namespace EventStock.Domain.Models
{
    public class Stock
    {
        public int Id { get; set; }
        public required string Name { get; set; }

        public Address? Address { get; set; }
        public ICollection<User> Users { get; set; } = new List<User>();
    }
}
