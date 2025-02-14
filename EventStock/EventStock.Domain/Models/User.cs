using Microsoft.AspNetCore.Identity;

namespace EventStock.Domain.Models
{
    public class User : IdentityUser
    {
        public required string FirstName { get; set; }
        public required string LastName { get; set; }

        public int? StockId { get; set; }
        public Stock? Stock { get; set; }
    }
}
