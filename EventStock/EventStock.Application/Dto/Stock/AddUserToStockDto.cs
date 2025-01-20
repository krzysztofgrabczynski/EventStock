using System.ComponentModel.DataAnnotations;

namespace EventStock.Application.Dto.Stock
{
    public class AddUserToStockDto
    {
        [Required]
        public int StockId { get; set; }
        [Required]
        public string UserId { get; set; }
        
        public string? Role { get; set; }
    }
}
