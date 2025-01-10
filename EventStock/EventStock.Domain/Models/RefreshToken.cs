namespace EventStock.Domain.Models
{
    public class RefreshToken
    {
        public string Id { get; set; } = Guid.NewGuid().ToString();
        public required string UserId { get; set; }
        public required string Token { get; set; }
        public DateTime CreationAt { get; set; } = DateTime.UtcNow;
        public required DateTime Expiration { get; set; }
    }
}
