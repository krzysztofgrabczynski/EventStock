using Microsoft.AspNetCore.Identity;

namespace EventStock.Domain.Models
{
    public  class UserStockRole
    {
        public int Id { get; set; }

        public required User User { get; set; }
        public required Stock Stock { get; set; }
        public required IdentityRole Role { get; set; }
    }
}
