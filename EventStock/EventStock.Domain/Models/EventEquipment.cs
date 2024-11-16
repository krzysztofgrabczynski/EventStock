namespace EventStock.Domain.Models
{
    public class EventEquipment
    {
        public int Id { get; set; }
        
        public int EventId { get; set; }
        public required Event Event { get; set; }

        public required ICollection<Equipment> Equipments { get; set; }
    }
}
