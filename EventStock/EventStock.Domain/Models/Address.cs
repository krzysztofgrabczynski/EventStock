namespace EventStock.Domain.Models
{
    public class Address
    {
        public int Id { get; set; }
        public required string Country { get; set; }
        public required string City { get; set; }
        public required string ZipCode { get; set; }
        public required string Street { get; set; }
        public required string BuildingNumber { get; set; }
        public int? FlatNumber { get; set; }
    }
}
