namespace HiremeAuthGate.BusinessModel.Results
{
    /// <summary>
    /// Generic result wrapper for operations that return a value.
    /// Provides a consistent way to handle success and failure scenarios.
    /// </summary>
    /// <typeparam name="T">The type of the value returned on success.</typeparam>
    public class Result<T>
    {
        public bool IsSuccess { get; }

        public T? Value { get; }

        public string? ErrorMessage { get; }

        public Exception? Exception { get; }

        /// <summary>
        /// Private constructor to enforce factory method usage.
        /// </summary>
        /// <param name="isSuccess">Whether the operation was successful.</param>
        /// <param name="value">The value returned on success.</param>
        /// <param name="errorMessage">The error message on failure.</param>
        /// <param name="exception">The exception that occurred on failure.</param>
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

        /// <summary>
        /// Maps the result to a new type using the specified mapper function.
        /// If the original result was successful, applies the mapper to the value.
        /// If the original result failed, returns a failed result with the same error.
        /// </summary>
        /// <typeparam name="TNew">The new type to map to.</typeparam>
        /// <param name="mapper">The function to transform the value.</param>
        /// <returns>A new result with the mapped value or the original error.</returns>
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
