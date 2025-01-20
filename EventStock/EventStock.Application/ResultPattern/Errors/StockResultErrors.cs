namespace EventStock.Application.ResultPattern.Errors
{
    public record StockSavingResultError : ResultError
    {
        public StockSavingResultError() : base("StockSavingError", "An error occurred while saving new stock") { }
    }

    public record StockAddUserResultError : ResultError
    {
        public StockAddUserResultError() : base("StockAddUserError", "An error occurred while adding user to stock") { }
    }

    public record UserExistInStockResultError : ResultError
    {
        public UserExistInStockResultError() : base("UserExistInStock", "User is already assigned to this stock") { }
    }

    public record GetStockResultError : ResultError
    {
        public GetStockResultError() : base("GetStockError", "An error occurred while retrieving stock information") { }
    }

    public record PermissionDeniedResultError : ResultError
    {
        public PermissionDeniedResultError() : base("PermissionDenied", "You do not have permission to access that resource") { }
    }
}

