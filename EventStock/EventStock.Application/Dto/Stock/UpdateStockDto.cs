using EventStock.Domain.Models;

namespace EventStock.Application.Dto.Stock
{
    public class UpdateStockDto
    {
        public string? Name { get; set; }
        public Address? Address { get; set; }
    }
}
