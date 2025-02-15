namespace EventStock.Domain.Models
{
    public class Event
    {
        public int Id { get; set; }
        public required string Name { get; set; }
        public Address? Address { get; set; }

        public DateTime? StartDate { get; set; }
        public DateTime? EndDate { get; set; }

        public required string Organizer { get; set; }
        public required User ResponsibleEmploee { get; set; }
        public ICollection<User> Employees { get; set; } = new List<User>();
        public Address? Accomodation { get; set; }
        public required Stock Stock { get; set; }
        public EventStatus Status { get; set; } = EventStatus.Scheduled;
    }
}
