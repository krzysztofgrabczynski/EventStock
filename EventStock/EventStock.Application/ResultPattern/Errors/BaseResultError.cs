namespace EventStock.Application.ResultPattern.Errors
{
    public record ResultError(string Code, string Description)
    {
        public static readonly ResultError None = new(string.Empty, string.Empty);
    }
}
