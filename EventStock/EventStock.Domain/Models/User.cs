using Microsoft.AspNetCore.Identity;


namespace EventStock.Domain.Models
{
    public class User : IdentityUser
    {
        public required string FirstName { get; set; }
        public required string LastName { get; set; }
        public ICollection<Stock>? Stocks { get; set; }
        public ICollection<UserStockRole>? UserStockRole { get; set; }
    }
}
