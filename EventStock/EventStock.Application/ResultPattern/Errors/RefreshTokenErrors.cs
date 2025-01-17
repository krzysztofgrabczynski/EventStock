namespace EventStock.Application.ResultPattern.Errors
{
    public record InvalidRefreshTokenResultError : ResultError
    {
        public InvalidRefreshTokenResultError() : base("InvalidRefreshToken", "Invalid refresh token") { }
    }

    public record RefreshTokenExpiredResultError : ResultError
    {
        public RefreshTokenExpiredResultError() : base("RefreshTokenExpired", "Refresh token expired") { }
    }
}
