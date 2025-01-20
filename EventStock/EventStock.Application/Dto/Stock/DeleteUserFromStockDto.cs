using System.ComponentModel.DataAnnotations;

namespace EventStock.Application.Dto.Stock
{
    public class DeleteUserFromStockDto
    {
        [Required]
        public int StockId { get; set; }
        [Required]
        public string UserId { get; set; }
    }
}
