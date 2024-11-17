namespace EventStock.Domain.Models
{
    public class EventEquipment
    {
        public int Id { get; set; }
        public int Quantity { get; set; }

        public int EquipmentId { get; set; }
        public int EventId { get; set; }

        public required Equipment Equipment { get; set; }
        public required Event Event { get; set; }
    }
}
