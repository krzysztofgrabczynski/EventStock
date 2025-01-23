using EventStock.Domain.Models;
using System.ComponentModel.DataAnnotations;

namespace EventStock.Application.Dto.Stock
{
    public class AddUserToStockDto
    {
        [Required]
        public int StockId { get; set; }
        [Required]
        public string UserId { get; set; }
        [Required]
        public Role Role { get; set; }
    }
}
