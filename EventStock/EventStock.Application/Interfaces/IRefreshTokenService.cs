namespace EventStock.Application.Interfaces
{
    public interface IRefreshTokenService
    {
        Task<string> GenerateRefreshTokenAsync(string userId);
    }
}
