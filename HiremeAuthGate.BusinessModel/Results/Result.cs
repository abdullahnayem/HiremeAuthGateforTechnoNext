namespace HiremeAuthGate.BusinessModel.Results
{
    public class Result<T>
    {
        public bool IsSuccess { get; }
        public T? Value { get; }
        public string? ErrorMessage { get; }
        public Exception? Exception { get; }

        private Result(bool isSuccess, T? value, string? errorMessage = null, Exception? exception = null)
        {
            IsSuccess = isSuccess;
            Value = value;
            ErrorMessage = errorMessage;
            Exception = exception;
        }

        public static Result<T> Success(T value) => new(true, value);
        public static Result<T> Failure(string errorMessage) => new(false, default, errorMessage);
        public static Result<T> Failure(string errorMessage, Exception exception) => new(false, default, errorMessage, exception);

        public Result<TNew> Map<TNew>(Func<T, TNew> mapper)
        {
            return IsSuccess && Value != null 
                ? Result<TNew>.Success(mapper(Value)) 
                : Result<TNew>.Failure(ErrorMessage ?? "Unknown error", Exception);
        }
    }

    public class Result
    {
        public bool IsSuccess { get; }
        public string? ErrorMessage { get; }
        public Exception? Exception { get; }

        private Result(bool isSuccess, string? errorMessage = null, Exception? exception = null)
        {
            IsSuccess = isSuccess;
            ErrorMessage = errorMessage;
            Exception = exception;
        }

        public static Result Success() => new(true);
        public static Result Failure(string errorMessage) => new(false, errorMessage);
        public static Result Failure(string errorMessage, Exception exception) => new(false, errorMessage, exception);
    }
}
