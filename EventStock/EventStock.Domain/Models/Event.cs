namespace EventStock.Domain.Models
{
    public class Event
    {
        public int Id { get; set; }
        public required string Name { get; set; }

        public int? IdAddress { get; set; }
        public Address? Address { get; set; }

        public DateTime? StartDate { get; set; }
        public DateTime? EndDate { get; set; }

        public required string Organizer { get; set; }
        public required User ResponsibleEmploee { get; set; }
        public ICollection<User>? Employees { get; set; }
        

        public int? IdAccomodation { get; set; }
        public Address? Accomodation { get; set; }

        public int IdStock { get; set; }
        public required Stock Stock { get; set; }

        public int EquipmentId { get; set; }
        public required EventEquipment Equipment { get; set; }

        public EventStatus Status { get; set; } = EventStatus.Scheduled;
    }
}
