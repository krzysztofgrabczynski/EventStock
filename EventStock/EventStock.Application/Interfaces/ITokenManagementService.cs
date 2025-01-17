using EventStock.Application.Dto.Token;
using EventStock.Application.ResultPattern;
using EventStock.Domain.Models;

namespace EventStock.Application.Interfaces
{
    public interface ITokenManagementService
    {
        Task<Result<TokensResultDto>> GenerateTokensAsync(string userId);
        Task<Result<TokensResultDto>> GenerateTokensAsync(RefreshToken refreshToken, string userId);
        Task<Result<RefreshToken>> GetRefreshTokenAsync(string refreshToken, bool needHash);
        Result CheckRefreshTokenExpiration(RefreshToken tokneFromDB);
        Task DeleteRefreshTokenAsync(string refreshToken, bool needHash);
        Task RevokeRefreshTokensAsync(string userId);
    }
}
