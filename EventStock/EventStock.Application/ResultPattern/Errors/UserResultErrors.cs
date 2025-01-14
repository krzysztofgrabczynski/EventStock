namespace EventStock.Application.ResultPattern.Errors
{
    public record UserNotFoundResultError : ResultError
    {
        public UserNotFoundResultError() : base("UserNotFound", "User with provided ID not found") { }
    }
}
