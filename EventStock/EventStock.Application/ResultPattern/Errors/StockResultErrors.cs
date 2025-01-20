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

    public record StockDoesNotExistResultError : ResultError
    {
        public StockDoesNotExistResultError() : base("StockDoesNotExist", "Stock with provided ID does not exist") { }
    }

    public record PermissionDeniedResultError : ResultError
    {
        public PermissionDeniedResultError() : base("PermissionDenied", "You do not have permission to access that resource") { }
    }

    public record UserNotExistInStockResultError : ResultError
    {
        public UserNotExistInStockResultError() : base("UserNotExistInStock", "User with provided ID does not exist in stock") { }
    }
}

