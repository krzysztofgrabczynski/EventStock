using Microsoft.AspNetCore.Identity;

namespace EventStock.Domain.Models
{
    public  class UserStockRole
    {
        public int UserId { get; set; }
        public required User User { get; set; }
        
        public int StockId { get; set; }
        public required Stock Stock { get; set; }

        public required IdentityRole Role { get; set; }
    }
}
