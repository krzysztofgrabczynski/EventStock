namespace EventStock.Domain.Models
{
    public class Equipment
    {
        public int Id { get; set; }
        public required string Name { get; set; }

        public int StockId { get; set; }
        public required Stock Stock { get; set; }
    
        public string? Brand { get; set; }
        public required Category Category { get; set; }
        public int Quantity { get; set; }
        public ICollection<EventEquipment>? EventEquipment { get; set; }
    }
}