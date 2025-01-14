using EventStock.Application.ResultPattern.Errors;

namespace EventStock.Application.ResultPattern
{
    public class Result<T>
    {
        private readonly T? _value;

        private Result(T value)
        {
            Value = value;
            Succeeded = true;
            Error = ResultError.None;
        }

        private Result(ResultError error)
        {
            if (error == ResultError.None)
            {
                throw new ArgumentException("Invalid error in Result class", nameof(error));
            }

            Succeeded = false;
            Error = error;
        }

        public static Result<T> Success(T value) => new Result<T>(value);

        public static Result<T> Failure(ResultError error) => new Result<T>(error);

        public bool Succeeded { get; }
        public T Value 
        {
            get
            {
                if (!Succeeded)
                {
                    throw new InvalidOperationException("There is no value for failuer in Result class");
                }
                return _value!;
            }
            private init => _value = value;
        }
        public ResultError Error { get; }
    }

    public class Result
    {
        private Result()
        {
            Succeeded = true;
            Error = ResultError.None;
        }

        private Result(ResultError error)
        {
            if (error == ResultError.None)
            {
                throw new ArgumentException("Invalid error in Result class", nameof(error));
            }

            Succeeded = false;
            Error = error;
        }

        public static Result Success() => new Result();

        public static Result Failure(ResultError error) => new Result(error);

        public bool Succeeded { get; }
        public ResultError Error { get; }
    }
}
