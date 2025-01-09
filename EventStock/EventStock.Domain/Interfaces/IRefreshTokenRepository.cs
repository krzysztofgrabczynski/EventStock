namespace EventStock.Domain.Interfaces
{
    public interface IRefreshTokenRepository
    {
        Task SaveRefreshTokenAsync(string userId, string refreshToken);
    }
}
